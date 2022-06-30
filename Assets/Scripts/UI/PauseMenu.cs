using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Keep track of if game is paused
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI, player, world, dayNight;

    // Update is called once per frame
    void Update()
    {
        // Esc to pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                // Resume game from pause 
                Resume();
            }
            else
            {
                // Pause game
                Pause();
            }
        }
    }

    public void Resume ()
    {
        // Resume time
        Time.timeScale = 1f;

        // Disable pause menu
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    void Pause ()
    {
        // Freeze time
        Time.timeScale = 0f;

        // Enable pause menu
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void Save()
    {
        // Save game
        Debug.Log("Saving game...");
        PlayerController playerData = player.GetComponent<PlayerController>();
        TileGrid worldData = world.GetComponent<TileGrid>();
        DayAndNightCycle dayNightData = dayNight.GetComponent<DayAndNightCycle>();
        SaveSystem.SaveAllData(playerData, worldData, dayNightData);
    }

    public void LoadMenu()
    {
        // Resume time
        Time.timeScale = 1f;

        // Return to main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
