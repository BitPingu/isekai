using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    public GameObject villager;

    public void Spawn()
    {
        var villages = FindObjectsOfType<VillageData>();
        foreach (VillageData village in villages)
        {
            foreach (Vector2 lot in village.lots)
            {
                // Spawn villager at spawnPoint
                Vector3 spawnPoint = new Vector3(lot.x, lot.y-.5f, 0);
                GameObject v = Instantiate(villager, spawnPoint, Quaternion.identity, village.transform);
                village.villagers.Add(v);
                if (!village.containsPlayer)
                    v.SetActive(false);
            }
            village.gameObject.SetActive(false);
        }
    }

    public void Despawn()
    {
        var villages = FindObjectsOfType<VillageData>();
        foreach (VillageData village in villages)
        {
            foreach (GameObject villager in village.villagers)
            {
                villager.SetActive(false);
            }
        }
    }
}
