using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private OptionsMenu options;
    public GameObject audio;

    private void Awake()
    {
        // Destroy leftover game objects
        // Destroy(GameObject.Find("Player"));
        // Destroy(GameObject.Find("Elf"));
        // Destroy(GameObject.Find("UI"));

        // Audio
        Instantiate(audio, Vector3.zero, Quaternion.identity);

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

        // Fade
        GetComponentInParent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(OnFadeComplete());
    }

    public IEnumerator OnFadeComplete()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().Stop();
        // Change to Game Scene from queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame ()
    {
        // Set temp vars
        TempData.loadGame = false;
        TempData.elfSaved = false;
        // init(true);
    }

    public void LoadGame ()
    {
        // Set temp vars
        TempData.loadGame = true;
        // init(true);
        // Load save data
        SaveSystem.Load();
    }

    public void QuitGame ()
    {
        // Play sound fx
        FindObjectOfType<AudioManager>().PlayFx("Button");
        
        Application.Quit();
    }

    // private void init (bool cond)
    // {
    //     TempData.initPlayerSpawn = cond;
    //     TempData.initElfSpawn = cond;
    //     TempData.initSeed = cond;
    //     TempData.initTime = cond;
    //     TempData.initFog = cond;
    //     TempData.initBuilding = cond;
    //     TempData.initElf = cond;
    // }
}
