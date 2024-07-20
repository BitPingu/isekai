using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyTypes[] enemies;
    [SerializeField]
    private PoissonDiscSampling algorithm;
    private List<Vector2> points = new List<Vector2>();
    [SerializeField]
    private float displayRadius = 1;

    private Vector3 spawnPoint;

    private int currentTile;

    private TilemapStructure groundMap;

    public DayAndNightCycle dayNight;

    private void Awake()
    {
        // Retrieve tilemap component
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);

        // Attach delegates
        dayNight.DayTime += dayEnemies;
        dayNight.NightTime += nightEnemies;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(algorithm.regionSize / 2, algorithm.regionSize);
    //     if (points != null)
    //     {
    //         foreach (Vector2 point in points)
    //         {
    //             Gizmos.DrawSphere(point, displayRadius);
    //         }
    //     }
    // }

    private void dayEnemies()
    {
        points.Clear();
        despawnEnemies();
        foreach (EnemyTypes enemy in enemies)
        {
            if (!enemy.nightEnemy)
            {
                // Determine spawn points using Poisson Disc Sampling
                points = algorithm.GeneratePoints();

                // Spawn day enemies
                spawnEnemy(enemy);
            }
        }
    }

    private void nightEnemies()
    {
        points.Clear();
        despawnEnemies();
        foreach (EnemyTypes enemy in enemies)
        {
            if (enemy.nightEnemy)
            {
                // Determine spawn points using Poisson Disc Sampling
                points = algorithm.GeneratePoints();

                // Spawn night enemies
                spawnEnemy(enemy);
            }
        }
    }

    private void spawnEnemy(EnemyTypes enemy)
    {
        foreach (Vector2 point in points)
        {
            // Create new spawn point from list
            spawnPoint = new Vector3(point.x, point.y, 0);

            // Check tile
            currentTile = groundMap.GetTile((int)spawnPoint.x, (int)spawnPoint.y);

            // Check valid tile
            if (currentTile == (int)GroundTileType.Land)
            {
                // Instantiate and spawn enemy
                GameObject childEnemy = Instantiate(enemy.gameObject, spawnPoint, Quaternion.identity);
                childEnemy.transform.parent = gameObject.transform;
            }
            else
            {
                // Debug.Log("enemy couldn't spawn :(");
            }
        }
    }

    public void spawnEnemy(string type, Vector3 spawnPoint, bool flipX)
    {
        // Check tile
        currentTile = groundMap.GetTile(Mathf.FloorToInt(spawnPoint.x), Mathf.FloorToInt(spawnPoint.y));

        // Check valid tile
        if (currentTile != (int)GroundTileType.Land)
        {
            Debug.Log("enemy couldn't spawn :(");
            return;
        }

        // Get enemy
        foreach (EnemyTypes enemy in enemies)
        {
            if (enemy.gameObject.name.Equals(type))
            {
                // Instantiate and spawn enemy
                GameObject childEnemy = Instantiate(enemy.gameObject, spawnPoint, Quaternion.identity);
                childEnemy.transform.parent = gameObject.transform;
                // childEnemy.GetComponent<EnemyData>().set(spawnPoint);

                // Assign special values
                childEnemy.tag = "SpecialEnemy";
                childEnemy.GetComponent<NPCMovement>().enabled = false;
                childEnemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                childEnemy.GetComponent<SpriteRenderer>().flipX = flipX;
                childEnemy.GetComponent<Animator>().SetBool("Attack", true);
                break;
            }
        }
    }

    public void despawnEnemies()
    {
        var clones = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }
}
