using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    private TilemapStructure villageMap;
    public GameObject villager;
    private bool villagersAlreadySpawned;

    public void Initialize(TileGrid grid)
    {
        // Retrieve tilemap component
        villageMap = grid.GetTilemap(TilemapType.Village);

        // Spawn init villagers
        Spawn();
    }

    public void Spawn()
    {
        if (!villagersAlreadySpawned)
        {
            villagersAlreadySpawned = true;
            for (int x=0; x<villageMap.width; x++)
            {
                for (int y=0; y<villageMap.height; y++)
                {
                    // Check tile
                    if (villageMap.GetTile(x, y) == (int)GroundTileType.VillagePlot)
                    {
                    // Spawn villager at spawnPoint
                    Vector3 spawnPoint = new Vector3(x, y-.5f, 0);
                    Instantiate(villager, spawnPoint, Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    public void Despawn()
    {
        villagersAlreadySpawned = false;
        var clones = GameObject.FindGameObjectsWithTag("Villager");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
