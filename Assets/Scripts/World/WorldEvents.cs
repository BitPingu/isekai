using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WorldEvents : MonoBehaviour
{
    // Call other functions when scene changes
    public delegate void OnSceneChange();
    public OnSceneChange SceneChange;

    private TileGrid gen;
    private TilemapStructure groundMap, overworldMap;

    public GameObject player;
    public GameObject elf;
    public EnemySpawner spawner;

    private void Awake()
    {
        gen = GetComponent<TileGrid>();
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
    }

    private void OnEnable()
    {
        gen.WorldGen += StartEvents;
    }

    private void OnDisable()
    {
        gen.WorldGen -= StartEvents;
    }

    private void StartEvents()
    {
        Debug.Log("start world events");
        PlayerPosition playerp = FindObjectOfType<PlayerPosition>();
        playerp.Spawn();

        if (SceneManager.GetActiveScene().buildIndex == 1) 
        {
            // Attacked by slime
            if (TempData.tempDays == 0)
            {
                elf.SetActive(true);
                ElfPosition elfp = elf.GetComponent<ElfPosition>();
                elfp.Spawn();

                // Spawn enemy
                if (groundMap.GetTile((int)elfp.spawnPoint.x+1, (int)elfp.spawnPoint.y) == (int)GroundTileType.Land)
                {
                    spawner.spawnEnemy("Slime", new Vector3(elfp.spawnPoint.x+1, elfp.spawnPoint.y), true);
                }
                else if (groundMap.GetTile((int)elfp.spawnPoint.x-1, (int)elfp.spawnPoint.y) == (int)GroundTileType.Land)
                {
                    spawner.spawnEnemy("Slime", new Vector3(elfp.spawnPoint.x-1, elfp.spawnPoint.y), false);
                }
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && elf) 
        {
            ElfPosition elfp = elf.GetComponent<ElfPosition>();
            if (elfp.inDanger && TempData.tempTime >= 60f)
            {
                // relese enemy
                GameObject enemy = GameObject.FindGameObjectWithTag("SpecialEnemy");
                enemy.tag = "Enemy";
                enemy.GetComponent<NPCMovement>().enabled = true;
                enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                enemy.GetComponent<Animator>().SetBool("Attack", false);
                Debug.Log("elf was slain!");
                Destroy(elf);
            }
        }
    }

    // Change scene based on current tile
    public void EnterBuilding(PlayerPosition position)
    {
        switch (position.currentOTile)
        {
            case (int)BuildingTileType.House:
                FindObjectOfType<AudioManager>().Stop();
                if (SceneManager.GetActiveScene().buildIndex == 1) 
                {
                    TempData.tempSpawnPoint = new Vector3(position.currentPos.x + .5f, position.currentPos.y + .4f);
                    GetComponentInChildren<FogData>().SaveFog();
                    Debug.Log("Enter Village");
                    SceneManager.LoadScene("Village");
                }
                else
                {
                    TempData.tempFog2 = FindObjectOfType<FogData>();;
                    Debug.Log("Exit Village");
                    SceneManager.LoadScene("Overworld");
                }
                break;
            case (int)BuildingTileType.Dungeon:
                FindObjectOfType<AudioManager>().Stop();
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    TempData.tempSpawnPoint = new Vector3(position.currentPos.x + .5f, position.currentPos.y + .4f);
                    GetComponentInChildren<FogData>().SaveFog();
                    Debug.Log("Enter Dungeon");
                    SceneManager.LoadScene("Dungeon");
                }
                else
                {
                    TempData.tempFog2 = FindObjectOfType<FogData>();;
                    Debug.Log("Exit Dungeon");
                    SceneManager.LoadScene("Overworld");
                }
                break;
            default:
                Debug.Log("No interactable tile!");
                break;
        }
    }
}
