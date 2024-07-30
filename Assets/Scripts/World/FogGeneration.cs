using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "FogGeneration", menuName = "Algorithms/FogGeneration")]
public class FogGeneration
{
    public List<int> clearFogCoordsX = new List<int>();
    public List<int> clearFogCoordsY = new List<int>();
    public void Apply(TilemapStructure tilemap)
    {
        // Generate new fog
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                tilemap.SetTile(x, y, (int)GroundTileType.Fog, setDirty: false);
            }
        }

        if (TempData.initFog)
        {
            if (!TempData.newGame)
            {
                // Load fog data
                SaveData saveData = SaveSystem.Load();
                clearFogCoordsX = saveData.saveClearFogCoordsX;
                clearFogCoordsY = saveData.saveClearFogCoordsY;

                // Combine lists
                HashSet<Vector2Int> clearFogCoords = new HashSet<Vector2Int>();
                var combinedCoords = clearFogCoordsX.Zip(clearFogCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                foreach (var coord in combinedCoords)
                {
                    clearFogCoords.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                }

                // Load no fog tiles
                foreach (var coord in clearFogCoords)
                {
                    tilemap.SetTile(coord.x, coord.y, (int)GroundTileType.Empty, setDirty: false);
                }
            }
        }
        else
        {
            // Load fog data
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                List<int> clearFogCoordsX = TempData.tempFog.clearFogCoordsX;
                List<int> clearFogCoordsY = TempData.tempFog.clearFogCoordsY;

                // Combine lists
                HashSet<Vector2Int> clearFogCoords = new HashSet<Vector2Int>();
                var combinedCoords = clearFogCoordsX.Zip(clearFogCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                foreach (var coord in combinedCoords)
                {
                    clearFogCoords.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                }

                // Load no fog tiles
                foreach (var coord in clearFogCoords)
                {
                    tilemap.SetTile(coord.x, coord.y, (int)GroundTileType.Empty, setDirty: false);
                }
            }
            // else
            // {
            //     List<int> clearFogCoordsX = TempData.tempFog2.clearFogCoordsX;
            //     List<int> clearFogCoordsY = TempData.tempFog2.clearFogCoordsY;
            // }
        }
    }
}
