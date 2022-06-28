using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float maxSpeed;
    private float moveSpeed;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;

    private Vector2 movement;

    [SerializeField]
    private TileInteraction tile;

    public bool isWalking;
    public float walkTime;
    private float walkCounter;
    public float waitTime;
    private float waitCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        chooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            // Get speed from current tile
            moveSpeed = maxSpeed;
            if (tile.tileObstruction(transform.position))
                moveSpeed = maxSpeed - 2;

            if (movement.x > 0)
            {
                sprite.flipX = false;
            }
            else if (movement.x < 0)
            {
                sprite.flipX = true;
            }

            // Movement
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;

            rb.velocity = Vector2.zero;
            movement = Vector2.zero;

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
        movement.x = Random.Range(-1f, 1f);
        movement.y = Random.Range(-1f, 1f);
        waitTime = Random.Range(3f, 5f);
        walkTime = Random.Range(0.5f, 1f);
        
        isWalking = true;
        walkCounter = walkTime;
    }
}
