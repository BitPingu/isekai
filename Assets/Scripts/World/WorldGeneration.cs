using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generation : MonoBehaviour
{
    public abstract void Initialize(WorldGeneration world);
}

public enum TileType
{
    None,
    Water,
    Sea,
    DeepSea,
    Ground,
    Cliff
}

public class WorldGeneration : MonoBehaviour
{
    public int width, height, seed = 123;
    public bool randomize;
    public TileType[,] groundTiles;
    
    private void Awake()
    {
        // Game init starts here
        if (TempData.loadGame)
        {
            // Load seed
            seed = SaveSystem.Load().saveSeed;
        }
        else
        {
            // Generate seed
            if (randomize)
                seed = GenerateSeed();
        }
        TempData.tempSeed = seed;


        groundTiles = new TileType[width, height];

        // Run generation steps
        Generation[] steps = GetComponentsInChildren<Generation>();
        foreach (Generation gen in steps)
        {
            if (gen != null)
                gen.Initialize(this);
        }

        // Place tiles
        GetComponentInChildren<TileGeneration>().Initialize(this);

        // Init tile grid
        // GetComponentInChildren<TileGrid>().Initialize(width, height, seed);
        // islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;

        // Init structures
        // GetComponentInChildren<VillageGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());
        // GetComponentInChildren<DungeonGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());
        // GetComponentInChildren<CampGeneration>().Initialize(GetComponentInChildren<TilemapStructure>());

        // Init vegetation
        // GetComponentInChildren<TreeGeneration>().Initialize(width, height, seed);

        // Init time
        // GetComponentInChildren<DayAndNightCycle>().Initialize();

        // Start world events
        // GetComponent<WorldEvents>().Initialize(GetComponentInChildren<TilemapStructure>(), GetComponentInChildren<DayAndNightCycle>());
    }

    private int GenerateSeed()
    {
        // Generate world seed
        return UnityEngine.Random.Range(-100000, 100000);
    }
}
