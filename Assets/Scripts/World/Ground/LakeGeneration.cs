using System;
using System.Linq;
using UnityEngine;

public class LakeGeneration : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseGenerator noise;

    [Serializable]
    private class NoiseValues
    {
        [Range(0f, 1f)]
        public float height;
        public GroundTileType groundTile;
    }

    [SerializeField]
    private NoiseValues[] tileTypes;

    public void Initialize(TilemapStructure tilemap)
    {
        // Make sure that TileTypes are ordered from small to high height
        tileTypes = tileTypes.OrderBy(a => a.height).ToArray();

        // Pass along parameters to generate noise
        var noiseMap = noise.GenerateNoiseMap(tilemap.width, tilemap.height);

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
                        TilemapStructure groundMap = tilemap.grid.GetTilemap(TilemapType.Ground);
                        var groundNeighbors = groundMap.GetNeighbors(x, y);
                        if (!groundNeighbors.ContainsValue((int)GroundTileType.Empty))
                        {
                            tilemap.SetTile(x, y, (int)tileTypes[i].groundTile, setDirty : false);  
                            break;
                        }
                    }
                }
            }
        }
    }
}
