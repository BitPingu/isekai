using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    private float moveSpeed;
    private Vector2 movement;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    public Vector2Int currentPos;
    [SerializeField]
    private int currentTile;
    [SerializeField]
    private TileGrid grid;
    private TilemapStructure objectMap;

    private void Awake()
    {
        // Retrieve components of player
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Retrieve tilemap component
        objectMap = grid.GetTilemap(TilemapType.Object);
    }

    private void Update()
    {
        // Retrieve coordinates of player
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Get current tile from player position
        currentTile = objectMap.GetTile(currentPos.x, currentPos.y);

        // Get speed from current tile
        moveSpeed = maxSpeed;
        if (currentTile == (int)ObjectTileType.Tree)
            moveSpeed = maxSpeed - 2;

        // Input
        if (!PauseMenu.GameIsPaused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        // Flip sprite based on horizontal movement
        if (movement.x > 0)
        {
            sprite.flipX = false;
        } else if (movement.x < 0)
        {
            sprite.flipX = true;
        }

        // Movement animation
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // Executed on a fixed timer instead of frame rate
    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

}
