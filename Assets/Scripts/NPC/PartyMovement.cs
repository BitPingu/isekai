using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour
{
    public float maxSpeed; // default is 5f
    private float moveSpeed;
    public float minDistance; // default is 1.55

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    private Vector2Int currentPos;
    [SerializeField]
    private int currentTile;

    private PlayerPosition player;

    private void Awake()
    {
        // Find player
        player = FindObjectOfType<PlayerPosition>();
        
        // Retrieve components
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Get speed from current tile
        moveSpeed = maxSpeed;

        if (moveSpeed <= 0)
            moveSpeed = 1;

        // Calculate current distance from player
        float distance = Vector3.Distance(player.transform.position, rb.transform.position);

        // Check if outside range
        if (distance > minDistance)
        {
            // Calculate current direction towards player
            Vector2 movement = (player.transform.position - rb.transform.position).normalized;

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

            // Teleport to player if too far
            if (distance > minDistance + 20)
            {
                transform.position = player.transform.position;
            }
        }
        if (distance < minDistance-0.3)
        {
            // Stop moving when within range
            rb.velocity = Vector2.zero;
        }

        // Movement animation
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }
}
