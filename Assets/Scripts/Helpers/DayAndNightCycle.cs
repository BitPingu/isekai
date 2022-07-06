using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
    private GameObject player;

    [SerializeField]
    private int currentTile;
    [SerializeField]
    private int savedTile;
    [SerializeField]
    private TileGrid grid;
    private TilemapStructure groundMap;

    private void Awake()
    {
        // Retrieve tilemap component
        groundMap = grid.GetTilemap(TilemapType.Ground);

        if (MainMenu.loadGame)
        {
            // Load world data
            WorldData data = SaveSystem.LoadWorld();
            time = data.time;
            isDay = data.isDay;

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
            DayTime();
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

            // Play day music (fade)
            DayMusic();
        }

        // Prevent day ticker from increasing multiple times
        if ((int)time == ((timePerDay / 2) + 5) && canChangeDay)
        {
            // Night time
            NightTime(); // Call delegate (and any methods tied to it)
            isDay = false;
            canChangeDay = false;
            days++;

            // Play night music (fade)
            NightMusic();
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

        // Get current tile from player position
        currentTile = groundMap.GetTile((int)player.transform.position.x, (int)player.transform.position.y);
        if (currentTile == (int)GroundTileType.Land && savedTile != (int)GroundTileType.Land)
        {
            // Play overworld music
            savedTile = (int)GroundTileType.Land;
            FindObjectOfType<AudioManager>().Stop();
            //FindObjectOfType<AudioManager>().FadeOut(1f);
            if (time >= 0 && time < ((timePerDay / 2) + 5))
            {
                // Day overworld music
                DayMusic();
            }
            else
            {
                // Night overworld music
                NightMusic();
            }
        }
        if (currentTile == (int)GroundTileType.Village && savedTile != (int)GroundTileType.Village)
        {
            // Play village music
            savedTile = (int)GroundTileType.Village;
            FindObjectOfType<AudioManager>().Stop();
            //FindObjectOfType<AudioManager>().FadeOut(1f);
            if (time >= 0 && time < ((timePerDay / 2) + 5))
            {
                // Day village music
                DayMusic();
            }
            else
            {
                // Night village music
                NightMusic();
            }
        }
    }

    private void DayMusic()
    {
        // Get current tile from player position
        currentTile = groundMap.GetTile((int)player.transform.position.x, (int)player.transform.position.y);
        switch (currentTile)
        {
            case (int)GroundTileType.Land:
                FindObjectOfType<AudioManager>().FadeIn("Overworld Day", 1f);
                break;
            case (int)GroundTileType.Village:
                FindObjectOfType<AudioManager>().FadeIn("Village Day", 1f);
                break;
        }
    }

    private void NightMusic()
    {
        // Get current tile from player position
        currentTile = groundMap.GetTile((int)player.transform.position.x, (int)player.transform.position.y);
        switch (currentTile)
        {
            case (int)GroundTileType.Land:
                FindObjectOfType<AudioManager>().FadeIn("Overworld Night", 1f);
                break;
            case (int)GroundTileType.Village:
                FindObjectOfType<AudioManager>().FadeIn("Village Night", 1f);
                break;
        }
    }
}
