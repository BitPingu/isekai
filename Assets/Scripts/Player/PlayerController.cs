using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed; // Default is 5
    [SerializeField]
    private KeyCode interactKey;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerPosition position;
    private float moveSpeed;

    public Collision2D currentCol;

    private void Awake()
    {
        // Retrieve components of player
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        position = GetComponent<PlayerPosition>();

        // Set speed
        moveSpeed = maxSpeed;

        // Attach delegates
        position.OTileChange += ChangeSpeed;

        GetComponent<PlayerBattle>().enabled = false;
    }

    private void Update()
    {
        // Movement
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 moveForce = movement * moveSpeed;
        rb.velocity = moveForce;

        // Movement animation
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        // Flip sprite based on horizontal movement
        if (movement.x > 0)
        {
            sprite.flipX = false;
        } 
        else if (movement.x < 0)
        {
            sprite.flipX = true;
        }

        // Interactable check
        if (Input.GetKeyDown(interactKey))
        {
            FindObjectOfType<WorldEvents>().EnterBuilding();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentCol = collision;
    }

    // Set speed based on current tile
    private void ChangeSpeed()
    {
        // Get speed from current object tile
        if (position.currentOTile == (int)FoilageTileType.Tree)
        {
            moveSpeed = maxSpeed - 2;
        }
        else
        {
            moveSpeed = maxSpeed;
        }
    }
}
