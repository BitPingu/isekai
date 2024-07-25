using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public BattleState state;
    public GameObject player, enemy;
    public Dialogue dialogue;
    public GameObject HUD;
    private GameObject playerHUD, enemyHUD;
    private BattleData playerData, enemyData;

    public void Initiate(GameObject p, GameObject e)
    {
        Debug.Log("battle!");
        state = BattleState.START;

        player = p;
        enemy = e;

        playerData = player.GetComponent<BattleData>();
        enemyData = enemy.GetComponent<BattleData>();

        // Init battle stance
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerBattle>().enabled = true;
        player.GetComponent<PlayerBattle>().Stance();

        enemy.GetComponent<NPCMovement>().enabled = false;
        enemy.GetComponent<EnemyBattle>().enabled = true;
        enemy.GetComponent<EnemyBattle>().Stance();

        // Start battle
        FindObjectOfType<Camera>().target = enemy;
        FindObjectOfType<AudioManager>().Stop();
        FindObjectOfType<AudioManager>().Play("Battle");
        
        // dialogue.prompts
        playerHUD = Instantiate(HUD, new Vector2(player.transform.position.x-1.5f, player.transform.position.y+0.3f), Quaternion.identity);
        playerHUD.transform.parent = player.transform;
        playerHUD.GetComponent<BattleHUD>().SetHUD(playerData);

        enemyHUD = Instantiate(HUD, new Vector2(enemy.transform.position.x-1.5f, enemy.transform.position.y), Quaternion.identity);
        enemyHUD.transform.parent = enemy.transform;
        enemyHUD.GetComponent<BattleHUD>().SetHUD(enemyData);

        FindObjectOfType<DialogueController>().StartDialogue(dialogue);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        Debug.Log("player turn");
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        PlayerAttack();
    }

    private void PlayerAttack()
    {
        bool isDead = enemyData.TakeDamage(playerData.damage);

        enemyHUD.GetComponent<BattleHUD>().SetHP(enemyData.currentHP);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    private void EnemyTurn()
    {
        Debug.Log("enemy turn");
        bool isDead = playerData.TakeDamage(enemyData.damage);

        playerHUD.GetComponent<BattleHUD>().SetHP(playerData.currentHP);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("won");
        }
        else if (state == BattleState.LOST)
        {
            Debug.Log("lost");
        }

        // DeInit battle stance
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerBattle>().enabled = false;
        enemy.GetComponent<NPCMovement>().enabled = true;
        enemy.GetComponent<EnemyBattle>().enabled = false;

        // DeStart battle
        FindObjectOfType<Camera>().target = player;
        FindObjectOfType<AudioManager>().Stop();
        DayAndNightCycle dayNight = FindObjectOfType<DayAndNightCycle>();
        if (dayNight.isDay)
        {
            dayNight.DayTime();
        }
        else
        {
            dayNight.NightTime();
        }

        // Back to overworld
    }
}
