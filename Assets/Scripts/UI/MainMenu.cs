using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public static bool loadGame;

    [SerializeField]
    private OptionsMenu options;

    private void Awake()
    {
        // Do not load save data
        loadGame = false;

        // Check if new game
        string path = Application.persistentDataPath + "/saveData.isekai";
        if (File.Exists(path))
        {
            // Show load game button
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("bgmVolume"))
        {
            // Apply saved settings
            options.SetBgmVolume(PlayerPrefs.GetFloat("bgmVolume"));
            options.SetSfxVolume(PlayerPrefs.GetFloat("sfxVolume"));
            options.SetQuality(PlayerPrefs.GetInt("quality"));
            options.SetSavedResolution(PlayerPrefs.GetInt("resolution"));
            
        }
        else
        {
            // Apply default settings
            options.ResetSettings();
        }

        // Play theme
        FindObjectOfType<AudioManager>().Play("Theme");
    }

    public void PlayGame ()
    {
        // Change to Game Scene from queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame ()
    {
        // Load save data
        SaveSystem.LoadAllData();
        loadGame = true;
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
