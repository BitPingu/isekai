using System.Collections;
using UnityEngine;

public static class AudioFade
{
    public static IEnumerator FadeIn(Sound s, float FadeTime)
    {
        float startVolume = 0.2f;

        s.source.volume = 0;
        s.source.Play();
        while (s.source.volume < s.maxVolume)
        {
            s.source.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        s.source.volume = s.maxVolume;
    }

    public static IEnumerator FadeOut(Sound s, float FadeTime)
    {
        float startVolume = s.maxVolume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        s.source.volume = 0f;
    }
}
