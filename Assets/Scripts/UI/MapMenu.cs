using UnityEngine;

public class MapMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager, enemies, villagers;

    // Update is called once per frame
    private void Update()
    {
        // M to resume
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Play sound fx
            FindObjectOfType<AudioManager>().PlayFx("Close");

            // Resume game from pause 
            MenuManager.Resume();

            // Enable menu manager
            menuManager.SetActive(true);

            // Enable NPCs
            enemies.SetActive(true);
            villagers.SetActive(true);

            // Disable map 
            gameObject.SetActive(false);
        }
    }
}
