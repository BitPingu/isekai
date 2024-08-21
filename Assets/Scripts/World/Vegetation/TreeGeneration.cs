using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public GameObject tree;

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

    private TileGrid grid;
    private int width;
    private System.Random random;
    private float[] noiseMap;


    public void Initialize(TileGrid g, int w, int height)
    {
        // Get grid and width
        grid = g;
        width = w;

        // Get random
        random = TempData.tempRandom;

        // Pass along parameters to generate noise
        noiseMap = noise.GenerateNoiseMap(width, height);
    }

    public void GetTree(int x, int y, GameObject chu)
    {
        // Get height at this position
        var noiseHeight = noiseMap[y * width + x];

        // Spawn tree based on height and chance
        if (noiseHeight <= treeNoiseHeight && random.Next(0, 100) <= spawnChance)
        {
            // Tree sprite offset
            if (x % 2 == 1)
            {
                return;
            }

            Vector2 treePos = new Vector2(x, y);

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
                    var villageNeighbors = grid.GetTilemap(TilemapType.Village).GetNeighbors(x, y);
                    if (grid.CheckLand(new Vector2(x, y)) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePath) && !villageNeighbors.ContainsValue((int)GroundTileType.VillagePlot))
                    {
                        float randX=0, randY=0;
                        TilemapStructure cliffMap = grid.GetTilemap(TilemapType.Cliff);
                        if (!(cliffMap.GetTile(x, y) == (int)GroundTileType.Cliff))
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
                        GameObject ex = Instantiate(extras[i].extraObj, new Vector3(x+randX, y+randY), Quaternion.identity, chu.transform);
                        if (cliffMap.GetTile((int)(x+randX), (int)(y+randY)) == (int)GroundTileType.Cliff)
                        {
                            ex.GetComponent<SpriteRenderer>().sortingOrder+=1;
                        }
                        ex.SetActive(false);
                    }
                    break;
                }
            }
        }
    }

}
