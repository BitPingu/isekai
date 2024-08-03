using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour
{
    public TileBase water, sea, ground, deepSea, cliff;

    public void Initialize(WorldGeneration world)
    {
        // Clear all tiles

        // Place tiles
        PlaceTiles(world);
    }

    private void PlaceTiles(WorldGeneration world)
    {
        Debug.Log("start placing");
        for (int x=0; x<world.width; x++)
        {
            for (int y=0; y<world.height; y++)
            {
                // Place ground tile
                GetComponentInChildren<Tilemap>().SetTile(new Vector3Int(x,y,0), GetTileFrom(world.groundTiles[x,y]));
            }
        }
    }

    private TileBase GetTileFrom(TileType type)
    {
        return type switch
        {
            TileType.Water => water,
            TileType.Sea => sea,
            TileType.Ground => ground,
            TileType.DeepSea => deepSea,
            TileType.Cliff => cliff,
            _ => throw new NotImplementedException()
        };
    }
}
