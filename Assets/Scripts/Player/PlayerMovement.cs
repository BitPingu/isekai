using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    PlayerController controller;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Transform movePoint;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Tilemap tree, water;

    private Vector2 movement;
    private Vector3Int detectWater;
    private Vector3 position;
    private Vector3Int insideTree;
    private SpriteRenderer sprite;
    private float timeDelay = 0;

    private void Awake()
    {
        // Player spawn point
        transform.position = new Vector3(Noise.halfWidth + 0.5f, Noise.halfHeight + 0.6f, 1);

        // Get sprite
        sprite = GetComponent<SpriteRenderer>();

        // Set move point
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine current tile
        position = transform.position;

        // Determine tree tile
        insideTree = tree.WorldToCell(position);

        // Change movePoint position based on input
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            // Disable diagonal movement (only 1 orientation at a time)
            if (movement.y == 0)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                movement.x = Input.GetAxisRaw("Horizontal");
            }
            if (movement.x == 0)
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                movement.y = Input.GetAxisRaw("Vertical");
            }
        }

        // Determine coordinates of water based on movePoint position
        detectWater = new Vector3Int(Mathf.FloorToInt(movePoint.position.x), Mathf.FloorToInt(movePoint.position.y), 0);

        // Disable input during movement
        if (movement.sqrMagnitude > 0.001f)
        {
            controller.OnDisable();
            // Reset time delay
            timeDelay = 0;
        } else
        {
            if (timeDelay > 0.1)
            {
                controller.OnEnable();
            }
            // Start time delay
            timeDelay += Time.deltaTime;
        }

        // Change animation sprite (only when moving to prevent going back to inital state)
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        // Movement animation
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // FixedUpdate is called once, zero, or several times per frame
    void FixedUpdate()
    {
        // Collision check
        if (checkObstacle())
        {
            // Move player based on movement vectors and speed
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.fixedDeltaTime);
        }

        // Change sorting order
        if (tree.GetTile(insideTree) != null && position.y > insideTree.y + 0.5)
        {
            sprite.GetComponent<SpriteRenderer>().sortingOrder = tree.GetComponent<TilemapRenderer>().sortingOrder - 1;
        } else
        {
            sprite.GetComponent<SpriteRenderer>().sortingOrder = tree.GetComponent<TilemapRenderer>().sortingOrder + 1;
        }

        // Tree sound
        if (tree.GetTile(insideTree) != null && !FindObjectOfType<AudioManager>().sounds[2].source.isPlaying && movement.sqrMagnitude > 0.001f)
        {
            FindObjectOfType<AudioManager>().Play("Tree");
        }

        // Ocean sound
        if (water.GetTile(detectWater) != null && !FindObjectOfType<AudioManager>().sounds[3].source.isPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Ocean");
        }
    }

    private bool checkObstacle()
    {
        // If obstacle is found or map is on
        if ((water.GetTile(detectWater) != null))
        {
            // Reset movePoint
            movePoint.position = position;
            return false;
        }
        return true;
    }
}
