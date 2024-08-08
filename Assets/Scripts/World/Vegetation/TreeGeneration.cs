using System;
using System.Collections.Generic;
using UnityEngine;

public enum TreeExtras
{
    /* 1001 - 2000 */
    Empty = 1001,
    Bush = 1002,
    Grass = 1003,
    Flower = 1004,
    Rock = 1005,
}


public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseGenerator noise;
    public float treeNoiseHeight; // default is .47f
    public float spawnChance; // default is 80f
    public GameObject tree, chunk;
    private GameObject chu;

    // Extras (move these somewhere else later?)
    [Serializable]
    private class NoiseValues
    {
        [Range(0f, 1f)]
        public float height;
        public float spawnChance;
        public TreeExtras extra;
        public GameObject extraObj;
    }
    [SerializeField]
    private NoiseValues[] extras;

    public void Initialize(TileGrid grid, int width, int height)
    {
        var random = TempData.tempRandom;

        // Pass along parameters to generate noise
        var noiseMap = noise.GenerateNoiseMap(width, height);

        Vector2Int currentPoint = new Vector2Int();
        int chunkSize = 16, pivotPoint = 0;
        while (currentPoint.x <= width-1 || currentPoint.y <= height-1)
        {
            if (currentPoint.x >= width-1 && currentPoint.y >= height-1)
            {
                break;
            } 
            else if (currentPoint.x >= width-1)
            {
                pivotPoint+=chunkSize;
                currentPoint.x = 0;
            }
            currentPoint.y = pivotPoint;

            // Divide into chunks
            chu = Instantiate(chunk, new Vector3(currentPoint.x, currentPoint.y), Quaternion.identity, transform);
            chu.GetComponent<BoxCollider2D>().size = new Vector2(chunkSize*3, chunkSize*3);
            chu.GetComponent<BoxCollider2D>().offset = new Vector2(chunkSize/2, chunkSize/2);

            for (int x=0; x<chunkSize; x++)
            {
                if (currentPoint.x > width-1)
                {
                    break;
                }
                currentPoint.y=pivotPoint;
                for (int y=0; y<chunkSize; y++)
                {
                    if (currentPoint.y > height-1)
                    {
                        break;
                    }
                    // Get height at this position
                    var noiseHeight = noiseMap[currentPoint.y * width + currentPoint.x];

                    // Spawn tree based on height and chance
                    if (noiseHeight <= treeNoiseHeight && random.Next(0, 100) <= spawnChance)
                    {
                        // Tree sprite offset
                        if (x % 2 == 1)
                        {
                            currentPoint.y++;
                            continue;
                        }

                        Vector2 treePos = new Vector2(currentPoint.x, currentPoint.y);

                        int randPos = random.Next(0, 100);

                        TilemapStructure cliffMap = grid.GetTilemap(TilemapType.Cliff);
                        if (!(cliffMap.GetTile((int)treePos.x, (int)treePos.y) == (int)GroundTileType.Cliff))
                        {
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
                        }
                        else
                        {
                            treePos.x += .5f;
                            treePos.y += 1.5f;
                        }

                        // Check if tree is spawning on land
                        var villageNeighbors = grid.GetTilemap(TilemapType.Village).GetNeighbors((int)treePos.x, (int)treePos.y);
                        if (grid.CheckLand(treePos) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePath) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePlot))
                        {
                            // Spawn tree
                            GameObject tr = Instantiate(tree, new Vector3(treePos.x, treePos.y), Quaternion.identity, chu.transform);
                            if (cliffMap.GetTile((int)treePos.x, (int)treePos.y) == (int)GroundTileType.Cliff)
                            {
                                tr.GetComponent<SpriteRenderer>().sortingOrder+=1;
                            }
                            tr.SetActive(false);
                        }
                    }
                    else
                    {
                        // Loop over configured tile types
                        for (int i=0; i<extras.Length; i++)
                        {
                            if (noiseHeight <= extras[i].height && random.Next(0, 100) <= extras[i].spawnChance)
                            {
                                var villageNeighbors = grid.GetTilemap(TilemapType.Village).GetNeighbors(currentPoint.x, currentPoint.y);
                                if (grid.CheckLand(new Vector2(currentPoint.x, currentPoint.y)) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePath) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePlot))
                                {
                                    float randX=0, randY=0;
                                    TilemapStructure cliffMap = grid.GetTilemap(TilemapType.Cliff);
                                    if (!(cliffMap.GetTile(currentPoint.x, currentPoint.y) == (int)GroundTileType.Cliff))
                                    {
                                        randX = (float)random.NextDouble();
                                        randY = (float)random.NextDouble();
                                    }
                                    else
                                    {
                                        randX = .5f;
                                        randY = 1.5f;
                                    }
                                    // Spawn extra
                                    GameObject ex = Instantiate(extras[i].extraObj, new Vector3(currentPoint.x+randX, currentPoint.y+randY), Quaternion.identity, chu.transform);
                                    if (cliffMap.GetTile((int)(currentPoint.x+randX), (int)(currentPoint.y+randY)) == (int)GroundTileType.Cliff)
                                    {
                                        ex.GetComponent<SpriteRenderer>().sortingOrder+=1;
                                    }
                                    ex.SetActive(false);
                                }
                                break;
                            }
                        }
                    }
                    currentPoint.y++;
                }
                currentPoint.x++;
            }
        }
    }
}
