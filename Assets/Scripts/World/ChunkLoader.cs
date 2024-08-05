using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public bool containsPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Player enters
            containsPlayer = true;
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            containsPlayer = false;
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }
}
