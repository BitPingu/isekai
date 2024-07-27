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

    public GameObject elf;
    public EnemySpawner spawner;

    private void Awake()
    {
        gen = GetComponent<TileGrid>();
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
    }

    private void OnEnable()
    {
        // Start world events after world gen
        gen.WorldGen += StartEvents;
    }

    private void OnDisable()
    {
        gen.WorldGen -= StartEvents;
    }

    private void Update()
    {
        // Check if elf event is ongoing
        if (SceneManager.GetActiveScene().buildIndex == 1 && TempData.initElf) 
        {
            // End elf event
            if (!TempData.elfSaved && TempData.tempTime >= 60f)
            {
                TempData.initElf = false;
                // relese enemy
                GameObject enemy = GameObject.FindGameObjectWithTag("SpecialEnemy");
                enemy.tag = "Enemy";
                enemy.GetComponent<NPCMovement>().enabled = true;
                enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                enemy.GetComponent<Animator>().SetBool("Battle", false);
                if (FindObjectOfType<DialogueController>().issuer.Equals("Helpless Elf"))
                    FindObjectOfType<DialogueController>().EndDialogue();
                Debug.Log("elf was slain!");
                Destroy(elf);
            }
        }
    }

    private void StartEvents()
    {
        // Spawn player
        FindObjectOfType<PlayerPosition>().Spawn(TempData.initPlayerSpawn, SceneManager.GetActiveScene().buildIndex);
        if (TempData.initPlayerSpawn)
            TempData.initPlayerSpawn = false;
        // Spawn elf
        if (TempData.newGame && TempData.initElfSpawn)
            TempData.elfSaved = false;
        if (TempData.elfSaved)
        {
            if (TempData.initElfSpawn)
                elf.SetActive(true);
            FindObjectOfType<ElfPosition>().Spawn(TempData.initElfSpawn, SceneManager.GetActiveScene().buildIndex);
            if (TempData.initElfSpawn)
                TempData.initElfSpawn = false;
        }

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                // Overworld events
                if (TempData.initElf && TempData.tempDays == 0 && !TempData.elfSaved)
                    ElfEvent();
                break;
            case 2:
                // Village events
                break;
            case 3:
                // Dungeon events
                break;
            default:
                Debug.Log("no events for this scene.");
                break;
        }
    }

    private void ElfEvent()
    {
        // Spawn elf
        elf.SetActive(true);
        ElfPosition elfp = elf.GetComponent<ElfPosition>();
        elfp.Spawn(TempData.initElfSpawn, SceneManager.GetActiveScene().buildIndex);
        if (TempData.initElfSpawn)
                TempData.initElfSpawn = false;

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
                TempData.tempFog2 = FindObjectOfType<FogData>();;
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
                TempData.tempFog2 = FindObjectOfType<FogData>();;
                SceneManager.LoadScene("Overworld");
            }
        }
        else
        {
            Debug.Log("No interactable tile!");
        }
    }
}
