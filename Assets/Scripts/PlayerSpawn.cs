using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase validTile;

    private Vector2 spawnPoint;
    private TileBase tile;
    private Vector3Int gridPosition;

    [SerializeField]
    private TileGrid mapSize;

    public bool randomize;

    void Awake()
    {
        if (randomize)
        {
            do
            {
                spawnPoint.x = Random.Range(0, mapSize.Width);
                spawnPoint.y = Random.Range(0, mapSize.Height);
                gridPosition = map.WorldToCell(spawnPoint);
                tile = map.GetTile(gridPosition);

            } while (tile != validTile);

            transform.position = spawnPoint;
        }
    }
}
