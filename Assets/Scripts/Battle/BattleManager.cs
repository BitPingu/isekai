using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject player, enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initiate(GameObject p, GameObject e)
    {
        Debug.Log("battle!");
        player = p;
        enemy = e;

        // Init battle stance
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerBattle>().enabled = true;
        player.GetComponent<PlayerBattle>().Stance();

        enemy.GetComponent<NPCMovement>().enabled = false;
        enemy.GetComponent<EnemyBattle>().enabled = true;
        enemy.GetComponent<EnemyBattle>().Stance();
    }
}
