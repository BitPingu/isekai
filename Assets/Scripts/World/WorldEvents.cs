using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WorldEvents : MonoBehaviour
{
    public GameObject player, elf, audio, rain;
    private GameObject p, e, r;
    public CameraController camera;

    private DayAndNightCycle time;

    public void Initialize(WorldGeneration world, TileGrid grid, DayAndNightCycle dayNight)
    {
        // Get time
        time = dayNight;

        // Spawn player
        SpawnPlayer(grid, world);

        // Spawn elf
        SpawnElf(grid, p.transform);

        // Audio
        Instantiate(audio, Vector3.zero, Quaternion.identity);

        // Init enemy spawner
        GetComponentInChildren<EnemySpawner>().Initialize(grid, dayNight, p);

        // Attach dayNight delegates to villager spawner
        dayNight.DayTime += GetComponentInChildren<VillagerSpawner>().Spawn;
        dayNight.NightTime += GetComponentInChildren<VillagerSpawner>().Despawn;

        // Attach dayNight delegate to rain
        dayNight.DayTime += Rain;

        // Call dayNight delegates (and any methods tied to it)
        if (dayNight.isDay)
        {
            dayNight.DayTime();
        }
        else
        {
            dayNight.NightTime();
        }

        // Spawn enemies
        StartCoroutine(FindObjectOfType<EnemySpawner>().Spawn());
        FindObjectOfType<EnemySpawner>().SpawnGoblins();



        if (TempData.loadGame)
            TempData.elfSaved = SaveSystem.Load().saveElf;

        // Start elf event
        if (!TempData.elfSaved)
            ElfEvent(e, GetComponentInChildren<EnemySpawner>());


    }

    private void SpawnPlayer(TileGrid grid, WorldGeneration world)
    {
        // Determine player spawn
        Vector3 spawnPoint = new Vector3();
        if (TempData.loadGame)
        {
            spawnPoint.x = SaveSystem.Load().savePlayerPos[0];
            spawnPoint.y = SaveSystem.Load().savePlayerPos[1];
        }
        else
        {
            float xCoord, yCoord;
            do
            {
                // Choose random spawn point
                xCoord = Random.Range(0f, world.width);
                yCoord = Random.Range(0f, world.height);
            }
            while (!grid.CheckLand(new Vector2(xCoord, yCoord)) || !grid.CheckCliff(new Vector2(xCoord, yCoord)));

            // Generate spawn point
            spawnPoint = new Vector3(xCoord, yCoord);
        }

        // Init player
        p = Instantiate(player, spawnPoint, Quaternion.identity);

        p.GetComponent<PlayerPosition>().currentArea = "Overworld";

        // Attach clearfog delegate to player movement
        p.GetComponent<PlayerPosition>().PosChange += GetComponentInChildren<FogData>().ClearFog;

        // Make camera look at player
        camera.LookAt(p.transform);
    }

    private void SpawnElf(TileGrid grid, Transform player)
    {
        Vector3 spawnPoint = new Vector3();
        if (TempData.loadGame)
        {
            spawnPoint.x = SaveSystem.Load().saveElfPos[0];
            spawnPoint.y = SaveSystem.Load().saveElfPos[1];
        }
        else
        {
            float xCoord, yCoord;
            // Generate elf spawn point
            do
            {
                // Choose random spawn point around player
                xCoord = Random.Range(player.position.x-5, player.position.x+5);
                yCoord = Random.Range(player.position.y-5, player.position.y+5);
            }
            while (!grid.CheckLand(new Vector2(xCoord, yCoord)) || !grid.CheckCliff(new Vector2(xCoord, yCoord)));

            // Generate spawn point
            spawnPoint = new Vector3(xCoord, yCoord);
        }

        // Init elf
        e = Instantiate(elf, spawnPoint, Quaternion.identity);
    }

    private void Update()
    {
        // Check if elf event is ongoing
        if (!TempData.elfSaved && e && e.GetComponent<ElfPosition>().inDanger && time.days == 0 && time.time >= 60f)
        {
            // end event and relese enemy
            GameObject enemy = GameObject.FindGameObjectWithTag("SpecialEnemy");
            enemy.tag = "Enemy";
            enemy.GetComponent<NPCMovement>().enabled = true;
            enemy.GetComponent<Animator>().SetBool("Battle", false);
            if (FindObjectOfType<DialogueController>().issuer.Equals("Helpless Elf"))
                FindObjectOfType<DialogueController>().EndDialogue();
            Debug.Log("elf was slain!");
            Destroy(e);
        }


    }

    private void ElfEvent(GameObject elf, EnemySpawner spawner)
    {
        // Spawn enemy
        Debug.Log("start elf event");
        GameObject elfEnemy;
        var random = TempData.tempRandom;
        if (random.Next(0,2) == 1)
        {
            elfEnemy = spawner.spawnEnemy("Slime", new Vector3(elf.transform.position.x+1, elf.transform.position.y));
            elfEnemy.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            elfEnemy = spawner.spawnEnemy("Slime", new Vector3(elf.transform.position.x-1, elf.transform.position.y));
            elfEnemy.GetComponent<SpriteRenderer>().flipX = false;
        }
        elfEnemy.tag = "SpecialEnemy";
        elfEnemy.GetComponent<NPCMovement>().enabled = false;
        elfEnemy.GetComponent<Animator>().SetBool("Battle", true);

        // Attacked by slime
        elf.GetComponent<ElfPosition>().InDanger();
    }

    private void Rain()
    {
        if (r)
            Destroy(r);
            
        // Init rain
        var random = TempData.tempRandom;
        if (random.Next(0,2) == 1)
        {
            r = Instantiate(rain, Vector3.zero, Quaternion.identity, transform);
            r.GetComponent<RainController>().Initialize(p.transform);
        }
    }

    public void EnterDungeon(PlayerPosition player)
    {
        if (player.currentArea.Contains("Overworld Dungeon Entrance"))
        {
            // Disable overworld tilemaps
            foreach (Transform tilemap in GetComponentInChildren<TileGrid>().transform)
            {
                tilemap.gameObject.SetActive(false);
            }

            // Enable underground tilemap
            GetComponentInChildren<TileGrid>().transform.Find("DungeonUndergroundTilemap").gameObject.SetActive(true);
            // GetComponentInChildren<TileGrid>().transform.Find("FogTilemap").gameObject.SetActive(true);

            // Disable overworld chunks
            GameObject[] chu = GameObject.FindGameObjectsWithTag("Chunk");
            foreach (GameObject c in chu)
            {
                if (c.GetComponent<ChunkLoader>().containsPlayer)
                {
                    foreach (Transform child in c.transform)
                    {
                        if (!child.gameObject.name.Contains("Dungeon"))
                            child.gameObject.SetActive(false);
                    }
                }
            }

            // elf goes to player
            if (TempData.elfSaved)
                e.transform.position = p.transform.position;

            player.currentArea = "Underground Dungeon Entrance";

            // Despawn enemies
            FindObjectOfType<EnemySpawner>().despawnEnemies();

            // rain
            if (r)
                r.SetActive(false);
        }
        else if (player.currentArea.Contains("Underground Dungeon Entrance"))
        {
            // Enable overworld tilemaps
            foreach (Transform tilemap in GetComponentInChildren<TileGrid>().transform)
            {
                tilemap.gameObject.SetActive(true);
            }

            // Disable underground tilemap
            GetComponentInChildren<TileGrid>().transform.Find("DungeonUndergroundTilemap").gameObject.SetActive(false);

            // Enable overworld chunks
            GameObject[] chu = GameObject.FindGameObjectsWithTag("Chunk");
            foreach (GameObject c in chu)
            {
                if (c.GetComponent<ChunkLoader>().containsPlayer)
                {
                    foreach (Transform child in c.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }

            // elf goes to player
            if (TempData.elfSaved)
                e.transform.position = p.transform.position;

            player.currentArea = "Overworld Dungeon Entrance";

            // Spawn enemies
            StartCoroutine(FindObjectOfType<EnemySpawner>().Spawn());

            // rain
            if (r)
                r.SetActive(true);
        }
    }
}
