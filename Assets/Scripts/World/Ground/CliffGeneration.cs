using System;
using System.Linq;
using UnityEngine;

public class CliffGeneration : Generation
{
    [SerializeField]
    private PerlinNoiseGenerator noise;

    // [Serializable]
    // private class NoiseValues
    // {
    //     [Range(0f, 1f)]
    //     public float height;
    //     public GroundTileType groundTile;
    // }

    // [SerializeField]
    // private NoiseValues[] tileTypes;

    [SerializeField, Range(0, 1)]
    private float hillsCircularMaskModifier = 0.48f;

    [SerializeField, Range(0, 1)] 
    private float hillsLevel1AmountModifier = 0.732f;
    [SerializeField, Range(0, 1)] 
    private float hillsLevel2AmountModifier = 0.56f;

    public override void Initialize(WorldGeneration world)
    {
        Debug.Log("start cliff gen");
        HillGeneration(world);
    }

    private void HillGeneration(WorldGeneration world)
    {
        // Make sure that TileTypes are ordered from small to high height
        // tileTypes = tileTypes.OrderBy(a => a.height).ToArray();

        // Pass along parameters to generate noise
        float[,] baseNoiseMap = noise.GenerateNoiseMap(world.width, world.height, world.seed);
        float[,] circularMask = noise.GenerateCircularMask(world.width, world.height, hillsCircularMaskModifier);
        for (int i=0; i<world.width; i++)
        {
            for (int j=0; j<world.height; j++)
            {
                Debug.Log(circularMask[i,j] + " at " + i + " " + j);
            }
        }

        for (int x=0; x<world.width; x++)
        {
            for (int y=0; y<world.height; y++)
            {
                // Invert mask
                float baseNoiseValue = baseNoiseMap[x,y] * (1 - circularMask[x,y]);

                if (baseNoiseValue > (1-hillsLevel1AmountModifier))
                {
                    world.groundTiles[x,y] = TileType.Cliff;
                }
                if (baseNoiseValue > (1-hillsLevel2AmountModifier))
                {
                    world.groundTiles[x,y] = TileType.Cliff;
                }
            }
        }
    }
}
