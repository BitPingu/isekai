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
        FindObjectOfType<DialogueController>().DisplayNextSentence();
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " attacks!"));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);

        bool isDead = enemyData.TakeDamage(playerData.damage);

        enemyHUD.GetComponent<BattleHUD>().SetHP(enemyData.currentHP);
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " took " + playerData.damage + " damage."));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            yield break;

        if (enemy.tag.Equals("SpecialEnemy"))
        {
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue("Couldn't get away!"));
            FindObjectOfType<DialogueController>().DisplayNextSentence();
            yield return new WaitForSeconds(1f);
            PlayerTurn();
            yield break;
        }
        
        StartCoroutine(PlayerRun());
    }

    private IEnumerator PlayerRun()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " ran."));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);
        StartCoroutine(EndBattle());
    }

    private IEnumerator EnemyTurn()
    {
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " attacks!"));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);
        
        bool isDead = playerData.TakeDamage(enemyData.damage);

        playerHUD.GetComponent<BattleHUD>().SetHP(playerData.currentHP);
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " took " + enemyData.damage + " damage."));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    public IEnumerator EndBattle()
    {
        bool special = false;
        if (enemy.tag.Equals("SpecialEnemy"))
            special = true;

        if (state == BattleState.WON)
        {
            Destroy(enemy);
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(enemyData.name + " was defeated!"));
            FindObjectOfType<DialogueController>().DisplayNextSentence();
            yield return new WaitForSeconds(1f);
        }
        else if (state == BattleState.LOST)
        {
            FindObjectOfType<DialogueController>().AddPrompt(new Dialogue(playerData.name + " was defeated!"));
            FindObjectOfType<DialogueController>().DisplayNextSentence();
            yield return new WaitForSeconds(1f);
        }
        FindObjectOfType<DialogueController>().EndDialogue();

        Destroy(playerHUD);
        Destroy(enemyHUD);

        // DeInit battle stance
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerBattle>().enabled = false;
        if (enemy)
        {
            enemy.GetComponent<NPCMovement>().enabled = true;
            enemy.GetComponent<EnemyBattle>().enabled = false;
        }

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

        if (special)
            StartCoroutine(FindObjectOfType<ElfPosition>().SaveElf2());

        // Back to overworld
    }
}
