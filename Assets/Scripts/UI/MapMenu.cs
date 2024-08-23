using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuManager;

    // Update is called once per frame
    private void Update()
    {
        // M or esc to resume
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            // Play sound fx
            FindObjectOfType<AudioManager>().PlayFx("Close");

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
        if (FindObjectOfType<MapController>())
        {
            FindObjectOfType<MapController>().enabled = true;
            FindObjectOfType<MapController>().GetComponent<CameraController>().enabled = false;
        }

        GameObject[] chu = GameObject.FindGameObjectsWithTag("Chunk");
        if (chu[0] && FindObjectOfType<PlayerPosition>().currentArea.Contains("Overworld"))
        {
            foreach (GameObject c in chu)
            {
                foreach (Transform child in c.transform)
                {
                     child.gameObject.SetActive(true);
                }
            }
        }
        else if (chu[0] && FindObjectOfType<PlayerPosition>().currentArea.Contains("Underground"))
        {
            foreach (GameObject c in chu)
            {
                foreach (Transform child in c.transform)
                {
                    if (child.gameObject.name.Contains("Dungeon"))
                        child.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (FindObjectOfType<MapController>())
        {
            FindObjectOfType<MapController>().enabled = false;
            FindObjectOfType<MapController>().GetComponent<CameraController>().enabled = true;
        }

        GameObject[] chu = GameObject.FindGameObjectsWithTag("Chunk");
        if (chu[0] && FindObjectOfType<PlayerPosition>().currentArea.Contains("Overworld"))
        {
            foreach (GameObject c in chu)
            {
                if (!c.GetComponent<ChunkLoader>().containsPlayer)
                {
                    foreach (Transform child in c.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (chu[0] && FindObjectOfType<PlayerPosition>().currentArea.Contains("Underground"))
        {
            foreach (GameObject c in chu)
            {
                foreach (Transform child in c.transform)
                {
                    if (!c.GetComponent<ChunkLoader>().containsPlayer)
                    {
                        if (child.gameObject.name.Contains("Dungeon"))
                            child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
