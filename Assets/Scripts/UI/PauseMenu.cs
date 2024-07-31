using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager;

    // Update is called once per frame
    private void Update()
    {
        // Esc to resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Resume game from pause 
            MenuController.Resume();

            // Enable menu manager
            menuManager.SetActive(true);

            // Disable pause menu
            gameObject.SetActive(false);
        }
    }

    public void ResumeFromClick ()
    {
        // Play sound fx
        // FindObjectOfType<AudioManager>().PlayFx("Button");

        // Resume game from pause 
        MenuController.Resume();

        // Enable menu manager
        menuManager.SetActive(true);

        // Disable pause menu
        gameObject.SetActive(false);
    }

    public void Save()
    {
        // Play sound fx
        // FindObjectOfType<AudioManager>().PlayFx("Button");

        // Save all data
        SaveSystem.Save();
    }

    public void Quit()
    {
        // Play sound fx
        // FindObjectOfType<AudioManager>().PlayFx("Button");

        // Resume time
        Time.timeScale = 1f;

        // Resume sound
        FindObjectOfType<AudioManager>().UnDampen();

        // Stop sound
        FindObjectOfType<AudioManager>().Stop();

        // Resume game from pause 
        MenuController.Resume();

        // Return to main menu
        SceneManager.LoadScene(0);
    }
}
