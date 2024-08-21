using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FogGeneration : MonoBehaviour
{
    public void Initialize(TilemapStructure tilemap)
    {
        // Generate new fog
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                tilemap.SetTile(x, y, (int)GroundTileType.Fog, setDirty: false);
            }
        }

        if (TempData.loadGame && GetComponent<TilemapStructure>().type == TilemapType.Fog)
        {
            // Load fog data
            List<int> clearFogCoordsX = SaveSystem.Load().saveClearFogCoordsX;
            List<int> clearFogCoordsY = SaveSystem.Load().saveClearFogCoordsY;

            // Combine lists
            var combinedCoords = clearFogCoordsX.Zip(clearFogCoordsY, (x, y) => new { xCoord = x, yCoord = y });
            foreach (var coord in combinedCoords)
            {
                GetComponent<FogData>().clearFogCoords.Add(new Vector2(coord.xCoord, coord.yCoord));
                tilemap.SetTile(coord.xCoord, coord.yCoord, (int)GroundTileType.Empty, setDirty: false);
            }
        }
        else if (TempData.loadGame && GetComponent<TilemapStructure>().type == TilemapType.FogUnderground)
        {
            // Load underground fog data
            List<int> clearFogCoords2X = SaveSystem.Load().saveClearFogCoords2X;
            List<int> clearFogCoords2Y = SaveSystem.Load().saveClearFogCoords2Y;

            // Combine lists
            var combinedCoords = clearFogCoords2X.Zip(clearFogCoords2Y, (x, y) => new { xCoord = x, yCoord = y });
            foreach (var coord in combinedCoords)
            {
                GetComponent<FogData>().clearFogCoords.Add(new Vector2(coord.xCoord, coord.yCoord));
                tilemap.SetTile(coord.xCoord, coord.yCoord, (int)GroundTileType.Empty, setDirty: false);
            }
        }

        // Init fog
        GetComponent<FogData>().Initialize(tilemap);
    }
}
