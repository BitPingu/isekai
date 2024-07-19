using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElfPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnPoint;
    private TilemapStructure groundMap;

    private PlayerPosition player;

    private void Awake()
    {
        // Retrieve tilemap and player components
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
        player = FindObjectOfType<PlayerPosition>();

        // Attacked by slime
        Jump();
    }
    
    private void OnEnable()
    {
        player.SceneChange += Spawn;
    }

    private void OnDisable()
    {
        player.SceneChange -= Spawn;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GetComponent<Animator>().SetBool("Jump", false);
            Debug.Log("hit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x - transform.position.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Jump()
    {
        // only enabling the line below makes it so you can push
        // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        GetComponent<Animator>().SetBool("Jump", true);
    }

    // Generates a random spawn point
    private void Spawn()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Generate initial spawn point
            int xCoord, yCoord, currentTile;
            Vector3 worldSpawn = TempData.tempWorldSpawn;
            do
            {
                // Choose random spawn point
                xCoord = (int)Random.Range(worldSpawn.x-5, worldSpawn.x+5);
                yCoord = (int)Random.Range(worldSpawn.y-5, worldSpawn.y+5);

                // Check tile
                currentTile = groundMap.GetTile(xCoord, yCoord);
            }
            while (currentTile != (int)GroundTileType.Land);

            // Generate spawn point
            spawnPoint = new Vector3(xCoord, yCoord);
            spawnPoint.x += .5f;
            spawnPoint.y += .5f;
            transform.position = spawnPoint;

            // Spawn enemy
            if (groundMap.GetTile(xCoord+1, yCoord) == (int)GroundTileType.Land)
            {
                FindObjectOfType<EnemySpawner>().spawnEnemy("Slime", new Vector3(xCoord+1, yCoord), true);
            }
            else if (groundMap.GetTile(xCoord-1, yCoord) == (int)GroundTileType.Land)
            {
                FindObjectOfType<EnemySpawner>().spawnEnemy("Slime", new Vector3(xCoord-1, yCoord), false);
            }
        }
    }
}
