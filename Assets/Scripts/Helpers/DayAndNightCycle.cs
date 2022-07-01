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

    public void Awake()
    {
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

                // Play overworld day
                FindObjectOfType<AudioManager>().Play("Overworld Day");
            }
            else
            {
                NightTime();

                // Play overworld night
                FindObjectOfType<AudioManager>().Play("Overworld Night");
            }
        }
        else
        {
            // New game
            DayTime();
            isDay = true;

            // Play overworld day
            FindObjectOfType<AudioManager>().Play("Overworld Day");
        }
    }

    public void Update()
    {
        // Reset time for new day
        if (time > timePerDay)
            time = 0;

        // Stop overworld night (fade)
        if ((int)time == timePerDay - 5)
            FindObjectOfType<AudioManager>().FadeOut();

        // Day time
        if (time == 0)
        {
            DayTime(); // Call delegate (and any methods tied to it)
            isDay = true;

            // Play overworld day (fade)
            FindObjectOfType<AudioManager>().FadeIn("Overworld Day");
        }

        // Prevent day ticker from increasing multiple times
        if ((int)time == ((timePerDay / 2) + 5) && canChangeDay)
        {
            // Night time
            NightTime(); // Call delegate (and any methods tied to it)
            isDay = false;
            canChangeDay = false;
            days++;

            // Play overworld night (fade)
            FindObjectOfType<AudioManager>().FadeIn("Overworld Night");
        }

        // Enable day change
        if ((int)time == (timePerDay / 2))
        {
            canChangeDay = true;

            // Stop overworld day (fade)
            FindObjectOfType<AudioManager>().FadeOut();
        }
        
        // Tie time to frame rate
        time += Time.deltaTime;

        // Pick color from gradient based on value from 0-1
        light.GetComponent<Light2D>().color = lightColor.Evaluate(time * ratePerDay);
    }
}
