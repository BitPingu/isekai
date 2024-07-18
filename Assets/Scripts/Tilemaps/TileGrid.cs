using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid : MonoBehaviour
{
    public int width, height, seed;

    [SerializeField]
    private int tileSize;
    [SerializeField]
    private bool randomize;

    [SerializeField]
    private TileTypes.GroundTiles[] groundTileTypes;
    [SerializeField]
    private TileTypes.FoilageTiles[] foilageTileTypes;
    [SerializeField]
    private TileTypes.BuildingTiles[] buildingTileTypes;

    private Dictionary<int, TileBase> tiles;
    private Dictionary<TilemapType, TilemapStructure> tilemaps;

    private void Awake ()
    {
        if (randomize)
        {
            if (TempData.initSeed)
            {
                if (TempData.newGame)
                {
                    // Generate init seed
                    seed = UnityEngine.Random.Range(-100000, 100000);
                }
                else
                {
                    // Load world data
                    SaveData saveData = SaveSystem.Load();
                    seed = saveData.saveSeed;
                }
                TempData.tempSeed = seed;
                TempData.initSeed = false;
            }
            else
            {
                seed = TempData.tempSeed;
            }
        }

        InitializeTiles();

        tilemaps = new Dictionary<TilemapType, TilemapStructure>();

        // Add all tilemaps by name to collection for easy access
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
            tilemap.Initialize();
        }
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
        AddTileSet(tiles, foilageTileTypes);
        AddTileSet(tiles, buildingTileTypes);
    }

    // Adds a new tileset to the dictionary
    private void AddTileSet (Dictionary<int, TileBase> tiles, TileTypes.TileData[] tileData)
    {
        foreach (var tiletype in tileData)
        {
            if (tiletype.tileTypeId == 0) continue;

            // If using custom tile, otherwise create new tile
            var tile = tiletype.tile == null ?
                CreateTile(tiletype.color, tiletype.sprite) :
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

    private Tile CreateTile (Color color, Sprite sprite)
    {
        // Create an instance of type Tile (inherits from TileBase)
        var tile = ScriptableObject.CreateInstance<Tile>();

        // If no sprite specified, create one for color instead
        if (sprite == null)
        {
            // Created sprites do not support custom physics shape
            var texture = new Texture2D(tileSize, tileSize)
            {
                filterMode = FilterMode.Point
            };

            // Create new sprite without any custom physics shape
            sprite = Sprite.Create(texture, new Rect(0, 0, tileSize, tileSize), new Vector2(0.5f, 0.5f), tileSize);

            // Make sure color is not transparent
            color.a = 1;

            // Set the tile color
            tile.color = color;
        }

        // Assign the sprite created to tiles
        tile.sprite = sprite;

        return tile;
    }
}
