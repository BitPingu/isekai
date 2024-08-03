using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/PerlinNoiseGenerator")]
public class PerlinNoiseGenerator : ScriptableObject
{
    [Header("Noise settings")]
    // The more octaves, the longer generation will take
    public float scale;
    public int octaves = 1;
    public float persistence;
    public float lacunarity;
    public Vector2 offset = Vector2.zero;

    public float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0f)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = 0, amplitude = 1;

        for (int i=0; i<octaves; i++)
        {
            maxNoiseHeight += amplitude;
            amplitude *= persistence;
        }

        for (int x=0; x<mapWidth; x++)
        {
            for (int y=0; y<mapHeight; y++)
            {
                // Define base values for amplitude, frequency, and noiseHeight
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Calculate noise for each octave
                for (int i=0; i<octaves; i++)
                {
                    // Sample a point (x,y)
                    var random = new System.Random(seed);
                    float sampleX = (x + offset.x + random.Next(-100000,100000)) / scale * frequency;
                    float sampleY = (y + offset.y + random.Next(-100000,100000)) / scale * frequency;

                    // Use unity's implementation of perlin noise
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                    // noiseHeight is final noise, add all octaves together
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                // Normalize noiseHeight to range [0, 1]
                noiseMap[x,y] = noiseHeight / maxNoiseHeight;
            }
        }

        return noiseMap;
    }

    public float[,] GenerateCircularMask(int width, int height, float circularRadiusModifier01=1)
    {
        float[,] mask = new float[width, height];
        float radius = Mathf.Min(width, height) / 2;

        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                float distance = Vector2.Distance(new Vector2(x,y), new Vector2(width/2f, height/2f)) / circularRadiusModifier01;
                mask[x, y] = Mathf.Clamp01(distance/radius);
            }
        }

        return mask;
    }
}
