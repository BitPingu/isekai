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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            FindObjectOfType<BattleManager>().Initiate(collision.gameObject, gameObject);
        }
        if (collision.gameObject.name.Contains("Player") && gameObject.tag.Equals("SpecialEnemy"))
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
}
