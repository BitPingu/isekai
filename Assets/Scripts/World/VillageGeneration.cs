using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;
using UnityEngine.Tilemaps;
using System;


public class VillageGeneration : TilemapAlgorithmBase
{
    [SerializeField]
    private LSystemGenerator lsystem;

    private Vector3 vilCenter, direction;
    private int length = 1; // adjust value to make roads span wider (default is 8)
    private float angle = 90;
    private int vilSquareRad = 2;
    private int vilMaxWidth = 0, vilMaxHeight = 0;

    private TilemapStructure groundMap;
    public GameObject house, fountain, townhall;

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

    private void Start()
    {
        
    }

    private void FillTile()
    {
        // for testing only
        for (int i=-100; i<100; i++)
        {
            for (int j=-100; j<100; j++)
            {
                // graphicMap.SetTile(new Vector3Int(i, j, 0), grass);
            }
        }
    }

    public override void Apply(TilemapStructure tilemap)
    {
        // Generate lsystem sequence
        var sequence = lsystem.GenerateSentence();

        // Get tilemap structure
        groundMap = tilemap;

        // Generate village coords
        vilCenter.x = tilemap.width/2;
        vilCenter.y = tilemap.height/2;

        // Start heading east in 2d world space
        direction = Vector3.right;

        // Generate village
        VisualizeSequence(sequence);

        // Spawn fountain (to be added later)
        Instantiate(fountain, new Vector3(.5f, .5f), Quaternion.identity, transform);

        // Village zone data for spawning in world
        Debug.Log("Max Width: " + vilMaxWidth*2);
        Debug.Log("Max Height: " + vilMaxHeight*2);
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
        Instantiate(townhall, new Vector3(.5f, vilSquareRad + 1.5f), Quaternion.identity, transform);
    }

    private void SpawnHouse(Vector3 housePos)
    {
        if (groundMap.GetTile(Mathf.FloorToInt(housePos.x), Mathf.FloorToInt(housePos.y)) == (int)GroundTileType.Land)
        {
            // Reserve plot (tile)
            groundMap.SetTile(Mathf.FloorToInt(housePos.x), Mathf.FloorToInt(housePos.y-1), (int)GroundTileType.VillagePlot, setDirty : false);

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
        Debug.Log("village center at: " + vilCenter);

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
                                Vector3 housePos = new Vector3(currentPosition.x+.5f, currentPosition.y+2f, 0);
                                SpawnHouse(housePos);
                            }
                        }

                        currentPosition += direction;
                        if (Math.Abs(currentPosition.x) > vilMaxWidth)
                            vilMaxWidth = Mathf.FloorToInt(Math.Abs(currentPosition.x));
                        if (Math.Abs(currentPosition.y) > vilMaxHeight)
                            vilMaxHeight = Mathf.FloorToInt(Math.Abs(currentPosition.y));
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
