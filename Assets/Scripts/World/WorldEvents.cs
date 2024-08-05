using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WorldEvents : MonoBehaviour
{
    public GameObject player, elf, audio;
    private GameObject p, e;
    public CameraController camera;

    private DayAndNightCycle time;

    public void Initialize(WorldGeneration world, TileGrid grid, DayAndNightCycle dayNight)
    {
        // Get time
        time = dayNight;

        // Init enemy spawner
        GetComponentInChildren<EnemySpawner>().Initialize(grid);

        // Init villager spawner
        GetComponentInChildren<VillagerSpawner>().Initialize(grid);

        // Attach dayNight delegates to villager spawner
        dayNight.DayTime += GetComponentInChildren<VillagerSpawner>().Spawn;
        dayNight.NightTime += GetComponentInChildren<VillagerSpawner>().Despawn;

        // Spawn player
        SpawnPlayer(grid, world);

        // Spawn elf
        SpawnElf(grid, p.transform);

        // Audio
        Instantiate(audio, Vector3.zero, Quaternion.identity);

        // Call dayNight delegates (and any methods tied to it)
        if (dayNight.isDay)
        {
            dayNight.DayTime();
        }
        else
        {
            dayNight.NightTime();
        }

        if (TempData.loadGame)
            TempData.elfSaved = SaveSystem.Load().saveElf;

        // Start elf event
        if (!TempData.elfSaved)
            ElfEvent(grid, e, GetComponentInChildren<EnemySpawner>());
    }

    private void SpawnPlayer(TileGrid grid, WorldGeneration world)
    {
        // Determine player spawn
        Vector3 spawnPoint = new Vector3();
        TilemapStructure groundMap = grid.GetTilemap(TilemapType.Ground);
        TilemapStructure lakeMap = grid.GetTilemap(TilemapType.Lake);
        TilemapStructure cliffMap = grid.GetTilemap(TilemapType.Cliff);
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
            while (groundMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Empty 
                || lakeMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Lake 
                    || cliffMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Cliff);

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
        TilemapStructure groundMap = grid.GetTilemap(TilemapType.Ground);
        TilemapStructure lakeMap = grid.GetTilemap(TilemapType.Lake);
        TilemapStructure cliffMap = grid.GetTilemap(TilemapType.Cliff);
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
            while (groundMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Empty 
                || lakeMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Lake 
                    || cliffMap.GetTile(Mathf.FloorToInt(xCoord), Mathf.FloorToInt(yCoord)) == (int)GroundTileType.Cliff);

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

    private void ElfEvent(TileGrid grid, GameObject elf, EnemySpawner spawner)
    {
        // Spawn enemy
        if (Random.Range(0,1) == 1)
        {
            spawner.spawnEnemy("Slime", new Vector3(elf.transform.position.x+1, elf.transform.position.y), true);
        }
        else
        {
            spawner.spawnEnemy("Slime", new Vector3(elf.transform.position.x-1, elf.transform.position.y), false);
        }

        // Attacked by slime
        elf.GetComponent<ElfPosition>().InDanger();
    }

    // Change scene based on nearby building to enter (via active icon)
    public void EnterBuilding()
    {
        PlayerPosition position = FindObjectOfType<PlayerPosition>();
        GameObject building;
        string buildingToEnter;
        if (GameObject.FindWithTag("Event"))
        {
            building = GameObject.FindWithTag("Event").transform.parent.gameObject;
            buildingToEnter = building.name;
            Debug.Log("enter building at " + building.transform.position);
        }
        else
        {
            buildingToEnter = "";
        }

        if (buildingToEnter.Contains("Village"))
        {
            // Stop current music  
            FindObjectOfType<AudioManager>().Stop();
            FindObjectOfType<AudioManager>().PlayFx("Enter");
            if (SceneManager.GetActiveScene().buildIndex == 1) 
            {
                // Enter house
                TempData.tempPlayerBuildingSpawn = new Vector3(position.currentPos.x + .5f, position.currentPos.y + .4f); // save player position when exit
                SceneManager.LoadScene("Village");
            }
            else
            {
                // Exit house
                SceneManager.LoadScene("Overworld");
            }
        }
        else if (buildingToEnter.Contains("Dungeon"))
        {
            // Stop current music  
            FindObjectOfType<AudioManager>().Stop();
            FindObjectOfType<AudioManager>().PlayFx("Enter");
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                // Enter dungeon
                TempData.tempPlayerBuildingSpawn = new Vector3(position.currentPos.x + .5f, position.currentPos.y + .4f); // save player position when exit
                SceneManager.LoadScene("Dungeon");
            }
            else
            {
                // Exit dungeon
                SceneManager.LoadScene("Overworld");
            }
        }
        else
        {
            Debug.Log("No interactable tile!");
        }
    }
}
