using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid : MonoBehaviour
{
    public int Width, Height;
    public int TileSize, Seed;
    public Dictionary<int, Tile> Tiles { get; private set; }
    public Dictionary<TilemapType, TilemapStructure> Tilemaps;

    [Serializable]
    class GroundTiles
    {
        public GroundTileType TileType;
        public Texture2D Texture;
        public Color Color;
        public Tile Tile;
    }

    [Serializable]
    class ObjectTiles
    {
        public ObjectTileType TileType;
        public Texture2D Texture;
        public Color Color;
        public Tile Tile;
    }

    [SerializeField]
    private GroundTiles[] GroundTileTypes;
    [SerializeField]
    private ObjectTiles[] ObjectTileTypes;

    private void Awake()
    {
        // Generate random seed
        Seed = UnityEngine.Random.Range(-100000, 100000);

        Tiles = InitializeTiles();

        Tilemaps = new Dictionary<TilemapType, TilemapStructure>();

        // Add all tilemaps by type to collection for easy access
        foreach (Transform child in transform)
        {
            var tilemap = child.GetComponent<TilemapStructure>();
            if (tilemap == null) continue;
            if (Tilemaps.ContainsKey(tilemap.Type))
            {
                throw new Exception("Duplicate tilemap type: " + tilemap.Type);
            }
            Tilemaps.Add(tilemap.Type, tilemap);
        }

        // Initialize tilemaps from collection
        foreach (var tilemap in Tilemaps.Values)
        {
            tilemap.Initialize();
        }
    }

    private Dictionary<int, Tile> InitializeTiles()
    {
        var dictionary = new Dictionary<int, Tile>();

        // Create Tile for each GroundTileType
        foreach (var tileType in GroundTileTypes)
        {
            // Don't make a tile for empty
            if (tileType.TileType == 0) continue;
            // Create tile scriptable object (if custom, otherwise new)
            var tile = tileType.Tile == null ?
                CreateTile(tileType.Color, tileType.Texture) :
                tileType.Tile;
            // Add to dictionary by key int value, value Tile
            dictionary.Add((int)tileType.TileType, tile);
        }

        // Create Tile for each ObjectTileType
        foreach (var tileType in ObjectTileTypes)
        {
            // Don't make a tile for empty
            if (tileType.TileType == 0) continue;
            // Create tile scriptable object (if custom, otherwise new)
            var tile = tileType.Tile == null ?
                CreateTile(tileType.Color, tileType.Texture) :
                tileType.Tile;
            // Add to dictionary by key int value, value Tile
            dictionary.Add((int)tileType.TileType, tile);
        }

        return dictionary;
    }

    private Tile CreateTile(Color color, Texture2D texture)
    {
        // Create empty color tile if not specified
        bool setColor = false;
        if (texture == null)
        {
            setColor = true;
            texture = new Texture2D(TileSize, TileSize);
        }

        // Use Point mode for most quality
        texture.filterMode = FilterMode.Point;

        // Create sprite with texture
        var sprite = Sprite.Create(texture, new Rect(0, 0, TileSize, TileSize), new Vector2(0.5f, 0.5f), TileSize);

        // Create scriptable object instance of type Tile (inherits from TileBase)
        var tile = ScriptableObject.CreateInstance<Tile>();

        if (setColor)
        {
            // Remove transparency
            color.a = 1;
            // Set tile color
            tile.color = color;
        }

        // Assign sprite
        tile.sprite = sprite;

        return tile;
    }
}
