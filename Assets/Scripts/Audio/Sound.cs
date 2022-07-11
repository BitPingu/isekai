using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [HideInInspector]
    public float maxVolume;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}
