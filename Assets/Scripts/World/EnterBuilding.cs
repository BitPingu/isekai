using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    [SerializeField]
    private float maxDistance; // default is 3.5f

    public GameObject enterIcon;
    private GameObject enterIconChild;

    public Vector3 activePos;

    private void Awake()
    {
        Vector3 iconPos = new Vector3(transform.position.x, transform.position.y+1.5f);
        enterIconChild = Instantiate(enterIcon, iconPos, Quaternion.identity, transform);
        enterIconChild.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            activePos = transform.position;
            // Player enters
            if (collision.gameObject.GetComponent<PlayerPosition>().currentArea.Contains("Overworld"))
                collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Overworld Dungeon Entrance";
            else if (collision.gameObject.GetComponent<PlayerPosition>().currentArea.Contains("Dungeon Underground"))
                collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Underground Dungeon Entrance";
            enterIconChild.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            activePos = Vector3.zero;
            // Player exits
            if (collision.gameObject.GetComponent<PlayerPosition>().currentArea.Contains("Overworld Dungeon Entrance"))
                collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Overworld";
             else if (collision.gameObject.GetComponent<PlayerPosition>().currentArea.Contains("Underground Dungeon Entrance"))
                collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Dungeon Underground";
            enterIconChild.SetActive(false);
        }
    }
}
