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

    public int timePerDay;
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
        if (MainMenu.loadGame)
        {
            // Load world data
            time = SaveSystem.LoadWorld().savedTime;
            isDay = SaveSystem.LoadWorld().savedIsDay;

            // Call delegates
            if (isDay)
            {
                DayTime();
            }
            else
            {
                NightTime();
            }
        }
        else
        {
            // New game
            DayTime(); // Call delegate (and any methods tied to it)
            isDay = true;
        }
    }

    private void Update()
    {
        // Reset time for new day
        if (time > timePerDay)
            time = 0;

        // Stop night music (fade)
        if ((int)time == timePerDay - 5)
            FindObjectOfType<AudioManager>().FadeOut(700f);

        // Day time
        if (time == 0)
        {
            DayTime(); // Call delegate (and any methods tied to it)
            isDay = true;
        }

        // Prevent day ticker from increasing multiple times
        if ((int)time == ((timePerDay / 2) + 5) && canChangeDay)
        {
            // Night time
            NightTime(); // Call delegate (and any methods tied to it)
            isDay = false;
            canChangeDay = false;
            days++;
        }

        // Enable day change
        if ((int)time == (timePerDay / 2))
        {
            canChangeDay = true;

            // Stop day music (fade)
            FindObjectOfType<AudioManager>().FadeOut(700f);
        }
        
        // Tie time to frame rate
        time += Time.deltaTime;

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
        if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<AudioManager>().FadeIn("Overworld Day", 1f);
        }
        else if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<AudioManager>().FadeIn("Village Day", 1f);
        }
        else if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
        }
    }

    private void NightMusic()
    {
        // Get current tile from player position
        if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<AudioManager>().FadeIn("Overworld Night", 1f);
        }
        else if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<AudioManager>().FadeIn("Village Night", 1f);
        }
        else if (player.currentGTile == (int)GroundTileType.Land && SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindObjectOfType<AudioManager>().FadeIn("Dungeon", 1f);
        }
    }
}
