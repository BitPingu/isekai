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

    public Queue<Dialogue.Prompt> prompts;
    public Dialogue.Prompt currentPrompt;

    public string issuer;
    public bool isActive;

    // Start is called before the first frame update
    private void Start()
    {
        prompts = new Queue<Dialogue.Prompt>();
        isActive = false;
    }

    private void Update()
    {
        if (prompts.Count >= 0 && Input.GetKeyDown(dialogKey))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isActive = true;

        issuer = dialogue.name;

        prompts.Clear();

        foreach (Dialogue.Prompt prompt in dialogue.prompts)
        {
            prompts.Enqueue(prompt);
        }

        DisplayNextSentence();
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
        Debug.Log("do " + act);
        switch(act)
        {
            case "Yes":
                break;
            case "No":
                DisplayNextSentence();
                break;
            case "Attack":
                FindObjectOfType<BattleManager>().OnAttackButton();
                break;
            case "Run":
                FindObjectOfType<BattleManager>().EndBattle();
                break;
            default:
                break;
        }
    }

    public void Action1()
    {
        string option = currentPrompt.options[0];
        Debug.Log("action1");
        Action(option);
    }

    public void Action2()
    {
        string option = currentPrompt.options[1];
        Debug.Log("action2");
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
