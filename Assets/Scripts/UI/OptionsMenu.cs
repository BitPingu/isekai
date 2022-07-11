using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown graphicsDropdown;

    [SerializeField]
    private Slider volumeSlider;

    private Resolution[] resolutions;

    private void Awake()
    {
        // Set default volume
        volumeSlider.value = -4;
    }

    private void Start()
    {
        // Get available screen resolutions
        resolutions = Screen.resolutions;

        // Reset default options in dropdown
        resolutionDropdown.ClearOptions();

        // Create list of options
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i=0; i<resolutions.Length; i++)
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

        // Set default graphics from dropdown
        graphicsDropdown.value = 2;
        graphicsDropdown.RefreshShownValue();
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        // Select resolution
        Resolution resolution = resolutions[resolutionIndex];

        // Set resolution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
