using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField]
    private Gradient lightColor;

    public int timePerDay; // default is 500
    public float ratePerDay; // default is 0.002

    public int days;
    public float time; // Default start time is 50
    private bool canChangeDay;
    public bool isDay;

    // Call other functions when day changes
    public delegate void OnDayChanged();
    public OnDayChanged DayTime, NightTime;

    private void OnEnable()
    {
        // Attach delegates
        // DayTime += DayMusic;
        // NightTime += NightMusic;
    }

    private void OnDisable()
    {
        // Detatch delegates
        // DayTime -= DayMusic;
        // NightTime -= NightMusic;
    }

    private void Start()
    {
        // if (TempData.initTime)
        // {
        //     if (TempData.newGame)
        //     {
        //         // New game
        //         days = 0;
        //         time = 50;
        //         isDay = true;
        //     }
        //     else
        //     {
        //         // Load world data
        //         SaveData saveData = SaveSystem.Load();
        //         days = saveData.saveDays;
        //         time = saveData.saveTime;
        //         isDay = saveData.saveIsDay;
        //     }
        //     TempData.tempDays = days;
        //     TempData.tempTime = time;
        //     TempData.tempIsDay = isDay;
        //     TempData.initTime = false;
        // }
        // else
        // {
        //     time = TempData.tempTime;
        //     isDay = TempData.tempIsDay;
        // }

        // // Call delegates (and any methods tied to it)
        // if (isDay)
        // {
        //     DayTime();
        // }
        // else
        // {
        //     NightTime();
        // }
    }

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
    }

    private void Update()
    {
        // Reset time for new day
        if (time > timePerDay)
            time = 0;

        // Stop night music (fade)
        if ((int)time == timePerDay - 10)
            FindObjectOfType<AudioManager>().FadeOut(800f);

        // Day time
        if (time == 0)
        {
            Debug.Log("it's a new day!");
            FindObjectOfType<AudioManager>().Stop();
            DayTime(); // Call delegate (and any methods tied to it)
            isDay = true;
            TempData.tempIsDay = isDay;
            days++;
        }

        // Stop day music (fade)
        if ((int)time == (timePerDay / 2) - 10)
            FindObjectOfType<AudioManager>().FadeOut(800f);

        // Prevent multiple day changes
        if ((int)time == (timePerDay / 2) - 1)
            canChangeDay = true;

        // Night time
        if ((int)time == (timePerDay / 2) && canChangeDay)
        {
            Debug.Log("night");
            FindObjectOfType<AudioManager>().Stop();
            NightTime(); // Call delegate (and any methods tied to it)
            isDay = false;
            TempData.tempIsDay = isDay;
            canChangeDay = false;
        }
        
        // Tie time to frame rate
        time += Time.deltaTime;
        TempData.tempTime = time;

        // Pick color from gradient based on value from 0-1
        GetComponent<Light2D>().color = lightColor.Evaluate(time * ratePerDay);   
    }

    public void DayMusic()
    {
        // Get current tile from player position
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<AudioManager>().FadeIn("Overworld Day", 1f);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<AudioManager>().FadeIn("Village Day", 1f);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
        }
    }

    public void NightMusic()
    {
        // Get current tile from player position
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<AudioManager>().FadeIn("Overworld Night", 1f);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<AudioManager>().FadeIn("Village Night", 1f);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
        }
    }
}
