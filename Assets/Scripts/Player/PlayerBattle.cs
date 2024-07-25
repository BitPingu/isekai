using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private Vector3 battlePos;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private TilemapStructure groundMap;
    private bool stance;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stance = false;
    }

    private void Update()
    {
        if (stance)
        {
            // Calculate current distance from pos
            float distance = Vector3.Distance(battlePos, rb.transform.position);

            // Check if outside range
            if (distance > 0)
            {
                // Calculate current direction towards pos
                Vector2 movement = (battlePos - rb.transform.position).normalized;

                // Move towards pos
                float moveSpeed = GetComponent<PlayerController>().maxSpeed;
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
            if (distance < 0.1) 
            {
                // Stop moving when within range
                rb.velocity = Vector2.zero;

                if (!sprite.flipX)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
                stance = false;
            }
        }

        // Movement animation
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }

    public void Stance()
    {
        // Generate random battle position
        float xCoord, yCoord, currentTile;
        // Get enemy position
        Vector3 enemyPos = FindObjectOfType<BattleManager>().enemy.transform.position;
        do
        {
            // Choose random spawn point
            xCoord = Random.Range(enemyPos.x-1.5f, enemyPos.x+1.5f);
            yCoord = Random.Range(enemyPos.y-1.5f, enemyPos.y+1.5f);

            // Check tile
            groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
            currentTile = groundMap.GetTile((int)xCoord, (int)yCoord);
        }
        while (currentTile != (int)GroundTileType.Land || (xCoord < enemyPos.x+0.5 && xCoord > enemyPos.x-0.5) || (xCoord < enemyPos.y+0.5 && xCoord > enemyPos.y-0.5));

        // Generate battle pos
        battlePos = new Vector3(xCoord, yCoord);

        stance = true;
    }

    
}
