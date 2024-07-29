using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;
using UnityEngine.Tilemaps;
using System;


public class VillageGeneration : MonoBehaviour
{
    [SerializeField]
    private LSystemGenerator lsystem;

    private int length = 6; // adjust value to make roads span wider (default is 8)
    private float angle = 90;
    private int vilSquareRad = 2;
    private int vilMaxWidth = 0, vilMaxHeight = 0;

    public Tilemap graphicMap;
    public TileBase road, grass, tile3, tile4;
    public GameObject house, fountain, townhall;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else 
            {
                return 2;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        FillTile();
        var sequence = lsystem.GenerateSentence();
        VisualizeSequence(sequence);
        Instantiate(fountain, new Vector3(.5f, .5f), Quaternion.identity, transform);
        Debug.Log("Max Width: " + vilMaxWidth*2);
        Debug.Log("Max Height: " + vilMaxHeight*2);
    }

    private void FillTile()
    {
        // for testing only
        for (int i=-100; i<100; i++)
        {
            for (int j=-100; j<100; j++)
            {
                graphicMap.SetTile(new Vector3Int(i, j, 0), grass);
            }
        }
    }

    private void SpawnVillageSquare(Vector3 centerPos)
    {
        for (int i=Mathf.FloorToInt(centerPos.x)-vilSquareRad; i<=Mathf.FloorToInt(centerPos.x)+vilSquareRad; i++)
        {
            for (int j=Mathf.FloorToInt(centerPos.y)-vilSquareRad; j<=Mathf.FloorToInt(centerPos.y)+vilSquareRad; j++)
            {
                graphicMap.SetTile(new Vector3Int(i, j, 0), road);
            }
        }

        // townhall
        Instantiate(townhall, new Vector3(.5f, vilSquareRad + 1.5f), Quaternion.identity, transform);
    }

    private void SpawnHouse(Vector3 housePos)
    {
        if (graphicMap.GetTile(new Vector3Int(Mathf.FloorToInt(housePos.x), Mathf.FloorToInt(housePos.y), 0)) != road)
        {
            // Debug.Log("spawn at " + new Vector3Int(Mathf.RoundToInt(housePos.x), Mathf.RoundToInt(housePos.y), 0));
            Instantiate(house, housePos, Quaternion.identity, transform);
        }
    }

    private void VisualizeSequence(string sequence)
    {
        // Initialize save points stack
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        // Start current position in center of village
        var currentPosition = Vector3.zero;

        // Initialize village square
        SpawnVillageSquare(currentPosition);

        // Start heading east in 2d world space
        Vector3 direction = Vector3.right;

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
                    if (currentPosition == Vector3.zero && direction.x > 0.1f)
                    {
                        currentPosition = new Vector3(vilSquareRad, 0, 0);
                    }
                    else if (currentPosition == Vector3.zero && direction.x < -0.1f)
                    {
                        currentPosition = new Vector3(-vilSquareRad, 0, 0);
                    }
                    // Debug.Log("start at: " + currentPosition + " heading " + direction);
                    // Place road
                    for (int i=0; i<Length; i++)
                    {
                        graphicMap.SetTile(Vector3Int.RoundToInt(currentPosition), road);
                        // if (i==0)
                        //     graphicMap.SetTile(Vector3Int.RoundToInt(currentPosition), tile3);
                        if (i == Length/2)
                        {
                            // Place house
                            if (direction.x > 0.1f || direction.x < -0.1f)
                            {
                                Vector3 housePos = new Vector3(currentPosition.x+.5f, currentPosition.y+1.5f, 0);
                                SpawnHouse(housePos);
                            }
                            else if (direction.y > 0.1f || direction.y < -0.1f)
                            {
                                // Vector3 housePos = new Vector3(currentPosition.x+1.5f, currentPosition.y+.5f, 0);
                                // SpawnHouse(housePos);
                                // housePos = new Vector3(currentPosition.x-.5f, currentPosition.y+.5f, 0);
                                // SpawnHouse(housePos);
                            }
                        }
                        // Debug.Log("moving " + currentPosition);

                        // Make road wider
                        // if (direction.x > 0.1f || direction.x < -0.1f)
                        // {
                        //     graphicMap.SetTile(new Vector3Int(Mathf.RoundToInt(currentPosition.x), Mathf.RoundToInt(currentPosition.y)-1, 0), road);
                        // }
                        // else if (direction.y > 0.1f || direction.y < -0.1f)
                        // {
                        //     graphicMap.SetTile(new Vector3Int(Mathf.RoundToInt(currentPosition.x)-1, Mathf.RoundToInt(currentPosition.y), 0), road);
                        // }

                        currentPosition += direction;
                        if (Math.Abs(currentPosition.x) > vilMaxWidth)
                            vilMaxWidth = Mathf.FloorToInt(Math.Abs(currentPosition.x));
                        if (Math.Abs(currentPosition.y) > vilMaxHeight)
                            vilMaxHeight = Mathf.FloorToInt(Math.Abs(currentPosition.y));
                    }
                    
                    Length -= 1; // default is -= 2
                    break;
                case EncodingLetters.turnRight:
                    // Debug.Log("right");
                    // Set direction towards right
                    direction = Quaternion.AngleAxis(-angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    // Debug.Log("left");
                    // Set direction towards left
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
                    break;
                default:
                    break;
            }
        }
    }
}
