using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoissonDiscSampling", menuName = "Algorithms/PoissonDiscSampling")]
public class PoissonDiscSampling : ScriptableObject
{
    public float radius = 1; // radius of each cell
    public Vector2 regionSize = Vector2.one;
    public int samplesBeforeRejection = 30;

    public List<Vector2> GeneratePoints()
    {
        // Calculate cell size
        float cellSize = radius / Mathf.Sqrt(2);

        // Calculate number of cells on x and y axis to make grid
        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.y / cellSize)];

        // Points generated
        List<Vector2> points = new List<Vector2>();
        // Spawn point used for adding future points when new point is added
        List<Vector2> spawnPoints = new List<Vector2>();

        // Add starting point in middle
        spawnPoints.Add(regionSize / 2);

        // While spawn points list is not empty
        while (spawnPoints.Count > 0)
        {
            // Pick random spawn point in list
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];

            bool candidateAccepted = false;

            // Attempt to spawn new point around spawn center point before rejection
            for (int i=0; i<samplesBeforeRejection; i++)
            {
                // Pick random direction
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

                // Create candidate point outside spawn center point using radius
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

                // Check if candidate point is accepted
                if (IsValid(candidate, cellSize, points, grid))
                {
                    // Add candidate point
                    // Debug.Log("new cand:"+candidate);
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    // Set index of new point at candidate point location
                    grid[(int)(candidate.x/cellSize),(int)(candidate.y/cellSize)] = points.Count;

                    candidateAccepted = true;
                    break;
                }
            }

            // If no candidate point was accepted, remove spawn point from list
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        // Return list of generated points
        return points;
    }

    private bool IsValid(Vector2 candidate, float cellSize, List<Vector2> points, int[,] grid)
    {
        // Check if candidate point lies within sample region
        if (candidate.x >= 0 && candidate.x < regionSize.x && candidate.y >= 0 && candidate.y < regionSize.y)
        {
            // Calculate cell location of candidate point
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            // Search 5x5 block around cell
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x=searchStartX; x<=searchEndX; x++)
            {
                for (int y=searchStartY; y<=searchEndY; y++)
                {
                    // Get index of point in cell
                    int pointIndex = grid[x, y] - 1;

                    // No point in cell
                    if (pointIndex != -1)
                    {
                        // Calculate distance between candidate point and point
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        // Check if candidate point too close to point
                        if (sqrDst < radius*radius)
                        {
                            return false;
                        }
                    }
                }
            }

            // Valid point
            return true;
        }

        // Not a valid point
        return false;
    }
}
