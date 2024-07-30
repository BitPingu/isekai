using System;
using System.Linq;
using UnityEngine;

public class GroundGeneration : TilemapAlgorithmBase
{
    [SerializeField]
    private PerlinNoiseGenerator noise;
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

    public Vector3 minIslandCoords;
    public Vector3 maxIslandCoords;

    public override void Apply(TilemapStructure tilemap)
    {
        // Make sure that TileTypes are ordered from small to high height
        tileTypes = tileTypes.OrderBy(a => a.height).ToArray();

        // Pass along parameters to generate noise
        var noiseMap = noise.GenerateNoiseMap(tilemap.width, tilemap.height, tilemap.seed);

        if (applyIslandGradient)
        {
            var islandGradient = noise.GenerateIslandGradientMap(tilemap.width, tilemap.height);
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

        minIslandCoords = new Vector3(tilemap.width, tilemap.height);
        maxIslandCoords = Vector3.zero;

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

                        if ((int)tileTypes[i].groundTile == (int)GroundTileType.Land)
                        {
                            if (x < minIslandCoords.x)
                            {
                                minIslandCoords.x = x;
                            }
                            if (y < minIslandCoords.y)
                            {
                                minIslandCoords.y = y;
                            }
                            if (x > maxIslandCoords.x)
                            {
                                maxIslandCoords.x = x;
                            }
                            if (y > maxIslandCoords.y)
                            {
                                maxIslandCoords.y = y;
                            }
                        }
                        
                        break;
                    }
                }
            }
        }
    }
}
