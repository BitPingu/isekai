using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public BattleState state;
    public GameObject player, enemy;
    public GameObject HUD;
    private GameObject playerHUD, enemyHUD;
    private BattleData playerData, enemyData;

    public void Initiate(GameObject p, GameObject e)
    {
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
        
        playerHUD = Instantiate(HUD, new Vector2(player.transform.position.x-1.5f, player.transform.position.y+0.3f), Quaternion.identity);
        playerHUD.transform.parent = player.transform;
        playerHUD.GetComponent<BattleHUD>().SetHUD(playerData);

        enemyHUD = Instantiate(HUD, new Vector2(enemy.transform.position.x-1.5f, enemy.transform.position.y), Quaternion.identity);
        enemyHUD.transform.parent = enemy.transform;
        enemyHUD.GetComponent<BattleHUD>().SetHUD(enemyData);

        FindObjectOfType<DialogueController>().StartDialogue("battlesystem");
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue("A wild " + enemyData.name + " approaches!", new string[2]{"Attack", "Run"}));
        FindObjectOfType<DialogueController>().DisplayNextSentence();

        state = BattleState.PLAYERTURN;
    }

    private void PlayerTurn()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + "'s Turn.", new string[2]{"Attack", "Run"}));
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        PlayerAttack();
    }

    private void PlayerAttack()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " attacks!"));

        bool isDead = enemyData.TakeDamage(playerData.damage);

        enemyHUD.GetComponent<BattleHUD>().SetHP(enemyData.currentHP);
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " took " + playerData.damage + " damage."));

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

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (enemy.tag.Equals("SpecialEnemy"))
        {
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue("Couldn't get away!"));
            PlayerTurn();
            return;
        }
        
        PlayerRun();
    }

    private void PlayerRun()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " ran."));
        EndBattle();
    }

    private void EnemyTurn()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " attacks!"));
        
        bool isDead = playerData.TakeDamage(enemyData.damage);

        playerHUD.GetComponent<BattleHUD>().SetHP(playerData.currentHP);
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " took " + enemyData.damage + " damage."));

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
        Destroy(playerHUD);
        Destroy(enemyHUD);

        if (state == BattleState.WON)
        {
            Destroy(enemy);
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " was defeated!"));
        }
        else if (state == BattleState.LOST)
        {
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " was defeated!"));
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
            dayNight.DayMusic();
        }
        else
        {
            dayNight.NightMusic();
        }

        if (enemy.tag.Equals("SpecialEnemy"))
        {
            FindObjectOfType<ElfPosition>().SaveElf2();
        }
        // Back to overworld
    }
}
