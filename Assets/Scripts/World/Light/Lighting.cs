using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    [SerializeField]
    private Gradient lightColor;
    private DayAndNightCycle dayNight;

    private void Awake()
    {
        // Find day night cycle
        dayNight = FindObjectOfType<DayAndNightCycle>();
    }

    private void Update()
    {
        if (dayNight.init)
        {
            // Pick color from gradient based on value from 0-1
            GetComponent<Light2D>().color = lightColor.Evaluate(dayNight.time * dayNight.ratePerDay);  
        }
    }
}
