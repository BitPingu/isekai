using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int width, height, seed;
    public Vector3 minIslandCoords, maxIslandCoords;
    public GameObject test;

    private void Awake()
    {
        // Generate world seed
        seed = UnityEngine.Random.Range(-100000, 100000);
        TempData.tempSeed = seed;

        // Init tile grid
        GetComponentInChildren<TileGrid>().Initialize(width, height, seed);

        minIslandCoords = GetComponentInChildren<GroundGeneration>().minIslandCoords;
        maxIslandCoords = GetComponentInChildren<GroundGeneration>().maxIslandCoords;
        Instantiate(test, minIslandCoords, Quaternion.identity);
        Instantiate(test, maxIslandCoords, Quaternion.identity);

        // Init vegetation
        GetComponentInChildren<TreeGeneration>().Initialize(width, height, seed);
    }
}
