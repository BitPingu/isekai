using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElfPosition : MonoBehaviour
{
    public Vector3 spawnPoint;
    private TilemapStructure groundMap, overworldMap;

    // [SerializeField]
    private PlayerPosition player;
    [SerializeField]
    private float maxDistance; // default is 3.5f

    public bool inDanger;
    
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
        player = FindObjectOfType<PlayerPosition>();
        // groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);

        // Attach delegates
        // player.SceneChange += RetrieveTilemap;
        PosChange += CheckPosition;
        PosChange += OTileSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inDanger && collision.gameObject.name == "Player")
        {
            inDanger = false;
            GetComponent<Animator>().SetBool("Jump", false);
            GetComponent<PartyMovement>().enabled = true;
            Debug.Log("hit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inDanger)
        {
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

    // Generates a random spawn point
    public void Spawn()
    {
        RetrieveTilemap();
        // Generate initial spawn point
        float xCoord, yCoord, currentTile;
        Vector3 worldSpawn = TempData.tempWorldSpawn;
        do
        {
            // Choose random spawn point
            xCoord = Random.Range(worldSpawn.x-5, worldSpawn.x+5);
            yCoord = Random.Range(worldSpawn.y-5, worldSpawn.y+5);

            // Check tile
            currentTile = groundMap.GetTile((int)xCoord, (int)yCoord);
        }
        while (currentTile != (int)GroundTileType.Land);

        // Generate spawn point
        spawnPoint = new Vector3(xCoord, yCoord);

        // Set spawn point
        transform.position = spawnPoint;
        prevPos = currentPos = Vector2Int.FloorToInt(transform.position);

        // Retrieve spawn point tile
        prevGTile = currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        prevOTile = currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Attacked by slime
        inDanger = true;
        GetComponent<Animator>().SetBool("Jump", true);
        GetComponent<PartyMovement>().enabled = false;
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
