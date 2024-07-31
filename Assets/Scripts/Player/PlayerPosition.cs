using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPosition : MonoBehaviour
{
    public Vector2Int prevPos;
    public Vector2Int currentPos;
    public int prevGTile, prevOTile;
    public int currentGTile, currentOTile;
    public int dominantTile;

    // Call other functions when player position changes
    public delegate void OnPosChange();
    public OnPosChange PosChange;

    [SerializeField]
    private Vector3 spawnPoint;

    private TilemapStructure groundMap;
    private List<KeyValuePair<Vector2Int, int>> neighbours;

    // public static PlayerPosition player;

    private void Awake()
    {
        // if (player == null)
        // {
        //     player = this;
        // } 
        // else 
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // DontDestroyOnLoad(gameObject);

        // Attach delegates
        // PosChange += CheckPosition;
        // PosChange += OTileSound;
        // PosChange += CheckNearby;
    }

    private void Update()
    {
        // Retrieve coordinates of player
        currentPos = Vector2Int.FloorToInt(transform.position);
        TempData.tempPlayerPos = new Vector3(transform.position.x, transform.position.y);

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }

    private void RetrieveTilemap()
    {
        // Retrieve tilemap components
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
    }

    // Generates a random spawn point
    public void Spawn()
    {
        // RetrieveTilemap();
        // switch (scene)
        // {
        //     case 1:
        //         if (initialSpawn)
        //         {
        //             // Coming from main menu
        //             if (TempData.newGame)
        //             {
        //                 // Generate initial spawn point
        //                 float xCoord, yCoord, currentTile;
        //                 do
        //                 {
        //                     // Choose random spawn point
        //                     xCoord = Random.Range(0f, TempData.tempWidth);
        //                     yCoord = Random.Range(0f, TempData.tempHeight);

        //                     // Check tile
        //                     currentTile = groundMap.GetTile((int)xCoord, (int)yCoord);
        //                 }
        //                 while (currentTile != (int)GroundTileType.Land);

        //                 // Generate spawn point
        //                 spawnPoint = new Vector3(xCoord, yCoord);
        //                 TempData.tempPlayerStartingSpawn = spawnPoint;
        //             }
        //             else
        //             {
        //                 // Load player position
        //                 SaveData saveData = SaveSystem.Load();
        //                 spawnPoint.x = saveData.savePlayerPos[0];
        //                 spawnPoint.y = saveData.savePlayerPos[1];
        //                 spawnPoint.z = saveData.savePlayerPos[2];
        //             }
        //         }
        //         else
        //         {
        //             // Still in game (exiting from building)
        //             spawnPoint = TempData.tempPlayerBuildingSpawn;
        //         }
        //         break;
        //     case 2:
        //     case 3:
        //         // Dungeon or village spawn
        //         spawnPoint = new Vector3(10 / 2, 0.5f);
        //         break;
        //     default:
        //         Debug.Log("cannot spawn!");
        //         break;
        // }

        // Set spawn point
        // transform.position = spawnPoint;
        // prevPos = currentPos = Vector2Int.FloorToInt(transform.position);
        // TempData.tempPlayerPos = new Vector3(spawnPoint.x, spawnPoint.y);

        // // Retrieve spawn point tile
        // prevGTile = currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        // prevOTile = currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // // Clear fog
        // PosChange();
    }

    // Play sound based on current object tile
    private void OTileSound()
    {
        // Get sound from current object tile
        if (currentOTile == (int)FoilageTileType.Tree)
        {
            FindObjectOfType<AudioManager>().PlayFx("Tree");
        }
    }
}
