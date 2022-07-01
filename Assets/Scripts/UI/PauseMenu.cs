using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Keep track of if game is paused
    public static bool GameIsPaused = false;

    [SerializeField]
    private GameObject player, world, dayNight;

    // Update is called once per frame
    private void Update()
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

        // Resume sound
        FindObjectOfType<AudioManager>().UnDampen();

        // Disable pause menu
        transform.GetChild(0).gameObject.SetActive(false);
        GameIsPaused = false;
    }

    public void Pause ()
    {
        // Freeze time
        Time.timeScale = 0f;

        // Lower sound
        FindObjectOfType<AudioManager>().Dampen();

        // Enable pause menu
        transform.GetChild(0).gameObject.SetActive(true);
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

        // Stop sound
        FindObjectOfType<AudioManager>().Stop();

        // Resume game from pause 
        Resume();

        // Return to main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
