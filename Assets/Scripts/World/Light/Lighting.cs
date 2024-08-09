using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    [SerializeField]
    private Gradient lightColor;
    private DayAndNightCycle dayNight;

    [SerializeField]
    private float dayLum, nightLum; // default is 1f, .4f for overworld

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
            if (TempData.tempIsDay)
                GetComponent<Light2D>().intensity = dayLum;
            else
                GetComponent<Light2D>().intensity = nightLum;
        }

        if ((int)dayNight.time == dayNight.timePerDay - 10)
        {
            StartCoroutine(Fade(nightLum, dayLum));
        }

        if ((int)dayNight.time == (dayNight.timePerDay / 2) - 10)
        {
            StartCoroutine(Fade(dayLum, nightLum));
        }
    }

    IEnumerator Fade(float minLum, float maxLum)
    {
        float counter = 0f;

        while (counter < 10)
        {
            counter += Time.deltaTime;
            GetComponent<Light2D>().intensity = Mathf.Lerp(minLum, maxLum, counter / 10);
            yield return null;
        }
    }
}
