using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int width, height, seed;
    public Vector2 islandRegionSize;

    private void Awake()
    {
        if (TempData.loadGame)
        {
            seed = SaveSystem.Load().saveSeed;
        }
        else
        {
            // Generate world seed
            seed = UnityEngine.Random.Range(-100000, 100000);
        }
        TempData.tempSeed = seed;

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);
        islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;

        // Init structures
        GetComponentInChildren<VillageGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());
        GetComponentInChildren<DungeonGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());
        GetComponentInChildren<CampGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());

        // Init vegetation
        GetComponentInChildren<TreeGeneration>().Initialize(width, height, seed);

        // Init time
        GetComponentInChildren<DayAndNightCycle>().Initialize();

        // Start world events
        GetComponent<WorldEvents>().Initialize(GetComponentInChildren<TilemapStructure>(), GetComponentInChildren<DayAndNightCycle>());
    }
}
