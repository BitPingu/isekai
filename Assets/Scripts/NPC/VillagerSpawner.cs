using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    private TilemapStructure villageMap;
    private DayAndNightCycle time;
    public GameObject villager;

    public void Initialize(TileGrid grid, DayAndNightCycle dayNight)
    {
        // Retrieve tilemap component and time
        villageMap = grid.GetTilemap(TilemapType.Village);
        time = dayNight;
    }

    public void Spawn()
    {
        var villages = GameObject.FindGameObjectsWithTag("Landmark");
        foreach (GameObject village in villages)
        {
            if (village.name.Contains("Village"))
            {
                foreach (Vector2 lot in village.GetComponent<VillageData>().lots)
                {
                    // Spawn villager at spawnPoint
                    Vector3 spawnPoint = new Vector3(lot.x, lot.y-.5f, 0);
                    GameObject v = Instantiate(villager, spawnPoint, Quaternion.identity, village.transform);
                    village.GetComponent<VillageData>().villagers.Add(v);
                    if (!village.GetComponent<VillageData>().containsPlayer)
                        v.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Despawn()
    {
        var villages = GameObject.FindGameObjectsWithTag("Landmark");
        foreach (GameObject village in villages)
        {
            if (village.name.Contains("Village"))
            {
                foreach (GameObject villager in village.GetComponent<VillageData>().villagers)
                {
                    villager.SetActive(false);
                }
            }
        }
    }
}
