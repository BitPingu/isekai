using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private OptionsMenu options;

    private void Awake()
    {
        // Destroy leftover game objects
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Menu"));

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
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");
        // Change to Game Scene from queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame ()
    {
        FindObjectOfType<AudioManager>().Stop();
        // Set temp vars
        TempData.newGame = true;
        init(true);
    }

    public void LoadGame ()
    {
        FindObjectOfType<AudioManager>().Stop();
        // Set temp vars
        TempData.newGame = false;
        init(true);
        // Load save data
        SaveSystem.Load();
    }

    public void QuitGame ()
    {
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");
        
        Application.Quit();
    }

    private void init (bool cond)
    {
        TempData.initSpawn = cond;
        TempData.initSeed = cond;
        TempData.initTime = cond;
        TempData.initFog = cond;
        TempData.initBuilding = cond;
    }
}
