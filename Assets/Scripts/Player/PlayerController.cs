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

    public bool isMoving;
    public float walkCounter;

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
        if (!isMoving)
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
            if (Input.GetKeyDown(interactKey) && GetComponent<PlayerPosition>().currentArea.Contains("Dungeon Entrance"))
            {
                Debug.Log("enter dun");
                FindObjectOfType<WorldEvents>().EnterDungeon(GetComponent<PlayerPosition>());
            }
        }
        else
        {
            // Get target position
            Vector3 targetPos = FindObjectOfType<EnterBuilding>().activePos;

            // Update walk counter
            walkCounter -= Time.deltaTime;

            // Calculate current direction towards enemy
            Vector2 movement = (targetPos - rb.transform.position).normalized;

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
                rb.velocity = Vector2.zero;
            }
        }
    }
}
