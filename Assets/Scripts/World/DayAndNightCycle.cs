using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayAndNightCycle : MonoBehaviour
{
    public int timePerDay; // default is 500
    public float ratePerDay; // default is 0.002

    public int days;
    public float time; // Default start time is 50
    private bool canChangeDay;
    public bool isDay;

    // Call other functions when day changes
    public delegate void OnDayChanged();
    public OnDayChanged DayTime, NightTime;

    public bool init;

    public void Initialize()
    {
        if (TempData.loadGame)
        {
            // Load time
            days = SaveSystem.Load().saveDays;
            time = SaveSystem.Load().saveTime;
            isDay = SaveSystem.Load().saveIsDay;
        }
        else
        {
            // New game
            days = 0;
            time = 50;
            isDay = true;
        }

        // Save time
        TempData.tempDays = days;
        TempData.tempTime = time;
        TempData.tempIsDay = isDay;

        // Attach music delegates
        DayTime += DayMusic;
        NightTime += NightMusic;

        init = true;
    }

    private void Update()
    {
        if (init)
            Cycle(); 
    }

    private void Cycle()
    {
        // Reset time for new day
        if (time > timePerDay)
            time = 0;

        // Stop night music (fade)
        if ((int)time == timePerDay - 10 
            && (FindObjectOfType<AudioManager>().bg.name.Contains("Overworld") || FindObjectOfType<AudioManager>().bg.name.Contains("Village")))
            StartCoroutine(FadeMusic());

        // Day time
        if (time == 0)
        {
            Debug.Log("it's a new day!");
            isDay = true;
            TempData.tempIsDay = isDay;
            days++;
            DayTime(); // Call delegates and play day music
        }

        // Stop day music (fade)
        if ((int)time == (timePerDay / 2) - 10 && 
            (FindObjectOfType<AudioManager>().bg.name.Contains("Overworld") || FindObjectOfType<AudioManager>().bg.name.Contains("Village")))
            StartCoroutine(FadeMusic());

        // Prevent multiple day changes
        if ((int)time == (timePerDay / 2) - 1)
            canChangeDay = true;

        // Night time
        if ((int)time == (timePerDay / 2) && canChangeDay)
        {
            Debug.Log("night");
            isDay = false;
            TempData.tempIsDay = isDay;
            canChangeDay = false;
            NightTime(); // Call delegates and play night music
        }
        
        // Tie time to frame rate
        time += Time.deltaTime;
        TempData.tempTime = time;
    }

    private IEnumerator FadeMusic()
    {
        FindObjectOfType<AudioManager>().FadeOut(800f);
        yield return new WaitForSeconds(8);
        FindObjectOfType<AudioManager>().Stop();
    }

    public void DayMusic()
    {
        if (!FindObjectOfType<AudioManager>().bg.source || !FindObjectOfType<AudioManager>().bg.source.isPlaying)
        {
            switch(FindObjectOfType<PlayerPosition>().currentArea)
            {
                case "Overworld":
                    FindObjectOfType<AudioManager>().FadeIn("Overworld Day", 1f);
                    break;
                case "Village":
                    FindObjectOfType<AudioManager>().FadeIn("Village Day", 1f);
                    break;
                case "Dungeon":
                    FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
                    break;
                default:
                    Debug.Log("unknown area to play day music");
                    break;
            }
        }
        else
        {
            Debug.Log("cannot play day music");
        }
    }

    public void NightMusic()
    {
        if (!FindObjectOfType<AudioManager>().bg.source || !FindObjectOfType<AudioManager>().bg.source.isPlaying)
        {
            switch(FindObjectOfType<PlayerPosition>().currentArea)
            {
                case "Overworld":
                    FindObjectOfType<AudioManager>().FadeIn("Overworld Night", 1f);
                    break;
                case "Village":
                    FindObjectOfType<AudioManager>().FadeIn("Village Night", 1f);
                    break;
                case "Dungeon":
                    FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
                    break;
                default:
                    Debug.Log("unknown area to play day music");
                    break;
            }
        }
        else
        {
            Debug.Log("cannot play night music");
        }
    }
}
