using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager;

    [SerializeField]
    private PlayerPosition position;
    [SerializeField]
    private TileGrid grid;
    [SerializeField]
    private DayAndNightCycle dayNight;
    [SerializeField]
    private FogData fog;
    [SerializeField]
    private BuildingData building;

    // Update is called once per frame
    private void Update()
    {
        // Esc to resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeFromClick();
        }
    }

    public void ResumeFromClick ()
    {
        // Resume game from pause 
        MenuController.Resume();

        // Enable menu manager
        menuManager.SetActive(true);

        // Disable pause menu
        gameObject.SetActive(false);
    }

    public void Save()
    {
        // Save all data
        SaveSystem.SaveAllData(position, grid, dayNight, fog, building);
    }

    public void Quit()
    {
        // Resume time
        Time.timeScale = 1f;

        // Stop sound
        FindObjectOfType<AudioManager>().Stop();

        // Resume game from pause 
        MenuController.Resume();

        // Return to main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
