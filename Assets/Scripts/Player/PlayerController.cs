using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 movement;
    private Vector2 moveForce;
    public float maxSpeed; // Default is 5
    
    [SerializeField]
    private KeyCode interactKey;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed;

    private void Awake()
    {
        // Retrieve components of player
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set speed
        moveSpeed = maxSpeed;

        GetComponent<PlayerBattle>().enabled = false;
    }

    private void Update()
    {
        // Movement
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        moveForce = movement * moveSpeed;
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
}
