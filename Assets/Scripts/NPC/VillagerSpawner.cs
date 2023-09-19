using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    [SerializeField]
    private VillagerTypes.VillagerData[] villagers;

    private int currentTile;
    [SerializeField]
    private TileGrid grid;
    private TilemapStructure overworldMap;

    public DayAndNightCycle dayNight;

    private void Awake()
    {
        // Retrieve tilemap component
        overworldMap = grid.GetTilemap(TilemapType.Overworld);

        // Attach delegates
        dayNight.DayTime += spawnVillagers;
        dayNight.NightTime += despawnVillagers;
    }

    private void spawnVillagers()
    {
        for (int x=0; x<grid.width; x++)
        {
            for (int y=0; y<grid.height; y++)
            {
                // Check tile
                //currentTile = objectMap.GetTile(x, y);
                //if (currentTile == (int)FoilageTileType.House && Random.value < 0.5)
                //{
                //    // Spawn villager at spawnPoint
                //    Vector3Int spawnPoint = new Vector3Int(x, y, 0);
                //    GameObject newVillager = Instantiate(villagers[0].villager, spawnPoint + new Vector3(.5f, .5f), Quaternion.identity);
                //    newVillager.transform.parent = gameObject.transform;
                //}
            }
        }
    }

    private void despawnVillagers()
    {
        var clones = GameObject.FindGameObjectsWithTag("Villager");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
