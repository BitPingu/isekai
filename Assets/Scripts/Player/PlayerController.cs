using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed; // Default is 5
    [SerializeField]
    private KeyCode interactKey;

    private Vector2 movement;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerPosition position;
    private float moveSpeed;

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
    }

    private void Update()
    {
        // Input
        if (!MenuController.openMenu)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        // Flip sprite based on horizontal movement
        if (movement.x > 0)
        {
            sprite.flipX = false;
        } 
        else if (movement.x < 0)
        {
            sprite.flipX = true;
        }

        // Movement animation
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Interactable check
        if (Input.GetKeyDown(interactKey))
        {
            EnterBuilding();
        }
    }

    // Executed on a fixed timer instead of frame rate
    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
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

    // Change scene based on current tile
    private void EnterBuilding()
    {
        switch (position.currentOTile)
        {
            case (int)BuildingTileType.House:
                FindObjectOfType<AudioManager>().Stop();
                if (SceneManager.GetActiveScene().buildIndex == 1) 
                {
                    TempData.tempSpawnPoint = new Vector3(position.currentPos.x, position.currentPos.y);
                    Debug.Log("Enter Village");
                    SceneManager.LoadScene("Village");
                }
                else
                {
                    Debug.Log("Exit Village");
                    SceneManager.LoadScene("Overworld");
                }
                break;
            case (int)BuildingTileType.Dungeon:
                FindObjectOfType<AudioManager>().Stop();
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    TempData.tempSpawnPoint = new Vector3(position.currentPos.x, position.currentPos.y);
                    Debug.Log("Enter Dungeon");
                    SceneManager.LoadScene("Dungeon");
                }
                else
                {
                    Debug.Log("Exit Dungeon");
                    SceneManager.LoadScene("Overworld");
                }
                break;
            default:
                Debug.Log("No interactable tile!");
                break;
        }
    }
}
