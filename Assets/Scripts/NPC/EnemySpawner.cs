using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class EnemyTypes
    {
        public GameObject gameObject;
        public Vector3 spawnPoint;
        public bool nightEnemy;
    }

    [SerializeField]
    private EnemyTypes[] enemies;

    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    private List<Vector2> enemySpawnPoints = new List<Vector2>();


    [SerializeField]
    private GoblinAlgorithm goblinAlgorithm;

    private Vector3 spawnPoint;

    private int currentTile;

    private TilemapStructure groundMap;

    public DayAndNightCycle dayNight;

    private void Awake()
    {
        // Retrieve tilemap component
        // groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);

        // Attach delegates
        // dayNight.DayTime += dayEnemies;
        // dayNight.NightTime += nightEnemies;
    }

    public void Initialize(TilemapStructure tilemap, DayAndNightCycle dayNight)
    {
        // Get tilemap structure
        groundMap = tilemap;

        // Spawn init enemies
        dayEnemies();
    }

    public void dayEnemies()
    {
        despawnEnemies();
        // Generate enemy spawn points (might change algo to random chance later ie more common in forested areas)
        enemySpawnPoints = sampling.GeneratePoints(groundMap);

        foreach (EnemyTypes enemy in enemies)
        {
            switch (enemy.gameObject.name)
            {
                case "Slime":
                    // Spawn overworld enemies
                    spawnEnemy(enemy);
                    break;
                // case "Goblin":
                //     points = goblinAlgorithm.GeneratePoints();
                //     spawnEnemy(enemy);
                //     break;
                default:
                    break;
            }
        }
    }

    public void nightEnemies()
    {
        despawnEnemies();
        // Generate enemy spawn points (might change algo to random chance later ie more common in forested areas)
        enemySpawnPoints = sampling.GeneratePoints(groundMap);

        foreach (EnemyTypes enemy in enemies)
        {
            switch (enemy.gameObject.name)
            {
                case "Zombie":
                    // Spawn overworld enemies
                    spawnEnemy(enemy);
                    break;
            }
        }
    }

    private void spawnEnemy(EnemyTypes enemy)
    {
        // Spawn enemies
        foreach (Vector2 point in enemySpawnPoints)
        {
            // Skip water coords
            if (groundMap.GetTile(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)) == (int)GroundTileType.Water)
                continue;

            // Create new spawn point from list
            spawnPoint = new Vector3(point.x, point.y, 0);

            // Instantiate and spawn enemy
            Instantiate(enemy.gameObject, spawnPoint, Quaternion.identity, transform);
        }
    }

    public void spawnEnemy(string type, Vector3 spawnPoint, bool flipX)
    {
        // Skip water coords
        if (groundMap.GetTile(Mathf.FloorToInt(spawnPoint.x), Mathf.FloorToInt(spawnPoint.y)) == (int)GroundTileType.Water)
        {
            Debug.Log("enemy will spawn on water!");
        }

        // Get enemy
        foreach (EnemyTypes enemy in enemies)
        {
            if (enemy.gameObject.name.Equals(type))
            {
                // Instantiate and spawn enemy
                GameObject childEnemy = Instantiate(enemy.gameObject, spawnPoint, Quaternion.identity, transform);

                // Assign special values
                childEnemy.tag = "SpecialEnemy";
                childEnemy.GetComponent<NPCMovement>().enabled = false;
                childEnemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                childEnemy.GetComponent<SpriteRenderer>().flipX = flipX;
                childEnemy.GetComponent<Animator>().SetBool("Battle", true);
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
