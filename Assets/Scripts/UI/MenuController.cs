using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Keep track of if a menu is opened
    public static bool openMenu = false;

    [SerializeField]
    private GameObject pauseMenuUI, mapUI;
    [SerializeField]
    private KeyCode pauseKey, mapKey;

    private static MenuController menu;

    private void Awake()
    {
        if (menu == null)
        {
            menu = this;
        } 
        else 
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject.transform.parent.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        // Esc to pause
        if (Input.GetKeyDown(pauseKey))
        {
            // Pause game
            Pause();

            // Enable pause menu
            pauseMenuUI.SetActive(true);

            // Disable menu manager
            gameObject.SetActive(false);
        }

        // M to map
        if (Input.GetKeyDown(mapKey))
        {
            // Play sound fx
            FindObjectOfType<AudioManager>().PlayFx("Open");

            // Pause game
            Pause();

            // Enable map
            mapUI.SetActive(true);

            // Disable menu manager
            gameObject.SetActive(false);
        }
    }

    public static void Resume()
    {
        // Resume time
        Time.timeScale = 1f;
        FindObjectOfType<PlayerController>().enabled = true;

        // Resume sound
        FindObjectOfType<AudioManager>().UnDampen();

        // Unpause game
        openMenu = false;
    }

    public void Pause()
    {
        // Freeze time
        Time.timeScale = 0f;
        FindObjectOfType<PlayerController>().enabled = false;

        // Lower sound
        FindObjectOfType<AudioManager>().Dampen();

        // Pause game
        openMenu = true;
    }
}
