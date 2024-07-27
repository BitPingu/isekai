using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float maxSpeed; // default is 3f 3 3 2
    private float moveSpeed;
    private Vector2 movement;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    private Vector2Int currentPos;
    [SerializeField]
    private int currentTile;
    private TilemapStructure overworldMap;

    private bool isMoving;
    private bool seePlayer;
    [SerializeField]
    private float walkTime; // default is 1f 1 .5 1
    private float walkCounter;
    private float waitTime;
    private float waitCounter;
    [SerializeField]
    private float minWaitTime; // default is 3f
    [SerializeField]
    private float maxWaitTime; // default is 5f

    private void Awake()
    {
        // Retrieve components of enemy
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Retrieve tilemap component
        overworldMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Overworld);

        // Set counters
        waitCounter = waitTime;
        walkCounter = walkTime;

        // Choose direction
        chooseDirection();

        GetComponent<EnemyBattle>().enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve coordinates of enemy
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Get current tile from enemy position
        currentTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Get speed from current tile
        moveSpeed = maxSpeed;
        if (currentTile == (int)FoilageTileType.Tree)
            moveSpeed = maxSpeed - 2;

        if (moveSpeed <= 0)
            moveSpeed = 1;

        // look at player when nearby and chase it
        if (GetComponent<EnemyData>().isHostile && GetComponent<EnemyPosition>().CheckPlayer())
        {
            // Calculate current direction towards player
            waitCounter = waitTime;
            Vector2 movement = (FindObjectOfType<PlayerPosition>().transform.position - rb.transform.position).normalized;

            // Move towards player
            Vector2 moveForce = movement * (moveSpeed+1);
            moveForce /= 1.2f;
            rb.velocity = moveForce;

            // Flip sprite based on horizontal movement
            if (movement.x < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        if (isMoving)
        {
            // Update walk counter
            walkCounter -= Time.deltaTime;

            // Movement
            Vector2 moveForce = movement * (moveSpeed+.5f);
            moveForce /= 1.2f;
            rb.velocity = moveForce;

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
                rb.velocity = Vector2.zero;
                isMoving = false;

                // Reset wait time
                waitCounter = waitTime;
            }
        }
        else
        {
            // Update wait counter
            waitCounter -= Time.deltaTime;

            // Choose direction
            if (waitCounter < 0)
            {
                chooseDirection();
            }
        }

        // Movement animation
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
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

        // Reset walk time
        walkCounter = walkTime;
    }
}
