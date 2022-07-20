using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager, player, world, dayNight;

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
        MenuManager.Resume();

        // Enable menu manager
        menuManager.SetActive(true);

        // Disable pause menu
        gameObject.SetActive(false);
    }

    public void Save()
    {
        // Save player data
        PlayerController playerData = player.GetComponent<PlayerController>();

        // Save world data
        TileGrid worldData = world.GetComponent<TileGrid>();
        DayAndNightCycle dayNightData = dayNight.GetComponent<DayAndNightCycle>();
        
        // Save fog data
        world.GetComponentInChildren<FogData>().GetClearFog();
        FogData fogData = worldData.GetComponentInChildren<FogData>();

        // Save all data
        SaveSystem.SaveAllData(playerData, worldData, dayNightData, fogData);
    }

    public void Quit()
    {
        // Resume time
        Time.timeScale = 1f;

        // Stop sound
        FindObjectOfType<AudioManager>().Stop();

        // Resume game from pause 
        MenuManager.Resume();

        // Return to main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
