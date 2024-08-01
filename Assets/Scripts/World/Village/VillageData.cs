using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageData : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Player enters
            collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Village";
            StartCoroutine(EnterVillage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Player exits
            collision.gameObject.GetComponent<PlayerPosition>().currentArea = "Overworld";
            StartCoroutine(ExitVillage());
        }
    }

    private IEnumerator EnterVillage()
    {
        // Despawn enemies
        FindObjectOfType<EnemySpawner>().despawnEnemies();

        // village music
        FindObjectOfType<AudioManager>().FadeOut(2f);
        yield return new WaitForSeconds(3);
        FindObjectOfType<AudioManager>().Stop();

        if (FindObjectOfType<DayAndNightCycle>().isDay)
        {
            FindObjectOfType<DayAndNightCycle>().DayTime();
        }
        else
        {
            FindObjectOfType<DayAndNightCycle>().NightTime();
        }
    }

    private IEnumerator ExitVillage()
    {
        // back to overworld
        FindObjectOfType<AudioManager>().FadeOut(2f);
        yield return new WaitForSeconds(3);
        FindObjectOfType<AudioManager>().Stop();
        
        if (FindObjectOfType<DayAndNightCycle>().isDay)
        {
            FindObjectOfType<DayAndNightCycle>().DayTime();
        }
        else
        {
            FindObjectOfType<DayAndNightCycle>().NightTime();
        }
    }

}
