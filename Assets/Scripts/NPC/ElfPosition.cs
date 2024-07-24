using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElfPosition : MonoBehaviour
{
    public Vector3 spawnPoint;
    private TilemapStructure groundMap, overworldMap;

    [SerializeField]
    private float maxDistance; // default is 3.5f
    
    public Vector2Int prevPos;
    public Vector2Int currentPos;
    public int prevGTile, prevOTile;
    public int currentGTile, currentOTile;

    // Call other functions when position changes
    public delegate void OnPosChange();
    public OnPosChange PosChange;

    // Call other functions when tile types change
    public delegate void OnGTileChange();
    public OnGTileChange GTileChange;
    public delegate void OnOTileChange();
    public OnOTileChange OTileChange;

    public Dialogue dialogue;
    public GameObject icon;
    private GameObject newIcon;

    public static ElfPosition elf;

    private void Awake()
    {
        if (TempData.elfSaved)
        {
            if (elf == null)
            {
                elf = this;
            } 
            else 
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        // Attach delegates
        PosChange += CheckPosition;
        PosChange += OTileSound;

        // Set event icon
        Vector3 iconPos = new Vector3(transform.position.x, transform.position.y + 1);
        newIcon = Instantiate(icon, iconPos, Quaternion.identity);
        newIcon.transform.parent = gameObject.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!TempData.elfSaved && collision.gameObject.name == "Player")
        {
            // Move to worldevents
            newIcon.SetActive(false);
            TempData.elfSaved = true;
            TempData.initElf = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Animator>().SetBool("Jump", false);
            GetComponent<PartyMovement>().enabled = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!TempData.elfSaved)
        {
            if (FindObjectOfType<PlayerPosition>().transform.position.x - transform.position.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (CheckPlayer())
            {
                // Debug.Log("hi");
                // FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                newIcon.SetActive(true);
            }
            else
            {
                newIcon.SetActive(false);
            }
        }

        // Retrieve coordinates
        currentPos = Vector2Int.FloorToInt(transform.position);
        TempData.tempElfPos = new Vector3(transform.position.x, transform.position.y);

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }

    private void RetrieveTilemap()
    {
        // Retrieve tilemap components
        groundMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Ground);
        overworldMap = FindObjectOfType<TileGrid>().GetTilemap(TilemapType.Overworld);
    }

    // Generates a random spawn point
    public void Spawn(bool initialSpawn, int scene)
    {
        RetrieveTilemap();
        switch (scene)
        {
            case 1:
                if (!TempData.elfSaved)
                {
                    // Not saved yet
                    // Debug.Log("elf in danger");
                    if (initialSpawn)
                    {
                        // Debug.Log("new elf");
                        // Coming from main menu
                        if (TempData.newGame)
                        {
                            // Generate initial spawn point
                            float xCoord, yCoord, currentTile;
                            // Get player spawn
                            Vector3 worldSpawn = TempData.tempPlayerStartingSpawn;
                            do
                            {
                                // Choose random spawn point
                                xCoord = Random.Range(worldSpawn.x-5, worldSpawn.x+5);
                                yCoord = Random.Range(worldSpawn.y-5, worldSpawn.y+5);

                                // Check tile
                                currentTile = groundMap.GetTile((int)xCoord, (int)yCoord);
                            }
                            while (currentTile != (int)GroundTileType.Land);

                            // Generate spawn point
                            spawnPoint = new Vector3(xCoord, yCoord);
                            TempData.tempElfStartingSpawn = spawnPoint;
                        }
                        else
                        {
                            // Debug.Log("load unsaved elf");
                            // Load unsaved elf position
                            SaveData saveData = SaveSystem.Load();
                            spawnPoint.x = saveData.saveElfPos[0];
                            spawnPoint.y = saveData.saveElfPos[1];
                            spawnPoint.z = saveData.saveElfPos[2];
                        }
                    }
                    else
                    {
                        // Debug.Log("notsaved, going to spawn");
                        // Stay at spawn point
                        spawnPoint = TempData.tempElfStartingSpawn;
                    }
                    // Attacked by slime
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    GetComponent<Animator>().SetBool("Jump", true);
                    GetComponent<PartyMovement>().enabled = false;
                }
                else
                {
                    // saved
                    if (initialSpawn)
                    {
                        // Coming from main menu (only possible when load)
                        // Debug.Log("load elf");
                        // Load elf position
                        SaveData saveData = SaveSystem.Load();
                        spawnPoint.x = saveData.saveElfPos[0];
                        spawnPoint.y = saveData.saveElfPos[1];
                        spawnPoint.z = saveData.saveElfPos[2];
                    }
                    else
                    {
                        // exiting from building
                        // Debug.Log("saved2, going to player");
                        spawnPoint = TempData.tempPlayerPos;
                    }
                }
                break;
            case 2:
            case 3:
                // Dungeon or village spawn
                // Debug.Log("building, going to player");
                spawnPoint = TempData.tempPlayerPos;
                break;
            default:
                Debug.Log("elf cannot spawn!");
                break;
            
        }

        // Set spawn point
        transform.position = spawnPoint;
        prevPos = currentPos = Vector2Int.FloorToInt(transform.position);

        // Retrieve spawn point tile
        prevGTile = currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        prevOTile = currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);
    }

    // Looks at current position
    private void CheckPosition()
    {
        // Get current tiles from position
        currentGTile = groundMap.GetTile(currentPos.x, currentPos.y);
        currentOTile = overworldMap.GetTile(currentPos.x, currentPos.y);

        // Ground tile check
        if (currentGTile != prevGTile)
        {
            // GTileChange();
            prevGTile = currentGTile;
        }

        // Object tile check
        if (currentOTile != prevOTile)
        {
            // OTileChange();
            prevOTile = currentOTile;
        }
    }

    private bool CheckPlayer()
    {
        // Calculate current distance from player
        float distance = Vector3.Distance(FindObjectOfType<PlayerPosition>().transform.position, transform.position);

        if (distance < maxDistance)
        {
            // Debug.Log("hi");
            return true;
        }

        return false;
    }

    // Play sound based on current object tile
    private void OTileSound()
    {
        if (CheckPlayer())
        {
            // Get sound from current object tile
            if (currentOTile == (int)FoilageTileType.Tree)
            {
                FindObjectOfType<AudioManager>().PlayFx("Tree");
            }
        }
    }
}
