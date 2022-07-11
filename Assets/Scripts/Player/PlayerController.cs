using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed; // Default is 5
    public float moveSpeed;
    private Vector2 movement;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    public Vector2Int prevPos;
    public Vector2Int currentPos;

    public int prevGTile, prevOTile;
    public int currentGTile, currentOTile;

    private List<KeyValuePair<Vector2Int, int>> neighbours;
    public int dominantTile;

    // Call other functions when player position changes
    public delegate void OnPosChanged();
    public OnPosChanged PosChange;

    // Call other functions when tile types change
    public delegate void OnGTileChange();
    public OnGTileChange GTileChange;
    public delegate void OnOTileChange();
    public OnOTileChange OTileChange;

    [SerializeField]
    private TileGrid grid;
    private TilemapStructure groundMap, objectMap;

    [SerializeField]
    private FogData fog;

    private void Awake()
    {
        // Retrieve components of player
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Retrieve tilemap components
        groundMap = grid.GetTilemap(TilemapType.Ground);
        objectMap = grid.GetTilemap(TilemapType.Object);

        // Attach delegates
        PosChange += CheckPosition;
        OTileChange += ChangeSpeed;
        PosChange += OTileSound;
        PosChange += CheckNearby;
    }

    private void Start()
    {
        // Retrieve player spawn point
        prevPos = currentPos = Vector2Int.FloorToInt(transform.position);

        // Retrieve spawn point tile
        prevGTile = currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        prevOTile = currentOTile = objectMap.GetTile(currentPos.x, currentPos.y);

        // Set speed
        moveSpeed = maxSpeed;

        // Clear spawn area fog
        fog.ClearFog();
    }

    private void Update()
    {
        // Retrieve coordinates of player
        currentPos = Vector2Int.FloorToInt(transform.position);

        // Input
        if (!MenuManager.openMenu)
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

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }

    // Executed on a fixed timer instead of frame rate
    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // Looks at current position
    private void CheckPosition()
    {
        // Get current tiles from player position
        currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        currentOTile = objectMap.GetTile(currentPos.x, currentPos.y);

        // Ground tile check
        if (currentGTile != prevGTile)
        {
            GTileChange();
            prevGTile = currentGTile;
        }

        // Object tile check
        if (currentOTile != prevOTile)
        {
            OTileChange();
            prevOTile = currentOTile;
        }
    }

    // Set speed based on current tile
    private void ChangeSpeed()
    {
        // Get speed from current object tile
        if (currentOTile == (int)ObjectTileType.Tree)
        {
            moveSpeed = maxSpeed - 2;
        }
        else
        {
            moveSpeed = maxSpeed;
        }
    }

    // Play sound based on current object tile
    private void OTileSound()
    {
        // Get sound from current object tile
        if (currentOTile == (int)ObjectTileType.Tree)
        {
            FindObjectOfType<AudioManager>().PlayFx("Tree");
        }
    }

    // Looks at adjacent tiles around player
    public void CheckNearby()
    {
        neighbours = groundMap.GetNeighbors(currentPos.x, currentPos.y);

        int landTiles = 0, villageTiles = 0, waterTiles = 0;

        foreach (var neighbour in neighbours)
        {
            if (neighbour.Value == (int)GroundTileType.Land)
                landTiles++;

            if (neighbour.Value == (int)GroundTileType.Village)
                villageTiles++;

            if (neighbour.Value == (int)GroundTileType.Water)
                waterTiles++;
        }

        // Compare village tiles with land tiles
        if (villageTiles >= landTiles-2)
        {
            dominantTile = (int)GroundTileType.Village;
        }
        else if (landTiles-2 > villageTiles)
        {
            dominantTile = (int)GroundTileType.Land;
        }

        // Compare water tiles with land tiles
        if (waterTiles >= landTiles - 2)
        {
            dominantTile = (int)GroundTileType.Water;
        }
        else if (waterTiles - 2 > villageTiles)
        {
            dominantTile = (int)GroundTileType.Land;
        }
    }
}
