using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class Village
{
    public Vector2 vilCenter;
    public string vilSequence;
    public Village(Vector2 center, string sequence)
    {
        vilCenter = center;
        vilSequence = sequence;
    }
}

public class VillageGeneration : MonoBehaviour
{
    [SerializeField]
    private LSystemGenerator lsystem;
    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    private List<Village> villages = new List<Village>();

    private Vector3 vilCenter, direction;
    private int length = 1; // adjust value to make roads span wider (default is 8)
    private float angle = 90;
    private int vilSquareRad = 2;
    private int vilMaxWidth, vilMaxHeight;

    private TilemapStructure groundMap;
    public GameObject house, fountain, townhall;
    private GameObject villageTree;

    public int Length
    {
        get
        {
            if (length > 3)
            {
                return length;
            }
            else 
            {
                // Min length of road branches
                if (direction.x > 0.1f || direction.x < -0.1f)
                {
                    return 4;
                }
                else if (direction.y > 0.1f || direction.y < -0.1f)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
        }
        set => length = value;
    }

    public void Initialize(TilemapStructure tilemap)
    {
        // Get tilemap structure
        groundMap = tilemap;

        if (TempData.loadGame)
        {
            // Load village data
            List<int> villageCoordsX = SaveSystem.Load().saveVillageCoordsX;
            List<int> villageCoordsY = SaveSystem.Load().saveVillageCoordsY;
            List<string> villageSequences = SaveSystem.Load().saveVillageSequences;

            for (int i=0; i<villageCoordsX.Count; i++)
            {
                villages.Add(new Village(new Vector2(villageCoordsX[i], villageCoordsY[i]), villageSequences[i]));
            }
        }
        else
        {
            // Generate village coords
            List<Vector2> vilPoints = sampling.GeneratePoints(tilemap);

            foreach (Vector2 point in vilPoints)
            {
                // Skip water coords
                if (groundMap.GetTile(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)) == (int)GroundTileType.Water)
                    continue;

                // Generate lsystem sequence for each village
                string vilSequence = lsystem.GenerateSentence();

                // Add new village to list
                villages.Add(new Village(point, vilSequence));
            }
        }

        // Save vil data
        TempData.tempVillages = villages;

        // Generate villages
        foreach (Village vil in villages)
        {
            // Set village centerpoint
            vilCenter = new Vector3(Mathf.FloorToInt(vil.vilCenter.x), Mathf.FloorToInt(vil.vilCenter.y));

            // Set village width
            vilMaxWidth = 0;
            vilMaxHeight = 0;

            // Start heading east in 2d world space
            direction = Vector3.right;

            // Generate village
            VisualizeSequence(vil.vilSequence);

            // Spawn fountain (to be added later)
            villageTree = Instantiate(fountain, new Vector3(vilCenter.x+.5f, vilCenter.y+.5f), Quaternion.identity, transform);

            // Village zone data for spawning in world
            villageTree.GetComponent<BoxCollider2D>().size = new Vector2(vilMaxWidth*2.3f, vilMaxHeight*2.7f);
        }

        // Render tiles
        groundMap.UpdateTiles();
    }

    private void SpawnVillageSquare(Vector3 centerPos)
    {
        for (int i=Mathf.FloorToInt(centerPos.x)-vilSquareRad; i<=Mathf.FloorToInt(centerPos.x)+vilSquareRad; i++)
        {
            for (int j=Mathf.FloorToInt(centerPos.y)-vilSquareRad; j<=Mathf.FloorToInt(centerPos.y)+vilSquareRad; j++)
            {
                // Place road for village square
                groundMap.SetTile(i, j, (int)GroundTileType.VillagePath, setDirty : false);
            }
        }

        // townhall
        groundMap.SetTile(Mathf.FloorToInt(vilCenter.x), Mathf.FloorToInt(vilCenter.y+vilSquareRad+1), (int)GroundTileType.VillagePlot, setDirty : false);
        Instantiate(townhall, new Vector3(vilCenter.x+.5f, vilCenter.y+vilSquareRad+1.5f), Quaternion.identity, transform);
    }

    private void SpawnHouse(Vector3 housePos)
    {
        if (groundMap.GetTile(Mathf.FloorToInt(housePos.x), Mathf.FloorToInt(housePos.y)) == (int)GroundTileType.Land)
        {
            // Reserve plot (tile)
            groundMap.SetTile(Mathf.FloorToInt(housePos.x), Mathf.FloorToInt(housePos.y), (int)GroundTileType.VillagePlot, setDirty : false);

            // Place house object
            Instantiate(house, housePos, Quaternion.identity, transform);
        }
    }

    private void VisualizeSequence(string sequence)
    {
        // Initialize save points stack
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        // Start current position in center of village
        var currentPosition = vilCenter;

        // Initialize village square
        SpawnVillageSquare(currentPosition);

        foreach(var letter in sequence)
        {
            // Get letter encoding from sequence
            EncodingLetters encoding = (EncodingLetters)letter;

            // Determine generation
            switch (encoding)
            {
                case EncodingLetters.save:
                    // Debug.Log("save");
                    // Save current parameters and push to stack
                    savePoints.Push(new AgentParameters{
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        // Debug.Log("load");
                        // Load saved parameters from stack
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("Dont have save point in our stack");
                    }
                    break;
                case EncodingLetters.draw:

                    // offset village square
                    if (currentPosition == vilCenter && direction.x > 0.1f)
                    {
                        currentPosition = new Vector3(vilCenter.x+vilSquareRad+1, vilCenter.y, 0);
                    }
                    else if (currentPosition == vilCenter && direction.x < -0.1f)
                    {
                        currentPosition = new Vector3(vilCenter.x-(vilSquareRad+1), vilCenter.y, 0);
                    }

                    // Place road
                    for (int i=0; i<Length; i++)
                    {
                        // Check for water
                        if (groundMap.GetTile(Mathf.FloorToInt(currentPosition.x), Mathf.FloorToInt(currentPosition.y)) == (int)GroundTileType.Water)
                            continue;

                        // Road tile
                        groundMap.SetTile(Mathf.FloorToInt(currentPosition.x), Mathf.FloorToInt(currentPosition.y), (int)GroundTileType.VillagePath, setDirty : false);

                        if (i == Length/2)
                        {
                            // Place house
                            if (direction.x > 0.1f || direction.x < -0.1f)
                            {
                                Vector3 housePos = new Vector3(currentPosition.x+.5f, currentPosition.y+1.5f, 0);
                                SpawnHouse(housePos);
                            }
                        }

                        // Move in current direction
                        currentPosition += direction;

                        // Get village zone
                        if (Mathf.FloorToInt(Math.Abs(currentPosition.x-(int)vilCenter.x)) > vilMaxWidth)
                            vilMaxWidth = Mathf.FloorToInt(Math.Abs(currentPosition.x-(int)vilCenter.x));
                        if (Mathf.FloorToInt(Math.Abs(currentPosition.y-(int)vilCenter.y)) > vilMaxHeight)
                            vilMaxHeight = Mathf.FloorToInt(Math.Abs(currentPosition.y-(int)vilCenter.y));
                    }
                    
                    Length -= 1; // default is -= 2
                    break;
                case EncodingLetters.turnRight:
                    // Set direction towards right
                    direction = Quaternion.AngleAxis(-angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    // Set direction towards left
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
                    break;
                default:
                    break;
            }
        }
    }
}
