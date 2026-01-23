using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/PerlinNoiseGenerator")]
public class PerlinNoiseGenerator: ScriptableObject
{
    [Header("Noise settings")]
    // The more octaves, the longer generation will take
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public float noiseScale;
    public Vector2 offset;

    // public int scale, octaves, persistance, lacunarity;
    public float[] GenerateNoiseMap(int mapWidth, int mapHeight)
    {
        float[] noiseMap = new float[mapWidth * mapHeight];

        // Need at least one octave
        if (octaves < 1)
        {
            octaves = 1;
        }

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i=0; i<octaves; i++)
        {
            float offsetX, offsetY;
            if (TempData.tempNoiseOffsetX == 0 && TempData.tempNoiseOffsetY == 0)
            {
                var random = TempData.tempRandom;
                offsetX = random.Next(-100000, 100000) + offset.x;
                offsetY = random.Next(-100000, 100000) + offset.y;
                TempData.tempNoiseOffsetX = offsetX;
                TempData.tempNoiseOffsetY = offsetY;
            }
            else
            {
                offsetX = TempData.tempNoiseOffsetX;
                offsetY = TempData.tempNoiseOffsetY;
            }
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0f)
        {
            noiseScale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Zoom from center instead of top-right corner
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int x=0, y; x<mapWidth; x++)
        {
            for (y=0; y<mapHeight; y++)
            {
                // Define base values for amplitude, frequency, and noiseHeight
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Calculate noise for each octave
                for (int i=0; i<octaves; i++)
                {
                    // Sample a point (x,y)
                    float sampleX = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;

                    // Use unity's implementation of perlin noise
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    // noiseHeight is final noise, add all octaves together
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // Find the min and max noise height in noisemap
                // to interpolate the min and max values between 0 and 1
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                // Assign noise
                noiseMap[y * mapWidth + x] = noiseHeight;
            }
        }

        for (int x = 0, y; x < mapWidth; x++)
        {
            for (y = 0; y < mapHeight; y++)
            {
                // Returns a value between 0f and 1f based on noiseMap value
                // minNoiseHeight being 0f, and maxNoiseHeight being 1f
                noiseMap[y * mapWidth + x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y * mapWidth + x]);
            }
        }

        return noiseMap;
    }

    public float[] GenerateIslandGradientMap(int mapWidth, int mapHeight)
    {
        float[] map = new float[mapWidth * mapHeight];
        for (int x=0; x<mapWidth; x++)
        {
            for (int y=0; y<mapHeight; y++)
            {
                // Value between 0 and 1 where * 2 - 1 makes it between -1 and 0
                float i = x / (float)mapWidth * 2 - 1;
                float j = y / (float)mapHeight * 2 - 1;

                // Find closest x or y to the edge of the map
                float value = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j));

                // Apply a curve graph to have more values around 0 on the edge
                // and more values >= 3 in the middle
                float a = 3;
                float b = 2.2f;
                float islandGradientValue = Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));

                // Apply gradient in the map
                map[y * mapWidth + x] = islandGradientValue;
            }
        }
        return map;
    }
}
