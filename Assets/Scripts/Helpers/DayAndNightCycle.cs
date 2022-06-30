using System.Collections;
using System.Collections.Generic;
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

    public float time;
    private bool canChangeDay = true;
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
            }
            else
            {
                NightTime();
            }
        }
    }

    public void Update()
    {
        // Reset time for new day
        if (time > timePerDay)
            time = 0;

        // Day time
        if (time == 0)
        {
            DayTime(); // Call delegate (and any methods tied to it)
            isDay = true;
            Debug.Log("from cycle: " + isDay);
        }

        // Prevent day ticker from increasing multiple times
        if ((int)time == ((timePerDay / 2) + 5) && canChangeDay)
        {
            // Night time
            canChangeDay = false;
            NightTime(); // Call delegate (and any methods tied to it)
            isDay = false;
            days++;
        }

        // Change day in middle of cycle
        if ((int)time == (timePerDay / 2))
            canChangeDay = true;

        // Tie time to frame rate
        time += Time.deltaTime;

        // Pick color from gradient based on value from 0-1
        light.GetComponent<Light2D>().color = lightColor.Evaluate(time * ratePerDay);
    }
}
