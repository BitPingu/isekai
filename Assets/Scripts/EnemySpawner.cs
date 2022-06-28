using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private PoissonDiscSampling algorithm;

    public Tilemap map;
    public TileBase validTile;
    public List<Vector2> points;
    public float displayRadius = 1;

    void Awake()
    {
        // Determine spawn points using Poisson Disc Sampling
        points = algorithm.GeneratePoints();

        // Spawn enemy
        spawnEnemy();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(algorithm.regionSize / 2, algorithm.regionSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }

    void spawnEnemy()
    {
        foreach (Vector2 point in points)
        {
            Vector3 spawnPoint = new Vector3(point.x, point.y, 0f);

            // Check valid tile
            if (map.GetTile(map.WorldToCell(spawnPoint)) == validTile)
            {
                // Spawn enemy at spawnPoint
                GameObject newEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                newEnemy.transform.parent = gameObject.transform;
            }
        }
    }
}
