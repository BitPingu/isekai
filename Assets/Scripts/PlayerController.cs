using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    private float moveSpeed;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;

    private Vector2 movement;

    [SerializeField]
    private TileInteraction tile;

    void Update()
    {
        // Get speed from current tile
        moveSpeed = maxSpeed;
        if (tile.tileObstruction(transform.position))
            moveSpeed = maxSpeed - 2;

        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

}
