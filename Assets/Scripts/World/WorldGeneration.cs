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
        TempData.tempRandom = new System.Random(seed);

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);
        islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;

        // Init structures
        GetComponentInChildren<VillageGeneration>().Initialize(GetComponentInChildren<TileGrid>());
        GetComponentInChildren<DungeonGeneration>().Initialize(this, GetComponentInChildren<TileGrid>());
        GetComponentInChildren<CampGeneration>().Initialize(GetComponentInChildren<TileGrid>());

        // Init vegetation
        // GetComponentInChildren<TreeGeneration>().Initialize(GetComponentInChildren<TileGrid>(), width, height);

        // Init time
        // GetComponentInChildren<DayAndNightCycle>().Initialize();

        // Start world events
        // GetComponent<WorldEvents>().Initialize(this, GetComponentInChildren<TileGrid>(), GetComponentInChildren<DayAndNightCycle>());
    }
}
