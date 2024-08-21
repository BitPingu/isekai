using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampData : MonoBehaviour
{
    public List<GameObject> goblins = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Spawn goblins
            foreach (GameObject goblin in goblins)
                goblin.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Despawn goblins
            if (FindObjectOfType<DayAndNightCycle>().isDay)
                foreach (GameObject goblin in goblins)
                    goblin.gameObject.SetActive(false);
        }
    }

}
