using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public static bool loadGame;

    public void Awake()
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
        Debug.Log("QUIT");
        Application.Quit();
    }
}
