using UnityEngine;

public class MapMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager, npcs;

    // Update is called once per frame
    private void Update()
    {
        // M or esc to resume
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            // Play sound fx
            // FindObjectOfType<AudioManager>().PlayFx("Close");

            // Resume game from pause 
            MenuController.Resume();

            // Enable menu manager
            menuManager.SetActive(true);

            // Disable map 
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        FindObjectOfType<MapController>().enabled = true;
        FindObjectOfType<MapController>().GetComponent<CameraController>().enabled = false;
        npcs = GameObject.Find("NPC");
        if (npcs)
            npcs.transform.localScale = new Vector3(0, 0, 0);
    }

    private void OnDisable()
    {
        FindObjectOfType<MapController>().enabled = false;
        FindObjectOfType<MapController>().GetComponent<CameraController>().enabled = true;
        npcs = GameObject.Find("NPC");
        if (npcs)
            npcs.transform.localScale = new Vector3(1, 1, 1);
    }
}
