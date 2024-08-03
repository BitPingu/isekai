using System;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundGeneration : Generation
{
    [SerializeField]
    private PerlinNoiseGenerator noise, islandNoise;
    [SerializeField]
    private bool applyIslandGradient;

    [Header("Terrain Threshold values")]
    [SerializeField, Range(0, 1)]
    private float waterThreshold = 0.2f;
    [SerializeField, Range(0, 1)]
    private float hillsThreshold = 0.7f;

    [Header("Island Threshold values")]
    [SerializeField, Range(0, 1), Tooltip("Controls the size of the Island map ONLY if we disable the Additional Island Mask")]
    private float islandMaskThreshold = 0.5f;
    [SerializeField, Range(0, 1), Tooltip("If Additional Mask is On controls the Island sie - higher value = less land")]
    private float islandAdditionalMaskThreshold = 0.09f;
    [SerializeField, Range(0, 1), Tooltip("The higher the value the more 'long edges' / less circular will the map be")]
    private float islandSmoothStepFrom = 0f;
    [SerializeField, Range(0, 1), Tooltip("The lower the value the BIGGER and more Circular the island will be")]
    private float islandSmoothStepTo = 0.518f;

    // [Serializable]
    // private class NoiseValues
    // {
    //     [Range(0f, 1f)]
    //     public float height;
    //     public GroundTileType groundTile;
    // }

    // [SerializeField]
    // private NoiseValues[] tileTypes;

    public override void Initialize(WorldGeneration world)
    {
        Debug.Log("start ground gen");
        BaseIslandGeneration(world);
    }

    private void BaseIslandGeneration(WorldGeneration world)
    {
        // // Make sure that TileTypes are ordered from small to high height
        // tileTypes = tileTypes.OrderBy(a => a.height).ToArray();

        // Pass along parameters to generate noise
        float[,] baseNoiseMap = noise.GenerateNoiseMap(world.width, world.height, world.seed);
        float[,] maskNoiseMap = null;
        if (applyIslandGradient)
            maskNoiseMap = BaseIslandMask(world);

        for (int x=0; x<world.width; x++)
        {
            for (int y=0; y<world.height; y++)
            {
                float noiseValue = 0f;

                // Apply island mask
                if (maskNoiseMap != null && x>=0 && x<maskNoiseMap.GetLength(0) && y>=0 && y<maskNoiseMap.GetLength(1))
                {
                    noiseValue = maskNoiseMap[x,y];
                }

                // Apply base terrain
                float baseNoiseValue = baseNoiseMap[x,y];

                if (applyIslandGradient)
                    noiseValue *= baseNoiseValue;
                else
                    noiseValue = baseNoiseValue;

                // Clamp noise value after applying all masks
                noiseValue = Mathf.Clamp(noiseValue, 0, hillsThreshold);

                if (noiseValue < waterThreshold)
                {
                    // Set tile to be placed later
                    world.groundTiles[x,y] = TileType.Water;
                }
                else
                {
                    // Debug.Log("noise: " + noiseValue);
                    // Debug.Log("grass at " + x + " " + y);
                    world.groundTiles[x,y] = TileType.Ground;
                }

                // // Loop over ground tile types
                // for (int i=0; i<tileTypes.Length; i++)
                // {
                //     // If the height is smaller or equal then use this tiletype
                //     if (noiseHeight <= tileTypes[i].height)
                //     {
                //         // tilemap.SetTile(x, y, (int)tileTypes[i].groundTile, setDirty : false);                        
                //         break;
                //     }
                // }
            }
        }

        // Deep sea
        foreach (Vector2Int seaTile in GetAllSeaTileConnectedTo(new Vector2Int(0, 0), world.groundTiles))
            world.groundTiles[seaTile.x, seaTile.y] = TileType.DeepSea;
    }

    private float[,] BaseIslandMask(WorldGeneration world)
    {
        // Generate island masks
        float[,] islandMask = islandNoise.GenerateCircularMask(world.width, world.height);
        float[,] islandAdditionalMask = islandNoise.GenerateNoiseMap(world.width, world.height, world.seed);

        float[,] baseNoiseMap = new float[world.width, world.height];

        for (int x=0; x<world.width; x++)
        {
            for (int y=0; y<world.height; y++)
            {
                // Apply details to circular mask
                baseNoiseMap[x,y] = islandAdditionalMask[x,y];
                if (baseNoiseMap[x,y] > islandAdditionalMaskThreshold)
                    baseNoiseMap[x,y] = 1;
                else
                    baseNoiseMap[x,y] = 0;

                // Smooth edges
                baseNoiseMap[x,y] = Mathf.SmoothStep(islandSmoothStepFrom, islandSmoothStepTo, islandMask[x,y]);

                // Calculate edges
                baseNoiseMap[x,y] = islandAdditionalMask[x,y] - baseNoiseMap[x,y];

                // Set edges
                if (baseNoiseMap[x,y] > islandAdditionalMaskThreshold)
                    baseNoiseMap[x,y] = 1; // edge (water)
                else
                    baseNoiseMap[x,y] = 0; // middle (island)

                // Apply additional mask
                float edgeMaskValue = 1 - islandMask[x,y];
                edgeMaskValue = edgeMaskValue > islandAdditionalMaskThreshold ? 1 : 0;
                baseNoiseMap[x,y] *= edgeMaskValue;

                if (baseNoiseMap[x,y] < waterThreshold)
                {
                    // Set tile to be placed later
                    // Debug.Log("water");
                    world.groundTiles[x,y] = TileType.Water;
                }
                else
                {
                    // Debug.Log("ground");
                    world.groundTiles[x,y] = TileType.Ground;
                }
            }
        }

        return baseNoiseMap;
    }

    private Vector2Int[] GetAllSeaTileConnectedTo(Vector2Int startPosition, TileType[,] map)
    {
        List<Vector2Int> connectedTiles = new List<Vector2Int>();
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);

        bool[,] visited = new bool[mapWidth, mapHeight];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPosition);
        visited[(int)startPosition.x, (int)startPosition.y] = true;

        // Check adjacent tiles
        Vector2Int[] adjacentTiles = new Vector2Int[]
        {
                new Vector2Int( 1, 0),
                new Vector2Int(- 1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, - 1)
        };

        while (queue.Count > 0)
        {
            Vector2Int currentTile = queue.Dequeue();
            connectedTiles.Add(currentTile);

            foreach (Vector2Int offset in adjacentTiles)
            {
                Vector2Int adjacentTile = currentTile + offset;
                int x = adjacentTile.x;
                int y = adjacentTile.y;

                // Check if the adjacent tile is within the map boundaries and is of type Water
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight && map[x, y] == TileType.Water && !visited[x, y])
                {
                    queue.Enqueue(adjacentTile);
                    visited[x, y] = true;
                }
            }
        }

        return connectedTiles.ToArray();
    }
}
