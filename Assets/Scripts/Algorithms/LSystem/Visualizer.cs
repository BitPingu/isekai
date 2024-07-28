using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;
using UnityEngine.Tilemaps;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    List<Vector3> positions = new List<Vector3>();

    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    private int length = 8; // adjust value to make roads span wider
    private float angle = 90;

    public Material lineMaterial;
    public Tilemap graphicMap;
    public TileBase tile, tile2, tile3;

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
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        FillTile();
        var sequence = lsystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    private void FillTile()
    {
        for (int i=-100; i<100; i++)
        {
            for (int j=-100; j<100; j++)
            {
                graphicMap.SetTile(new Vector3Int(i, j, 0), tile2);
            }
        }
    }

    private void VisualizeSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.right;
        // Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach(var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameters{
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
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
                    // tempPosition = currentPosition;
                    // currentPosition += direction * Length;

                    // DrawLine(tempPosition, currentPosition, Color.red);
                    // roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                    // graphicMap.SetTile(Vector3Int.RoundToInt(currentPosition), tile);

                    for (int i=0; i<Length; i++)
                    {
                        roadHelper.PlaceStreetPositions2(currentPosition, Vector3Int.RoundToInt(direction), tile);
                        graphicMap.SetTile(Vector3Int.RoundToInt(currentPosition), tile);
                        // if (i==0)
                        //     graphicMap.SetTile(Vector3Int.RoundToInt(currentPosition), tile3);
                        // Debug.Log("i am going direction: " + direction);
                        if (direction.x > 0.1f || direction.x < -0.1f)
                        {
                            graphicMap.SetTile(new Vector3Int(Mathf.RoundToInt(currentPosition.x), Mathf.RoundToInt(currentPosition.y)-1, 0), tile);
                        }
                        else if (direction.y > 0.1f || direction.y < -0.1f)
                        {
                            graphicMap.SetTile(new Vector3Int(Mathf.RoundToInt(currentPosition.x)-1, Mathf.RoundToInt(currentPosition.y), 0), tile);
                        }
                        currentPosition += direction;
                    }
                    
                    Length -= 2;
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.forward) * direction;
                    break;
                default:
                    break;
            }
        }
        structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoadPositions());
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
