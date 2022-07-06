using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float maxSpeed;
    private float moveSpeed;
    private Vector2 movement;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    private Vector2Int currentPos;
    [SerializeField]
    private int currentTile;
    [SerializeField]
    private TileGrid grid;
    private TilemapStructure objectMap;

    private bool isMoving;
    [SerializeField]
    private float walkTime; // default is 1f
    private float walkCounter;
    private float waitTime;
    private float waitCounter;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;

    private void Awake()
    {
        // Retrieve components of enemy
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Retrieve tilemap component
        grid = FindObjectOfType<TileGrid>();
        objectMap = grid.GetTilemap(TilemapType.Object);

        // Set counters
        waitCounter = waitTime;
        walkCounter = walkTime;

        // Choose direction
        chooseDirection();
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve coordinates of enemy
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Get current tile from enemy position
        currentTile = objectMap.GetTile(currentPos.x, currentPos.y);

        // Get speed from current tile
        moveSpeed = maxSpeed;
        if (currentTile == (int)ObjectTileType.Tree)
            moveSpeed = maxSpeed - 2;

        if (moveSpeed <= 0)
            moveSpeed = 1;

        if (isMoving)
        {
            // Update walk counter
            walkCounter -= Time.deltaTime;

            // Movement
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

            // Flip sprite based on horizontal movement
            if (movement.x > 0)
            {
                sprite.flipX = false;
            }
            else if (movement.x < 0)
            {
                sprite.flipX = true;
            }

            // Stop moving
            if (walkCounter < 0)
            {
                isMoving = false;
                waitCounter = waitTime;
            }
        }
        else
        {
            // Update wait counter
            waitCounter -= Time.deltaTime;

            // Reset values
            rb.velocity = Vector2.zero;
            movement = Vector2.zero;

            // Choose direction
            if (waitCounter < 0)
            {
                chooseDirection();
            }
        }

        // Movement animation
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void chooseDirection()
    {
        // Choose random movement
        movement.x = Random.Range(-1f, 1f);
        movement.y = Random.Range(-1f, 1f);

        // Choose random wait time
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        
        // Start moving
        isMoving = true;
        walkCounter = walkTime;
    }
}
