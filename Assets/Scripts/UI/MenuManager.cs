using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Keep track of if a menu is opened
    public static bool openMenu = false;

    [SerializeField]
    private GameObject pauseMenuUI, mapUI, enemies, villagers;

    // Update is called once per frame
    private void Update()
    {
        // Esc to pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause game
            Pause();

            // Enable pause menu
            pauseMenuUI.SetActive(true);

            // Disable menu manager
            gameObject.SetActive(false);
        }

        // M to map
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Play sound fx
            FindObjectOfType<AudioManager>().PlayFx("Open");

            // Pause game
            Pause();

            // Enable map
            mapUI.SetActive(true);

            // Disable NPCs
            enemies.SetActive(false);
            villagers.SetActive(false);

            // Disable menu manager
            gameObject.SetActive(false);
        }
    }

    public static void Resume()
    {
        // Resume time
        Time.timeScale = 1f;

        // Resume sound
        FindObjectOfType<AudioManager>().UnDampen();

        // Unpause game
        openMenu = false;
    }

    public void Pause()
    {
        // Freeze time
        Time.timeScale = 0f;

        // Lower sound
        FindObjectOfType<AudioManager>().Dampen();

        // Pause game
        openMenu = true;
    }
}
