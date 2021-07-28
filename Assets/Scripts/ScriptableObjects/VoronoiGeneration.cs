using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "VoronoiGeneration", menuName = "Algorithms/VoronoiGeneration")]

public class VoronoiGeneration : AlgorithmBase
{
    [Serializable]
    private class NoiseValues
    {
        public GroundTileType GroundTile;
    }

    [SerializeField]
    private NoiseValues[] TileTypes;

    private List<Vector3> seeds = new List<Vector3>();
    private List<int> tilePrefabIndex = new List<int>();

    public override void Apply(TilemapStructure tilemap)
    {
        // Create random points
        for (int i = 0; i < 100000; i++)
        {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(0, tilemap.Width), UnityEngine.Random.Range(0, tilemap.Height));
            seeds.Add(randomPosition);

            int RandomTileNumber = UnityEngine.Random.Range(0, 7);
            tilePrefabIndex.Add(RandomTileNumber);
          
            tilemap.SetTile((int)randomPosition.x, (int)randomPosition.y, (int)TileTypes[RandomTileNumber].GroundTile);
        }

        // Generate map
        for (int x=0; x<tilemap.Width; x++)
        {
            for (int y=0; y<tilemap.Height; y++)
            {
                Vector3 point = new Vector3(x, y);

                if (!seeds.Contains(point))
                {
                    int closestPointIndex = FindClosestPoint(point);
                    tilemap.SetTile(x, y, (int)TileTypes[tilePrefabIndex[closestPointIndex]].GroundTile);
                }
            }
        }
    }

    private int FindClosestPoint(Vector3 point)
    {
        int closestPointIndex = 0;
        var distance = Vector3.Distance(point, seeds[0]);

        for (int i=0; i<seeds.Count; i++)
        {
            var tempDistance = Vector3.Distance(point, seeds[i]);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                closestPointIndex = i;
            }
        }
        return closestPointIndex;
    }
}
