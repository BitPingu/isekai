using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoblinAlgorithm", menuName = "Algorithms/GoblinAlgorithm")]
public class GoblinAlgorithm : ScriptableObject
{
    private TilemapStructure groundMap;

    public List<Vector2> GeneratePoints()
    {
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
        // Points generated
        List<Vector2> points = new List<Vector2>();

        // Look for camp points
        List<Vector3> campPoints = TempData.tempCamps;

        foreach (Vector3 point in campPoints)
        {
            // Check if safe to spawn
            if (groundMap.GetTile((int)point.x+1, (int)point.y) == (int)GroundTileType.Land)
            {
                points.Add(new Vector2((int)point.x+1.5f, (int)point.y+.55f));
            }
            if (groundMap.GetTile((int)point.x-1, (int)point.y) == (int)GroundTileType.Land)
            {
                points.Add(new Vector2((int)point.x-.5f, (int)point.y+.55f));
            }
        }

        // Return list of generated points
        return points;
    }
}
