using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElfPosition : MonoBehaviour
{
    public Vector3 spawnPoint;

    [SerializeField]
    private float maxDistance; // default is 3.5f
    public Vector2Int currentPos;
    public bool inDanger;

    private bool triggerDia;
    public GameObject helpIcon;
    private GameObject helpIconChild;


    // Update is called once per frame
    private void Update()
    {
        if (inDanger)
        {
            // Stop moving
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (FindObjectOfType<PlayerPosition>().transform.position.x - transform.position.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (CheckPlayer())
            {
                helpIconChild.SetActive(true);
            }
            else
            {
                helpIconChild.SetActive(false);
            }

            if (CheckClosePlayer() && !FindObjectOfType<DialogueController>().isActive && !triggerDia)
            {
                triggerDia = true;
                FindObjectOfType<DialogueController>().StartDialogue("Helpless Elf");
                FindObjectOfType<DialogueController>().AddPrompt(new Dialogue("Please help me!"));
                FindObjectOfType<DialogueController>().DisplayNextSentence();
            }
            if (!CheckClosePlayer() && triggerDia)
            {
                triggerDia = false;
                FindObjectOfType<DialogueController>().EndDialogue();
            }
        }

        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);
        TempData.tempElfPos = new Vector3(transform.position.x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inDanger && collision.gameObject.name.Contains("Player"))
        {
            SaveElf();
            FindObjectOfType<BattleManager>().Initiate(collision.gameObject, GameObject.FindGameObjectWithTag("SpecialEnemy"));
        }
    }

    public void InDanger()
    {
        Vector3 iconPos = new Vector3(transform.position.x, transform.position.y+1.5f);
        helpIconChild = Instantiate(helpIcon, iconPos, Quaternion.identity, transform);
        GetComponent<Animator>().SetBool("Jump", true);
        GetComponent<PartyMovement>().enabled = false;
        inDanger = true;
    }

    public void SaveElf()
    {
        Debug.Log("saved");
        // TempData.elfSaved = true;
        // newIcon.SetActive(false);
        // TempData.initElf = false;
        inDanger = false;
        Destroy(helpIconChild);
        GetComponent<Animator>().SetBool("Jump", false);
        GetComponent<PartyMovement>().minDistance = 0.8f;
        GetComponent<PartyMovement>().enabled = true;
        // DontDestroyOnLoad(gameObject);
    }

    public IEnumerator SaveElf2()
    {
        FindObjectOfType<DialogueController>().StartDialogue("Helpless Elf");
        FindObjectOfType<DialogueController>().AddPrompt(new Dialogue("Thanks!"));
        FindObjectOfType<DialogueController>().DisplayNextSentence();
        yield return new WaitForSeconds(1f);
        FindObjectOfType<DialogueController>().EndDialogue();
        GetComponent<PartyMovement>().minDistance = 1.55f;
        TempData.elfSaved = true;
    }

    private bool CheckPlayer()
    {
        // Calculate current distance from player
        float distance = Vector3.Distance(FindObjectOfType<PlayerPosition>().transform.position, transform.position);

        if (distance < maxDistance)
        {
            return true;
        }

        return false;
    }

    private bool CheckClosePlayer()
    {
        // Calculate current distance from player
        float distance = Vector3.Distance(FindObjectOfType<PlayerPosition>().transform.position, transform.position);

        if (distance < maxDistance-1.5)
        {
            return true;
        }

        return false;
    }
}
