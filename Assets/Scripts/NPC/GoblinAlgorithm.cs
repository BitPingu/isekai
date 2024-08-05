using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoblinAlgorithm", menuName = "Algorithms/GoblinAlgorithm")]
public class GoblinAlgorithm : ScriptableObject
{
    public List<Vector2> GeneratePoints()
    {
        // Points generated
        List<Vector2> points = new List<Vector2>();

        // Look for camp points
        List<Vector2> campPoints = TempData.tempCamps;

        foreach (Vector2 point in campPoints)
        {
            // Check if safe to spawn (randomize later on?)
            points.Add(new Vector2((int)point.x+1.5f, (int)point.y+.7f));
            points.Add(new Vector2((int)point.x+.5f, (int)point.y+1.2f));
        }

        // Return list of generated points
        return points;
    }
}
