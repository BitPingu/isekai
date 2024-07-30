using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int width, height, seed;
    public Vector2 islandRegionSize;
    public GameObject test;

    private void Awake()
    {
        // Generate world seed
        seed = UnityEngine.Random.Range(-100000, 100000);
        TempData.tempSeed = seed;

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);

        islandRegionSize = GetComponentInChildren<GroundGeneration>().islandRegionSize;
        // Instantiate(test, islandRegionSize, Quaternion.identity);

        // Init vegetation
        GetComponentInChildren<TreeGeneration>().Initialize(width, height, seed);
    }
}
