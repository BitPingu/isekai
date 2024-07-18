using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField]
    private Gradient lightColor;

    [SerializeField]
    private GameObject light;

    [SerializeField]
    private int days;

    public int timePerDay; // default is 500
    public float ratePerDay;

    // Getter method
    public int Days => days;

    public float time; // Default start time is 50
    private bool canChangeDay;
    public bool isDay;

    // Call other functions when day changes
    public delegate void OnDayChanged();
    public OnDayChanged DayTime;
    public OnDayChanged NightTime;

    [SerializeField]
    private PlayerPosition player;

    private void Awake()
    {
        // Attach delegates
        DayTime += DayMusic;
        NightTime += NightMusic;
        player.GTileChange += ChangeBgMusic;
    }

    private void Start()
    {
        if (TempData.initTime)
        {
            if (TempData.newGame)
            {
                Debug.Log("new time");
                // New game
                time = 50;
                isDay = true;
            }
            else
            {
                Debug.Log("load time");
                // Load world data
                SaveData saveData = SaveSystem.Load();
                time = saveData.saveTime;
                isDay = saveData.saveIsDay;
            }
            TempData.tempTime = time;
            TempData.tempIsDay = isDay;
            TempData.initTime = false;
        }
        else
        {
            Debug.Log("state time");
            time = TempData.tempTime;
            isDay = TempData.tempIsDay;
        }

        // Call delegates (and any methods tied to it)
        if (isDay)
        {
            Debug.Log("load day");
            DayTime();
        }
        else
        {
            Debug.Log("load night");
            NightTime();
        }
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
        light.GetComponent<Light2D>().color = lightColor.Evaluate(time * ratePerDay);
    }

    private void ChangeBgMusic()
    {
        // Prevent music change when near water
        if (player.prevGTile == (int)GroundTileType.Water || player.currentGTile == (int)GroundTileType.Water)
            return;

        // Stop current music
        FindObjectOfType<AudioManager>().Stop();

        // Play day/night music
        if (isDay)
        {
            DayMusic();
        }
        else
        {
            NightMusic();
        }
    }

    private void DayMusic()
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

    private void NightMusic()
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
