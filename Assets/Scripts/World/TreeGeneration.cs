using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseGenerator noise;
    public float treeNoiseHeight; // default is 0.4f
    public float spawnChance; // default is 65f
    public TilemapStructure groundMap;

    public GameObject tree, grass;

    private void Awake()
    {
        
    }

    public void Initialize(int width, int height, int seed)
    {
        var random = new System.Random(seed);

        // Pass along parameters to generate noise
        var noiseMap = noise.GenerateNoiseMap(width, height, seed);

        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                // Get height at this position
                var noiseHeight = noiseMap[y * width + x];

                // Spawn tree based on height and chance
                if (noiseHeight <= treeNoiseHeight && random.Next(0, 100) <= spawnChance)
                {
                    // Tree sprite offset
                    if (x % 2 == 1)
                        continue;

                    Vector2 treePos = new Vector2(x, y);
                    int randPos = random.Next(0, 100);

                    // Random offset placement
                    if (randPos < 20)
                    {
                        treePos.x += .7f;
                        treePos.y += .5f;
                    }
                    else if (randPos < 40)
                    {
                        treePos.x -= .7f;
                        treePos.y += .5f;
                    }

                    // Check if tree is spawning on land
                    var neighbors = groundMap.GetNeighbors(Mathf.FloorToInt(treePos.x), Mathf.FloorToInt(treePos.y));
                    if (!neighbors.ContainsValue((int)GroundTileType.Water) 
                        && !neighbors.ContainsValue((int)GroundTileType.VillagePath) && !neighbors.ContainsValue((int)GroundTileType.VillagePlot))
                    {
                        // Additional chance to replace tree to spawn a different foliage? ie bush
                        // Spawn tree
                        Instantiate(tree, new Vector3(treePos.x, treePos.y), Quaternion.identity, transform);
                    }
                }
                else if (noiseHeight <= .52 && random.Next(0, 100) <= spawnChance)
                {
                    var neighbors = groundMap.GetNeighbors(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
                    if (!neighbors.ContainsValue((int)GroundTileType.Water) 
                        && !neighbors.ContainsValue((int)GroundTileType.VillagePath) && !neighbors.ContainsValue((int)GroundTileType.VillagePlot))
                    {
                        // Grass
                        float randX = (float)random.NextDouble();
                        float randY = (float)random.NextDouble();
                        Instantiate(grass, new Vector3(x+randX, y+randY), Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
