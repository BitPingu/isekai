using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FogGeneration", menuName = "Algorithms/FogGeneration")]
public class FogGeneration : AlgorithmBase
{
    public override void Apply(TilemapStructure tilemap)
    {
        // Generate new fog
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                tilemap.SetTile(x, y, (int)GroundTileType.Fog, setDirty: false);
            }
        }

        if (MainMenu.loadGame)
        {
            // Load fog data
            WorldData data = SaveSystem.LoadWorld();
            List<int> clearFogCoordsX = data.clearFogCoordsX;
            List<int> clearFogCoordsY = data.clearFogCoordsY;

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
}
