using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnPoint;
    private TilemapStructure groundMap, overworldMap;

    private PlayerPosition player;
    [SerializeField]
    private float maxDistance; // default is 3.5f
    
    public Vector2Int prevPos;
    public Vector2Int currentPos;
    public int prevGTile, prevOTile;
    public int currentGTile, currentOTile;

    // Call other functions when position changes
    public delegate void OnPosChange();
    public OnPosChange PosChange;

    // Call other functions when tile types change
    public delegate void OnGTileChange();
    public OnGTileChange GTileChange;
    public delegate void OnOTileChange();
    public OnOTileChange OTileChange;

    private void Awake()
    {
        // Retrieve tilemap and player components
        RetrieveTilemap();
        player = FindObjectOfType<PlayerPosition>();

        // Get spawn point
        spawnPoint = transform.position;
    }
    
    private void OnEnable()
    {
        // Attach delegates
        // player.SceneChange += RetrieveTilemap;
        PosChange += CheckPosition;
        PosChange += OTileSound;
    }

    private void OnDisable()
    {
        // player.SceneChange -= RetrieveTilemap;
        PosChange -= CheckPosition;
        PosChange -= OTileSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("hit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // look at player when nearby and (chase it?)
        if (GetComponent<EnemyData>().isHostile && CheckPlayer())
        {
            // Debug.Log("I see u");
            if (player.transform.position.x - transform.position.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);
        TempData.tempPos = new Vector3(currentPos.x, currentPos.y);

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }

    private void RetrieveTilemap()
    {
        // Retrieve tilemap components
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
        overworldMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Overworld);
    }

    // Looks at current position
    private void CheckPosition()
    {
        // Get current tiles from position
        currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Ground tile check
        if (currentGTile != prevGTile)
        {
            // GTileChange();
            prevGTile = currentGTile;
        }

        // Object tile check
        if (currentOTile != prevOTile)
        {
            // OTileChange();
            prevOTile = currentOTile;
        }
    }

    private bool CheckPlayer()
    {
        // Calculate current distance from player
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < maxDistance)
        {
            // Debug.Log("hi");
            return true;
        }

        return false;
    }

    // Play sound based on current object tile
    private void OTileSound()
    {
        if (CheckPlayer())
        {
            // Get sound from current object tile
            if (currentOTile == (int)FoilageTileType.Tree)
            {
                FindObjectOfType<AudioManager>().PlayFx("Tree");
            }
        }
    }
}