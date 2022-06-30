using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyTypes.EnemyData[] enemies;
    [SerializeField]
    private PoissonDiscSampling algorithm;
    [SerializeField]
    private List<Vector2> points;
    [SerializeField]
    private float displayRadius = 1;

    private Vector3Int spawnPoint;

    private int currentTile;
    [SerializeField]
    private TileGrid grid;
    private TilemapStructure groundMap;

    public DayAndNightCycle dayNight;

    private void Awake()
    {
        // Retrieve tilemap component
        groundMap = grid.GetTilemap(TilemapType.Ground);

        // Attach delegates
        dayNight.DayTime += dayEnemies;
        dayNight.NightTime += nightEnemies;
    }

    private void OnDrawGizmos()
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

    private void dayEnemies()
    {
        points.Clear();
        despawnEnemies();
        foreach (EnemyTypes.EnemyData enemyType in enemies)
        {
            if (!enemyType.nightEnemy)
            {
                // Determine spawn points using Poisson Disc Sampling
                points = algorithm.GeneratePoints();

                // Spawn day enemies
                spawnEnemy(enemyType.enemy);
            }
        }
    }

    private void nightEnemies()
    {
        points.Clear();
        despawnEnemies();
        foreach (EnemyTypes.EnemyData enemyType in enemies)
        {
            if (enemyType.nightEnemy)
            {
                // Determine spawn points using Poisson Disc Sampling
                points = algorithm.GeneratePoints();

                // Spawn night enemies
                spawnEnemy(enemyType.enemy);
            }
        }
    }

    private void spawnEnemy(GameObject enemy)
    {
        foreach (Vector2 point in points)
        {
            // Create new spawn point from list
            spawnPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

            // Check tile
            currentTile = groundMap.GetTile(spawnPoint.x, spawnPoint.y);

            // Check valid tile
            if (currentTile == (int)GroundTileType.Land)
            {
                // Spawn enemy at spawnPoint
                GameObject newEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                newEnemy.transform.parent = gameObject.transform;
            }
        }
    }

    private void despawnEnemies()
    {
        var clones = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
