using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageData : MonoBehaviour
{
    public List<Vector2> lots = new List<Vector2>();
    public List<GameObject> villagers = new List<GameObject>();
    public bool containsPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            containsPlayer = true;
            // Player enters
            collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Village";

            // Despawn enemies (or make enemies run away?)
            FindObjectOfType<EnemySpawner>().despawnEnemies();

            // Spawn villagers
            if (FindObjectOfType<DayAndNightCycle>().isDay)
                foreach (GameObject villager in villagers)
                    villager.gameObject.SetActive(true);

            StartCoroutine(EnterVillage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            containsPlayer = false;
            // Player exits
            collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Overworld";

            // Despawn villagers
            if (FindObjectOfType<DayAndNightCycle>().isDay)
                foreach (GameObject villager in villagers)
                    villager.gameObject.SetActive(false);

            // Spawn enemies
            StartCoroutine(FindObjectOfType<EnemySpawner>().Spawn());

            StartCoroutine(ExitVillage());
        }
    }

    private IEnumerator EnterVillage()
    {
        // village music
        if (FindObjectOfType<AudioManager>().bg.name.Contains("Overworld"))
        {
            FindObjectOfType<AudioManager>().FadeOut(1f);
            yield return new WaitForSeconds(2);
            FindObjectOfType<AudioManager>().Stop();

            if (FindObjectOfType<DayAndNightCycle>().isDay)
            {
                FindObjectOfType<DayAndNightCycle>().DayMusic();
            }
            else
            {
                FindObjectOfType<DayAndNightCycle>().NightMusic();
            }
        }
    }

    private IEnumerator ExitVillage()
    {
        // back to overworld
        if (FindObjectOfType<AudioManager>().bg.name.Contains("Village"))
        {
            FindObjectOfType<AudioManager>().FadeOut(1f);
            yield return new WaitForSeconds(2f);
            FindObjectOfType<AudioManager>().Stop();
            
            if (FindObjectOfType<DayAndNightCycle>().isDay)
            {
                FindObjectOfType<DayAndNightCycle>().DayMusic();
            }
            else
            {
                FindObjectOfType<DayAndNightCycle>().NightMusic();
            }
        }
    }

}
