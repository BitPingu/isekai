using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnPoint;
    private TilemapStructure groundMap;

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

        // For goblins
        if (name.Contains("Goblin"))
        {
            GameObject[] camps = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject camp in camps)
            {
                float distance = Vector3.Distance(camp.transform.position, transform.position);
                if (camp.name.Contains("Camp") && distance <= 1.6)
                {
                    if (camp.transform.position.x - transform.position.x > 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }
        }
    }
    
    private void OnEnable()
    {
        // Attach delegates
        PosChange += CheckPosition;
        PosChange += OTileSound;
    }

    private void OnDisable()
    {
        PosChange -= CheckPosition;
        PosChange -= OTileSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            FindObjectOfType<BattleManager>().Initiate(collision.gameObject, gameObject);
        }
        if (gameObject.tag.Equals("SpecialEnemy"))
        {
            FindObjectOfType<ElfPosition>().SaveElf();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);

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
        // groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
        // overworldMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Overworld);
    }

    // Looks at current position
    private void CheckPosition()
    {
        // Get current tiles from position
        // currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        // currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

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

    public bool CheckPlayer()
    {
        // Calculate current distance from player
        // float distance = Vector3.Distance(player.transform.position, transform.position);

        // if (distance < maxDistance)
        // {
        //     // Debug.Log("hi");
        //     return true;
        // }

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
