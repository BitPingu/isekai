using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour
{
    public float maxSpeed; // default is 3f 3 3 2
    private float moveSpeed;
    [SerializeField]
    private float minDistance; // default is 1.5f

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    private Vector2Int currentPos;
    [SerializeField]
    private int currentTile;
    private TilemapStructure overworldMap;

    private PlayerPosition player;

    private void Awake()
    {
        // Retrieve components
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Retrieve tilemap component
        overworldMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Overworld);
        player = FindObjectOfType<PlayerPosition>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Get current tile
        currentTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Get speed from current tile
        moveSpeed = maxSpeed;
        if (currentTile == (int)FoilageTileType.Tree)
            moveSpeed = maxSpeed - 2;

        if (moveSpeed <= 0)
            moveSpeed = 1;

        // Movement animation
        animator.SetBool("Move", transform.hasChanged);
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
        }
    }

    private void FixedUpdate()
    {
        // Calculate current direction towards player
        Vector3 dir = (player.transform.position - rb.transform.position).normalized;

        // Calculate current distance from player
        float distance = Vector3.Distance(player.transform.position, rb.transform.position);

        // Check if outside range
        if (distance > minDistance)
        {
            // Move towards player
            moveSpeed+=1;
            rb.MovePosition(rb.transform.position + dir * moveSpeed * Time.fixedDeltaTime);

            // Flip sprite based on horizontal movement
            if (dir.x < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        // Reset values
        rb.velocity = Vector2.zero;
    }
}
