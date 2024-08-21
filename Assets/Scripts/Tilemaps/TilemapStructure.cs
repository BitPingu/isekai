using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TilemapType
{
    Sea,
    Ground,
    Lake,
    Cliff,
    Village,
    Dungeon,
    DungeonUnderground,
    Fog,
    FogUnderground
}

public class TilemapStructure : MonoBehaviour
{
    [SerializeField]
    private TilemapType _type;
    public TilemapType type { get { return _type;  } }

    [HideInInspector]
    public int width, height;

    private int[] tiles;
    private Tilemap graphicMap;

    [HideInInspector]
    public TileGrid grid;

    private HashSet<Vector2Int> dirtyCoords = new HashSet<Vector2Int>();

    // True if changes are done in the structure
    public bool IsDirty => dirtyCoords.Count > 0;

    // Method to initialize tilemap
    public void Initialize(TileGrid parentGrid, int gridWidth, int gridHeight, int worldSeed)
    {
        // Retrieve the Tilemap component from the same object this script is attached to
        graphicMap = GetComponent<Tilemap>();

        // Retrieve the TileGrid component from parent gameObject
        grid = parentGrid;

        // Get width, height from parent
        width = gridWidth;
        height = gridHeight;

        // Initialize one-dimensional array with map size
        tiles = new int[width * height];

        // Apply all algorithms to tilemap
        if (type == TilemapType.Sea)
        {
            GetComponent<GroundGeneration>().Initialize(this);
        }
        else if (type == TilemapType.Ground)
        {
            GetComponent<GroundGeneration>().Initialize(this);
        }
        else if (type == TilemapType.Lake)
        {
            GetComponent<LakeGeneration>().Initialize(this);
        }
        else if (type == TilemapType.Cliff)
        {
            GetComponent<CliffGeneration>().Initialize(this);
        }
        else if (type == TilemapType.Fog || type == TilemapType.FogUnderground)
        {
            GetComponent<FogGeneration>().Initialize(this);
        }

        // Render data
        UpdateTiles();
    }

    // Updates a specific portion of the structure, ie. one tile
    public void UpdateTile(int x, int y)
    {
        // Remove coordinate from dirty list
        var coord = new Vector2Int(x, y);
        if (dirtyCoords.Contains(coord))
            dirtyCoords.Remove(coord);

        var typeOfTile = GetTile(x, y);

        // Get ScriptableObject that matches this type and insert it
        grid.GetTileCache().TryGetValue(typeOfTile, out TileBase tile); // Default return null if not found

        // Update tilemap
        graphicMap.SetTile(new Vector3Int(x, y, 0), tile);
        graphicMap.RefreshTile(new Vector3Int(x, y, 0));
    }

    // Updates a select few positions of the structure, ie. few tiles
    public void UpdateTiles(Vector2Int[] positions)
    {
        var positionsArray = new Vector3Int[positions.Length];
        var tilesArray = new TileBase[positions.Length];
        for (int i=0; i<positions.Length; i++)
        {
            var typeOfTile = GetTile(positions[i].x, positions[i].y);

            // Get ScriptableObject that matches this type and insert it
            grid.GetTileCache().TryGetValue(typeOfTile, out TileBase tile); // Default return null if not found
            positionsArray[i] = new Vector3Int(positions[i].x, positions[i].y, 0);
            tilesArray[i] = tile;

            // Remove coordinate from dirty list
            if (dirtyCoords.Contains(positions[i]))
            {
                dirtyCoords.Remove(positions[i]);
            }
        }

        // Update tilemap
        graphicMap.SetTiles(positionsArray, tilesArray);
        foreach (var position in positionsArray)
            graphicMap.RefreshTile(position);
    }

    // Updates the entire structure, only used on map initialization
    public void UpdateTiles()
    {
        // Create a positions array and tile array required by graphicMap.SetTiles
        var positionsArray = new Vector3Int[width * height];
        var tilesArray = new TileBase[width * height];

        // Loop over all tiles in data structure
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Add the position at the same index position as the Tile
                positionsArray[x * width + y] = new Vector3Int(x, y, 0);
                // Get what tile is at this position
                var typeOfTile = GetTile(x, y);
                // Get the ScriptableObject that matches this type and insert it
                grid.GetTileCache().TryGetValue(typeOfTile, out TileBase tile); // Default return null if not found
                tilesArray[x * width + y] = tile;
            }
        }

        // Clear all dirty coordinates
        dirtyCoords.Clear();

        // Update tilemap
        graphicMap.SetTiles(positionsArray, tilesArray);
        graphicMap.RefreshAllTiles();
    }

    // Returns all 8 neightbors (vertical, horizontal, diagonal)
    public Dictionary<Vector2Int, int> GetNeighbors(int tileX, int tileY)
    {
        int startX = tileX - 1;
        int startY = tileY - 1;
        int endX = tileX + 1;
        int endY = tileY + 1;

        var neighbors = new Dictionary<Vector2Int, int>();
        for (int x=startX; x<endX+1; x++)
        {
            for (int y=startY; y<endY+1; y++)
            {
                // Don't add the tile of neighbors
                if (x == tileX && y == tileY) continue;

                // Check if the tile is within tilemap, otherwise don't pass it along (invalid neighbor)
                if (InBounds(x, y))
                {
                    // Pass along a key value pair of the coordinate + the tile type
                    neighbors.Add(new Vector2Int(x, y), GetTile(x, y));
                }
            }
        }

        return neighbors;
    }

    // Returns only the direct 4 neighbors (horizontal and vertical)
    public List<KeyValuePair<Vector2Int, int>> Get4Neighbors(int tileX, int tileY)
    {
        int startX = tileX - 1;
        int startY = tileY - 1;
        int endX = tileX + 1;
        int endY = tileY + 1;

        var neighbors = new List<KeyValuePair<Vector2Int, int>>();
        for (int x = startX; x < endX + 1; x++)
        {
            if (x == tileX || !InBounds(x, tileY)) continue;
            neighbors.Add(new KeyValuePair<Vector2Int, int>(new Vector2Int(x, tileY), GetTile(x, tileY)));
        }
        for (int y = startY; y < endY + 1; y++)
        {
            if (y == tileY || !InBounds(tileX, y)) continue;
            neighbors.Add(new KeyValuePair<Vector2Int, int>(new Vector2Int(tileX, y), GetTile(tileX, y)));
        }

        return neighbors;
    }

    // Return type of tile, otherwise 0 if invalid position
    public int GetTile(int x, int y)
    {
        return InBounds(x, y) ? tiles[y * width + x] : 0;
    }

    // Set type of tile at the given position
    public void SetTile(int x, int y, int? value, bool updateTilemap = false, bool setDirty = true)
    {
        if (InBounds(x, y))
        {
            var prev = tiles[y * width + x];
            tiles[y * width + x] = value ?? 0;

            // If tile was changed, update
            if (prev != value)
            {
                // Add dirty coordinate to list, if modified and not yet dirty
                if (!updateTilemap && setDirty)
                {
                    var coord = new Vector2Int(x, y);
                    if (!dirtyCoords.Contains(coord))
                        dirtyCoords.Add(coord);
                }  
                if (updateTilemap)
                    UpdateTile(x, y);
            }
        }
    }

    // Check if the tile position is valid
    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
