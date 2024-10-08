using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            seed = Random.Range(-100000, 100000);
        }
        TempData.tempSeed = seed;
        TempData.tempRandom = new System.Random(seed);

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);
        islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;

        // Init structures
        GetComponentInChildren<ChunkGeneration>().Initialize(GetComponentInChildren<TileGrid>(), width, height);

        // Disable underground tilemaps
        GetComponentInChildren<TileGrid>().transform.Find("UndergroundTilemaps").gameObject.SetActive(false);

        // Init time
        GetComponentInChildren<DayAndNightCycle>().Initialize();

        // Start world events
        GetComponent<WorldEvents>().Initialize(this, GetComponentInChildren<TileGrid>(), GetComponentInChildren<DayAndNightCycle>());

        // for testing
        // GameObject.Find("FogTilemap").GetComponent<TilemapRenderer>().enabled = false;
    }
}
