using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] bgSounds, soundFx;
    public Sound bg, sf;
    public static AudioManager audioManager;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
        } 
        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Add sound components to bgSounds
        AddComponents(bgSounds);

        // Add sound components to soundFx
        AddComponents(soundFx);
    }

    private void AddComponents(Sound[] sounds)
    {
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
            s.source.outputAudioMixerGroup = s.group; // Link to audio mixer
        }
    }

    public void PlayFx (string name)
    {
        // Find sound fx to play
        sf = Array.Find(soundFx, sound => sound.name == name);

        // Sound fx not found
        if (sf == null)
        {
            Debug.LogWarning("Sound fx: " + name + " not found");
            return;
        }

        // Play sound fx
        sf.volume = sf.maxVolume;
        sf.source.Play();
    }

    public void Play (string name)
    {
        // Find bg sound to play
        bg = Array.Find(bgSounds, sound => sound.name == name);

        // Bg sound not found
        if (bg == null)
        {
            Debug.LogWarning("Bg sound: " + name + " not found");
            return;
        }

        // Play bg sound
        if (!bg.source.isPlaying)
        {
            bg.volume = bg.maxVolume;
            bg.source.Play();
        }
        else
        {
            Debug.Log("There is already music playing");
        }
    }

    public void Stop()
    {
        // Stop sound
        if (bg != null && bg.source.isPlaying)
        {
            bg.source.Stop();
        }
    }

    public void FadeIn(string name, float FadeTime)
    {
        // Find sound to play
        bg = Array.Find(bgSounds, sound => sound.name == name);

        // Sound not found
        if (bg == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        // Fade in chosen sound
        if (!bg.source.isPlaying)
        {
            StartCoroutine(AudioFade.FadeIn(bg, FadeTime));
        }
        else
        {
            Debug.Log("There is already music playing");
        }
    }

    public void FadeOut(float FadeTime)
    {
        // Fade out current sound
        if (bg != null && bg.source.isPlaying)
            StartCoroutine(AudioFade.FadeOut(bg, FadeTime));
    }

    public void Dampen()
    {
        // Lower sound volume
        if (bg != null && bg.source.isPlaying)
            bg.source.volume *= .5f;
    }

    public void UnDampen()
    {
        // Increase sound volume
        if (bg != null && bg.source.isPlaying)
            bg.source.volume *= 2f;
    }
}
