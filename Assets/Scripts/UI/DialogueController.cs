using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private GameObject textDialogueUI;
    [SerializeField]
    private GameObject optionDialogueUI;
    [SerializeField]
    private KeyCode dialogKey;

    public Queue<Dialogue> prompts;
    public Dialogue currentPrompt;

    public string issuer;
    public bool isActive;

    // Start is called before the first frame update
    private void Start()
    {
        prompts = new Queue<Dialogue>();
        isActive = false;
    }

    private void Update()
    {
        // if (prompts.Count >= 0 && Input.GetKeyDown(dialogKey))
        // {
        //     DisplayNextSentence();
        // }
        if (isActive && Input.GetKeyDown(dialogKey) && currentPrompt.options.Length == 0)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string character)
    {
        isActive = true;
        issuer = character;
        // prompts.Clear();
    }

    public void AddPrompt(Dialogue newPrompt)
    {
        prompts.Enqueue(newPrompt);
    }

    public void DisplayNextSentence()
    {
        if (prompts.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentPrompt = prompts.Dequeue();

        if (currentPrompt.options.Length == 0)
        {
            textDialogueUI.SetActive(true);
            optionDialogueUI.SetActive(false);

            Transform[] transforms = textDialogueUI.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms)
            {
                if (t.name == "Name")
                {
                    t.gameObject.GetComponent<TextMeshProUGUI>().text = issuer;
                }
                else if (t.name == "Dialogue")
                {
                    t.gameObject.GetComponent<TextMeshProUGUI>().text = currentPrompt.sentence;
                }
            }
        }
        else
        {
            optionDialogueUI.SetActive(true);
            textDialogueUI.SetActive(false);

            Transform[] transforms = optionDialogueUI.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms)
            {
                if (t.name == "Dialogue")
                {
                    t.gameObject.GetComponent<TextMeshProUGUI>().text = currentPrompt.sentence;
                }
                else if (t.name == "Option1")
                {
                    t.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentPrompt.options[0];
                }
                else if (t.name == "Option2")
                {
                    t.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentPrompt.options[1];
                }
            }
        }
    }

    public void Action(string act)
    {
        switch(act)
        {
            case "Yes":
                // if (issuer.Equals("Helpless Elf"))
                //     FindObjectOfType<WorldEvents>().SaveElf();
                break;
            case "No":
                break;
            case "Attack":
                FindObjectOfType<BattleManager>().OnAttackButton();
                DisplayNextSentence();
                break;
            case "Run":
                FindObjectOfType<BattleManager>().OnRunButton();
                DisplayNextSentence();
                break;
            default:
                break;
        }
    }

    public void Action1()
    {
        string option = currentPrompt.options[0];
        Action(option);
    }

    public void Action2()
    {
        string option = currentPrompt.options[1];
        Action(option);
    }

    public void EndDialogue()
    {
        while (prompts.Count > 0)
            prompts.Dequeue();
        textDialogueUI.SetActive(false);
        optionDialogueUI.SetActive(false);
        isActive = false;
    }
}
