using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadHelper : MonoBehaviour
{
    public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    Dictionary<Vector3Int, TileBase> roadDictionary2 = new Dictionary<Vector3Int, TileBase>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPositions()
    {
        // return roadDictionary.Keys.ToList();
        return roadDictionary2.Keys.ToList();
    }

    public void PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if (direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i=0; i<length; i++)
        {
            var position = Vector3Int.RoundToInt(startPosition + direction * i);
            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            var road = Instantiate(roadStraight, position, rotation, transform);
            roadDictionary.Add(position, road);
            if (i==0 || i == length-1)
            {
                fixRoadCandidates.Add(position);
            }
        }
    }

    public void PlaceStreetPositions2(Vector3 startPosition, Vector3Int direction, TileBase road)
    {
        var position = Vector3Int.RoundToInt(startPosition + direction);
        if (roadDictionary2.ContainsKey(position))
        {
            return;
        }
        roadDictionary2.Add(position, road);
    }
}
