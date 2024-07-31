using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    private TilemapStructure groundMap;
    public GameObject villager;

    public void Initialize(TilemapStructure tilemap)
    {
        // Retrieve tilemap component
        groundMap = tilemap;

        // Spawn init villagers
        Spawn();
    }

    public void Spawn()
    {
        for (int x=0; x<groundMap.width; x++)
        {
            for (int y=0; y<groundMap.height; y++)
            {
                // Check tile
                if (groundMap.GetTile(x, y) == (int)GroundTileType.VillagePlot && Random.value < 0.5)
                {
                   // Spawn villager at spawnPoint
                   Vector3Int spawnPoint = new Vector3Int(x, y, 0);
                   Instantiate(villager, spawnPoint, Quaternion.identity, transform);
                }
            }
        }
    }

    public void Despawn()
    {
        var clones = GameObject.FindGameObjectsWithTag("Villager");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
