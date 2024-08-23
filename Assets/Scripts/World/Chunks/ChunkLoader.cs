using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public bool containsPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player") && collision.GetComponent<PlayerPosition>().currentArea.Contains("Overworld"))
        {
            // Player enters
            containsPlayer = true;
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
        else if (collision.gameObject.name.Contains("Player") && collision.GetComponent<PlayerPosition>().currentArea.Contains("Underground"))
        {
            // Player enters when underground
            containsPlayer = true;
            foreach (Transform child in transform)
                if (child.gameObject.name.Contains("Dungeon"))
                    child.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player") && collision.GetComponent<PlayerPosition>().currentArea.Contains("Overworld"))
        {
            containsPlayer = false;
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
        else if (collision.gameObject.name.Contains("Player") && collision.GetComponent<PlayerPosition>().currentArea.Contains("Underground"))
        {
            containsPlayer = false;
            foreach (Transform child in transform)
                if (child.gameObject.name.Contains("Dungeon"))
                    child.gameObject.SetActive(false);
        }
    }
}
