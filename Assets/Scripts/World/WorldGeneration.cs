using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int width, height, seed;
    public Vector2 islandRegionSize;

    private void Awake()
    {
        // Generate world seed
        seed = UnityEngine.Random.Range(-100000, 100000);
        TempData.tempSeed = seed;

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);

        islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;

        // Init vegetation
        GetComponentInChildren<TreeGeneration>().Initialize(width, height, seed);

        // Init time
        GetComponentInChildren<DayAndNightCycle>().Initialize();

        // Start world events
        GetComponent<WorldEvents>().Initialize(GetComponentInChildren<TilemapStructure>(), GetComponentInChildren<DayAndNightCycle>());
    }
}
