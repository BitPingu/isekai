using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction dialogue, map;

    public void OnEnable()
    {
        dialogue.Enable();
        map.Enable();
    }

    public void OnDisable()
    {
        dialogue.Disable();
        map.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<DialogueManager>().box.activeSelf)
        {
            dialogue.Enable();
        } else
        {
            dialogue.Disable();
        }
    }
}
