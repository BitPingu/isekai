using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer bgmAudioMixer;
    public AudioMixer sfxAudioMixer;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;
    public Resolution[] resolutions;

    // Player prefs 
    [HideInInspector]
    public string bgmVolume = "bgmVolume";
    [HideInInspector]
    public string sfxVolume = "sfxVolume";
    [HideInInspector]
    public string quality = "quality";
    [HideInInspector]
    public string resolution = "resolution";

    private void OnEnable()
    {
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");
    }

    public void SaveSettings ()
    {
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");

        // Save player prefs
        PlayerPrefs.SetFloat(bgmVolume, bgmVolumeSlider.value);
        PlayerPrefs.SetFloat(sfxVolume, sfxVolumeSlider.value);
        PlayerPrefs.SetInt(quality, qualityDropdown.value);
        PlayerPrefs.SetInt(resolution, resolutionDropdown.value);
        PlayerPrefs.Save();
    }

    public void ResetSettings ()
    {
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");
        
        // Reset player prefs
        SetDefaultVolume();
        SetDefaultQuality();
        SetDefaultResolution();
        PlayerPrefs.DeleteAll();
    }

    public void SetDefaultVolume ()
    {
        // Adjust bgm volume slider
        bgmVolumeSlider.value = -4;
        bgmAudioMixer.SetFloat("bgmVolume", bgmVolumeSlider.value);

        // Adjust sfx volume slider
        sfxVolumeSlider.value = -4;
        sfxAudioMixer.SetFloat("sfxVolume", bgmVolumeSlider.value);
    }

    public void SetBgmVolume (float volume)
    {
        bgmVolumeSlider.value = volume;
        bgmAudioMixer.SetFloat("bgmVolume", volume);
    }

    public void SetSfxVolume (float volume)
    {
        sfxVolumeSlider.value = volume;
        sfxAudioMixer.SetFloat("sfxVolume", volume);
    }

    public void SetDefaultQuality ()
    {
        // Set default graphics from dropdown
        qualityDropdown.value = 1;
        qualityDropdown.RefreshShownValue();
    }

    public void SetQuality (int qualityIndex)
    {
        qualityDropdown.value = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetDefaultResolution ()
    {
        // Get available screen resolutions
        resolutions = Screen.resolutions;

        // Reset default options in dropdown
        resolutionDropdown.ClearOptions();

        // Create list of options
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Add resolution options to list
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Select current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to dropdown
        resolutionDropdown.AddOptions(options);

        // Set current resolution from dropdown
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetSavedResolution (int resolutionIndex)
    {
        // Get available screen resolutions
        resolutions = Screen.resolutions;

        // Reset default options in dropdown
        resolutionDropdown.ClearOptions();

        // Create list of options
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Add resolution options to list
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Select current resolution
            if (i == resolutionIndex)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to dropdown
        resolutionDropdown.AddOptions(options);

        // Set current resolution from dropdown
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        // Select resolution
        Resolution resolution = resolutions[resolutionIndex];

        // Set resolution
        resolutionDropdown.value = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
