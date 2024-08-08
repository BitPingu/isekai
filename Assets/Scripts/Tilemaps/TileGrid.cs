using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid : MonoBehaviour
{
    [SerializeField]
    private int tileSize;
    [SerializeField]
    private bool randomize;

    [SerializeField]
    private TileTypes.GroundTiles[] groundTileTypes;

    private Dictionary<int, TileBase> tiles;
    private Dictionary<TilemapType, TilemapStructure> tilemaps;

    public void Initialize(int width, int height, int seed)
    {
        InitializeTiles();

        tilemaps = new Dictionary<TilemapType, TilemapStructure>();

        // Add all tilemaps (in children) by name to collection for easy access
        foreach (Transform child in transform)
        {
            var tilemap = child.GetComponent<TilemapStructure>();
            if (tilemap == null) continue;
            if (tilemaps.ContainsKey(tilemap.type))
            {
                throw new Exception("Duplicate tilemap type: " + tilemap.type);
            }
            tilemaps.Add(tilemap.type, tilemap);
        }

        // Initialize tilemaps in the collection
        foreach (var tilemap in tilemaps.Values)
        {
            tilemap.Initialize(this, width, height, seed);
        }
    }

    public bool CheckLand(Vector2 position)
    {
        TilemapStructure groundMap = GetTilemap(TilemapType.Ground);
        TilemapStructure lakeMap = groundMap.grid.GetTilemap(TilemapType.Lake);
        TilemapStructure dungeonMap = GetTilemap(TilemapType.Dungeon);
        var groundNeighbors = groundMap.GetNeighbors(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        var lakeNeighbors = lakeMap.GetNeighbors(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        var dungeonNeighbors = dungeonMap.GetNeighbors(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

        if (!groundNeighbors.ContainsValue((int)GroundTileType.Empty) && !lakeNeighbors.ContainsValue((int)GroundTileType.Lake) && !dungeonNeighbors.ContainsValue((int)GroundTileType.DungeonEntrance))
        {
            return true;
        }

        return false;
    }

    public bool CheckCliff(Vector2 position)
    {
        TilemapStructure cliffMap = GetTilemap(TilemapType.Cliff);
        var cliffNeighbors = cliffMap.GetNeighbors(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

        if (!cliffNeighbors.ContainsValue((int)GroundTileType.Cliff))
        {
            return true;
        }

        return false;
    }

    // Returns all cached shared tiles available to be placed on tilemap
    public Dictionary<int, TileBase> GetTileCache()
    {
        return tiles;
    }

    // Returns tilemap of given type
    public TilemapStructure GetTilemap(TilemapType type)
    {
        if (!tilemaps.TryGetValue(type, out TilemapStructure structure))
            throw new Exception($"This grid does not contain a tilemap of type {type}.");
        return structure;
    }

    private void InitializeTiles()
    {
        tiles = new Dictionary<int, TileBase>
        {
            // Add default void tile
            { 0, null }
        };

        // Add all tilesets
        AddTileSet(tiles, groundTileTypes);
    }

    // Adds a new tileset to the dictionary
    private void AddTileSet (Dictionary<int, TileBase> tiles, TileTypes.TileData[] tileData)
    {
        foreach (var tiletype in tileData)
        {
            if (tiletype.tileTypeId == 0) continue;

            // If using custom tile, otherwise create new tile
            var tile = tiletype.tile == null ?
                CreateTile(tiletype.sprite) :
                tiletype.tile;

            // Check if tile id already exists in the tiles
            if (tiles.ContainsKey(tiletype.tileTypeId))
            {
                var tileTypeInfo = GetTileTypeInfo(tiletype);
                throw new Exception($"Error adding tile from enum [{tileTypeInfo.Item1}]: Tile Id '{tileTypeInfo.Item2}' already exists in another tile enum.");
            }

            tiles.Add(tiletype.tileTypeId, tile);
        }
    }

    // Uses reflection to retrieve the type information from the TileData
    private (string, string) GetTileTypeInfo(TileTypes.TileData data)
    {
        var type = data.GetType();
        var tileTypeField = type.GetField("TileType", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var value = tileTypeField.GetValue(data);
        return (tileTypeField.FieldType.Name, value.ToString());
    }

    private Tile CreateTile (Sprite sprite)
    {
        // Create an instance of type Tile (inherits from TileBase)
        var tile = ScriptableObject.CreateInstance<Tile>();

        // Assign the sprite created to tiles
        tile.sprite = sprite;

        return tile;
    }
}
