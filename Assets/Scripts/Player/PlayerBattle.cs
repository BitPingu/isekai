using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private Vector3 battlePos;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private bool stance, isMoving;
    private float walkCounter;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (stance)
        {
            if (isMoving)
            {
                // Get enemy position
                Vector3 enemyPos = FindObjectOfType<BattleManager>().enemy.transform.position;

                // Update walk counter
                walkCounter -= Time.deltaTime;

                // Calculate current direction towards enemy
                Vector2 movement = (enemyPos - rb.transform.position).normalized;

                // Move away from enemy
                movement *= -1;

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

                // Stop moving
                if (walkCounter < 0)
                {
                    isMoving = false;
                }
            }
            else
            {
                // Stop moving
                rb.velocity = Vector2.zero;

                if (!sprite.flipX)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
                stance = false;
            }

            // Movement animation
            animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        }
    }

    public void Stance()
    {
        walkCounter = .4f;
        stance = true;
        isMoving = true;
    }

    public void Run()
    {
        walkCounter = 1f;
        stance = true;
        isMoving = true;
    }

}
