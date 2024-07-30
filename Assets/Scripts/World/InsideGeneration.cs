using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InsideGeneration", menuName = "Algorithms/InsideGeneration")]
public class InsideGeneration
{
    public void Apply(TilemapStructure tilemap)
    {
        // Generate inside
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                tilemap.SetTile(x, y, (int)GroundTileType.Land, setDirty: false);
            }
        }
    }
}
