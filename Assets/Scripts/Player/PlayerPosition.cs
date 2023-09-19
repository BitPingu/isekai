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
    public delegate void OnPosChanged();
    public OnPosChanged PosChange;

    // Call other functions when tile types change
    public delegate void OnGTileChange();
    public OnGTileChange GTileChange;
    public delegate void OnOTileChange();
    public OnOTileChange OTileChange;

    [SerializeField]
    private TileGrid grid;
    [SerializeField]
    private Vector3 spawnPoint;

    private TilemapStructure groundMap, overworldMap;
    private List<KeyValuePair<Vector2Int, int>> neighbours;
    private int xCoord, yCoord, currentTile;

    private void Awake()
    {
        Debug.Log("After Scene: " + SceneManager.GetActiveScene().buildIndex);
        // Retrieve tilemap components
        groundMap = grid.GetTilemap(TilemapType.Ground);
        overworldMap = grid.GetTilemap(TilemapType.Overworld);

        // Attach delegates
        PosChange += CheckPosition;
        PosChange += OTileSound;
        PosChange += CheckNearby;

        if (MainMenu.loadGame)
        {
            // Load player position
            spawnPoint.x = SaveSystem.LoadPlayer().savedPosX + .5f;
            spawnPoint.y = SaveSystem.LoadPlayer().savedPosY + .5f;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Generate spawn point
            GenerateSpawnPoint();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            spawnPoint = new Vector3(grid.width / 2, 0.5f);
        }
    }

    private void Start()
    {
        // Set spawn point
        transform.position = spawnPoint;
        prevPos = currentPos = Vector2Int.FloorToInt(transform.position);

        // Retrieve spawn point tile
        prevGTile = currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        prevOTile = currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Clear fog
        PosChange();
    }

    private void Update()
    {
        // Retrieve coordinates of player
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }

    // Generates a random spawn point
    private void GenerateSpawnPoint()
    {
        // Retrieve tilemap component
        groundMap = grid.GetTilemap(TilemapType.Ground);

        do
        {
            // Choose random spawn point
            xCoord = Random.Range(0, grid.width);
            yCoord = Random.Range(0, grid.height);

            // Check tile
            currentTile = groundMap.GetTile(xCoord, yCoord);
        }
        while (currentTile != (int)GroundTileType.Land);

        // Generate spawn point
        spawnPoint = new Vector3(xCoord + .5f, yCoord + .5f);
    }

    // Looks at current position
    private void CheckPosition()
    {
        // Get current tiles from player position
        currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Ground tile check
        if (currentGTile != prevGTile)
        {
            GTileChange();
            prevGTile = currentGTile;
        }

        // Object tile check
        if (currentOTile != prevOTile)
        {
            OTileChange();
            prevOTile = currentOTile;
        }
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

    // Looks at adjacent tiles around player
    public void CheckNearby()
    {
        neighbours = groundMap.GetNeighbors(currentPos.x, currentPos.y);

        int landTiles = 0, villageTiles = 0, waterTiles = 0;

        foreach (var neighbour in neighbours)
        {
            if (neighbour.Value == (int)GroundTileType.Land)
                landTiles++;

            if (neighbour.Value == (int)GroundTileType.Water)
                waterTiles++;
        }

        // Compare water tiles with land tiles
        if (waterTiles >= landTiles - 2)
        {
            dominantTile = (int)GroundTileType.Water;
        }
        else if (waterTiles - 2 > villageTiles)
        {
            dominantTile = (int)GroundTileType.Land;
        }
    }
}
