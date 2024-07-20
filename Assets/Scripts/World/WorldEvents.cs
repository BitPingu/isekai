using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldEvents : MonoBehaviour
{
    // Call other functions when scene changes
    // public delegate void OnSaveTemp();
    // public OnSaveTemp SaveTemp;
    // public delegate void OnSceneChange();
    // public OnSceneChange SceneChange;

    private TileGrid gen;

    public GameObject elf;

    private void Awake()
    {
        gen = GetComponent<TileGrid>();
    }

    private void OnEnable()
    {
        gen.WorldGen += Initiate;
    }

    private void OnDisable()
    {
        gen.WorldGen -= Initiate;
    }

    private void Initiate()
    {
        Debug.Log("init");
        // SaveTemp();
        // SceneChange();

        if (SceneManager.GetActiveScene().buildIndex == 1) 
        {
            // Attacked by slime
            if (TempData.tempDays == 0)
            {
                // elf.SetActive(true);
                // elf.GetComponent<ElfPosition>().inDanger = true;
                // elf.GetComponent<Animator>().SetBool("Jump", true);
                // elf.GetComponent<PartyMovement>().enabled = false;
                // elf.GetComponent<ElfPosition>().RetrieveTilemap();
            }
        }
    }

    private void Update()
    {
        
    }
}
