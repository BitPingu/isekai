using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private Sound s;

    public static AudioManager instance;

    private void Awake()
    {
        // Only one instance of audio manager
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        // Persist through scenes
        DontDestroyOnLoad(gameObject);

        // Loop through each sound
        foreach (Sound s in sounds)
        {
            // Add audio source component
            s.source = gameObject.AddComponent<AudioSource>();

            // Assign audio source attributes
            s.source.clip = s.clip;
            s.maxVolume = s.volume;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Update()
    {
        //Debug.Log(s.source.volume + ": " + s.source.isPlaying);
    }
    public void Play (string name)
    {
        // Find sound to play
        s = Array.Find(sounds, sound => sound.name == name);

        // Sound not found
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        // Play sound
        s.volume = s.maxVolume;
        s.source.Play();
    }

    public void Stop()
    {
        // Stop sound
        s.source.Stop();
    }

    public void FadeIn(string name, float FadeTime)
    {
        // Find sound to play
        s = Array.Find(sounds, sound => sound.name == name);

        // Sound not found
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        // Fade in chosen sound
        StartCoroutine(AudioFade.FadeIn(s, FadeTime));
    }

    public void FadeOut(float FadeTime)
    {
        // Fade out current sound
        StartCoroutine(AudioFade.FadeOut(s, FadeTime));
    }

    public void Dampen()
    {
        // Lower sound volume
        s.source.volume *= .5f;
    }

    public void UnDampen()
    {
        // Increase sound volume
        s.source.volume *= 2f;
    }
}
