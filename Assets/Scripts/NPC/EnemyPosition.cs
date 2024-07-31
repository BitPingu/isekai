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
    public Vector2Int currentPos;

    private void Awake()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
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
    }

    public bool CheckPlayer()
    {
        // Find player
        player = FindObjectOfType<PlayerPosition>();
        
        // Calculate current distance from player
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < maxDistance)
        {
            return true;
        }

        return false;
    }

    // Play sound based on current object tile
    private void OTileSound()
    {
        // if (CheckPlayer())
        // {
        //     // Get sound from current object tile
        //     if (currentOTile == (int)FoilageTileType.Tree)
        //     {
        //         FindObjectOfType<AudioManager>().PlayFx("Tree");
        //     }
        // }
    }
}
