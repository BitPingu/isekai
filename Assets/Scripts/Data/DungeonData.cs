using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : MonoBehaviour
{
    private TilemapStructure tilemap;
    private HashSet<Vector2Int> dungeonCoords;
    public List<int> dungeonCoordsX, dungeonCoordsY;

    private void Awake()
    {
        // Retrieve tilemap component
        tilemap = GetComponent<TilemapStructure>();
    }

    // Retrieves coordinates with dungeon entrances
    public void GetDungeon()
    {
        dungeonCoords = new HashSet<Vector2Int>();

        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                // Only add unique coords (using hashset)
                if (tilemap.GetTile(x, y) == (int)ObjectTileType.Dungeon)
                    dungeonCoords.Add(new Vector2Int(x, y));
            }
        }

        // Separate coords into lists for serialization
        dungeonCoordsX = new List<int>();
        dungeonCoordsY = new List<int>();

        foreach (var coord in dungeonCoords)
        {
            dungeonCoordsX.Add(coord.x);
            dungeonCoordsY.Add(coord.y);
        }
    }
}
