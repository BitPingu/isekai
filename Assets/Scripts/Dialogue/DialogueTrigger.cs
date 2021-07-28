using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().dialogue.performed += _ => FindObjectOfType<DialogueManager>().DisplayNextSentance();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<DialogueManager>().box.SetActive(true);

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
