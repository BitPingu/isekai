using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="NoiseGeneration", menuName="Algorithms/NoiseGeneration")]
public class NoiseGeneration : AlgorithmBase
{
    [Header("Noise settings")]
    // The more octaves, the longer generation will take
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public float noiseScale;
    public Vector2 offset;
    public bool applyIslandGradient;

    [Serializable]
    private class NoiseValues
    {
        [Range(0f, 1f)]
        public float height;
        public GroundTileType groundTile;
    }

    [SerializeField]
    private NoiseValues[] tileTypes;

    public override void Apply(TilemapStructure tilemap)
    {
        // Make sure that TileTypes are ordered from small to high height
        tileTypes = tileTypes.OrderBy(a => a.height).ToArray();

        // Pass along parameters to generate noise
        var noiseMap = Noise.GenerateNoiseMap(tilemap.width, tilemap.height, tilemap.grid.seed, noiseScale, octaves, persistance, lacunarity, offset);

        if (applyIslandGradient)
        {
            var islandGradient = Noise.GenerateIslandGradientMap(tilemap.width, tilemap.height);
            for (int x=0, y; x<tilemap.width; x++)
            {
                for (y=0; y<tilemap.height; y++)
                {
                    // Subtract the islandGradient value from the noiseMap value
                    float subtractedValue = noiseMap[y * tilemap.width + x] - islandGradient[y * tilemap.width + x];

                    // Apply it into the map, but make sure clamped between 0f and 1f
                    noiseMap[y * tilemap.width + x] = Mathf.Clamp01(subtractedValue);
                }
            }
        }

        for (int x=0; x<tilemap.width; x++)
        {
            for (int y=0; y<tilemap.height; y++)
            {
                // Get height at this position
                var height = noiseMap[y * tilemap.width + x];

                // Loop over configured tile types
                for (int i=0; i<tileTypes.Length; i++)
                {
                    // If the height is smaller or equal then use this tiletype
                    if (height <= tileTypes[i].height)
                    {
                        tilemap.SetTile(x, y, (int)tileTypes[i].groundTile, setDirty : false);
                        break;
                    }
                }
            }
        }
    }
}
