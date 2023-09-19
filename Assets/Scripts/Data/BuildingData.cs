using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public List<int> villageCoordsX, villageCoordsY, dungeonCoordsX, dungeonCoordsY;

    private TilemapStructure tilemap;

    private void Awake()
    {
        // Retrieve tilemap component
        tilemap = GetComponent<TilemapStructure>();

        GetVillage();
        GetDungeon();
    }

    // Retrieves coordinates with village entrances
    public void GetVillage()
    {
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                // Only add unique coords (using hashset)
                if (tilemap.GetTile(x, y) == (int)BuildingTileType.House)
                {
                    villageCoordsX.Add(x);
                    villageCoordsY.Add(y);
                }
            }
        }
    }

    // Retrieves coordinates with dungeon entrances
    public void GetDungeon()
    {
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                // Only add unique coords (using hashset)
                if (tilemap.GetTile(x, y) == (int)BuildingTileType.Dungeon)
                {
                    dungeonCoordsX.Add(x);
                    dungeonCoordsY.Add(y);
                }
            }
        }
    }
}
