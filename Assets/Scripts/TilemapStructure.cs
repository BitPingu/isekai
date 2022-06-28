using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TilemapType
{
    Ground,
    Object
}
public class TilemapStructure : MonoBehaviour
{
    [SerializeField]
    private TilemapType _type;
    public TilemapType Type { get { return _type;  } }

    [HideInInspector]
    public int Width, Height;

    public int[] _tiles;
    private Tilemap _graphicMap;

    [HideInInspector]
    public TileGrid Grid;

    [SerializeField]
    private AlgorithmBase[] _algorithms;

    // Method to initialize tilemap
    public void Initialize()
    {
        // Retrieve the Tilemap component from the same object this script is attached to
        _graphicMap = GetComponent<Tilemap>();

        // Retrieve the TileGrid component from parent gameObject
        Grid = transform.parent.GetComponent<TileGrid>();

        // Get width and height from parent
        Width = Grid.Width;
        Height = Grid.Height;

        // Initialize the one-dimensional array with map size
        _tiles = new int[Width * Height];

        // Apply all the algorithms to the tilemap
        foreach (var algorithm in _algorithms)
        {
            Generate(algorithm);
        }

        // Render data
        RenderAllTiles();
    }

    // Renders the entire data structure to unity's tilemap
    public void RenderAllTiles()
    {
        // Create a positions array and tile array required by _graphicMap.SetTiles
        var positionsArray = new Vector3Int[Width * Height];
        var tilesArray = new TileBase[Width * Height];

        // Loop over all tiles in data structure
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Add the position at the same index position as the Tile
                positionsArray[x * Width + y] = new Vector3Int(x, y, 0);
                // Get what tile is at this position
                var typeOfTile = GetTile(x, y);
                // Get the ScriptableObject that matches this type and insert it
                if (!Grid.Tiles.TryGetValue(typeOfTile, out TileBase tile))
                {
                    if (typeOfTile != 0)
                    {
                        Debug.LogError("Tile not defined for id: " + typeOfTile);
                    }

                    tilesArray[x * Width + y] = null;
                    continue;
                }
                tilesArray[x * Width + y] = tile;
            }
        }

        _graphicMap.SetTiles(positionsArray, tilesArray);
        _graphicMap.RefreshAllTiles();
    }

    public List<KeyValuePair<Vector2Int, int>> GetNeighbors(int tileX, int tileY)
    {
        int startX = tileX - 1;
        int startY = tileY - 1;
        int endX = tileX + 1;
        int endY = tileY + 1;

        var neighbors = new List<KeyValuePair<Vector2Int, int>>();
        for (int x=startX; x<endX+1; x++)
        {
            for (int y=startY; y<endY+1; y++)
            {
                // Don't add the tile of neighbors
                if (x == tileX && y == tileY) continue;

                // Check if the tile is within tilemap, otherwise don't pass it along
                if (InBounds(x, y))
                {
                    // Pass along a key value pair of the coordinate + the tile type
                    neighbors.Add(new KeyValuePair<Vector2Int, int>(new Vector2Int(x, y), GetTile(x, y)));
                }
            }
        }
        return neighbors;
    }

    // Return type of tile, otherwise 0 if invalid position
    public int GetTile(int x, int y)
    {
        return InBounds(x, y) ? _tiles[y * Width + x] : 0;
    }

    // Set type of tile at the given position
    public void SetTile(int x, int y, int value)
    {
        if (InBounds(x, y))
        {
            _tiles[y * Width + x] = value;
        }
    }

    // Check if the tile position is valid
    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public void Generate(AlgorithmBase algorithm)
    {
        algorithm.Apply(this);
    }

}
