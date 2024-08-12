using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room
{
    public Vector2Int center;
    public HashSet<Vector2Int> floor;
    public string type;

    public Room(Vector2Int c, HashSet<Vector2Int> f)
    {
        center = c;
        floor = f;
    }
}

public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    private List<Room> rooms = new List<Room>();
    public GameObject player, renderCanvas;
    public Text textPrefab;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        // Define room boundaries using binary space partitioning
        var roomsList = walk.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        // Make rooms 
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        // Get room centers
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // Connect rooms with corridors
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

        // Paint dungeon
        floor.UnionWith(corridors);
        tilemapVisualizer.PaintFloorTiles(floor);
        tilemapVisualizer.PaintCorridors(corridors);
        wall.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i=0; i<roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var pos in roomFloor)
            {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) 
                    && pos.y >= (roomBounds.yMin + offset) && pos.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(pos);
                }
            }

            // save room data
            rooms.Add(new Room(roomCenter, roomFloor));
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        int maxRooms = roomCenters.Count;
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        // pick random room
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        // spawn entry
        Debug.Log("instant at " + currentRoomCenter);
        Instantiate(player, new Vector3(currentRoomCenter.x, currentRoomCenter.y), Quaternion.identity);
        rooms.Find(x => x.center == currentRoomCenter).type = "Entry";

        Text tempTextBox = Instantiate(textPrefab, new Vector3(currentRoomCenter.x, currentRoomCenter.y), Quaternion.identity) as Text;
        tempTextBox.transform.SetParent(renderCanvas.transform, false);
        tempTextBox.fontSize = 6;
        tempTextBox.text = currentRoomCenter.ToString() + " " + rooms.Find(x => x.center == currentRoomCenter).type;

        // connect nearby rooms
        while (roomCenters.Count > 0)
        {
            // find closest room to current room
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);

            // connect corridor
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            corridors.UnionWith(newCorridor);

            // next room
            currentRoomCenter = closest;

            // categorize room
            if (roomCenters.Count == 0)
            {
                rooms.Find(x => x.center == currentRoomCenter).type = "End";
            }
            else if (roomCenters.Count < maxRooms/2)
            {
                rooms.Find(x => x.center == currentRoomCenter).type = "Near";
            }
            else
            {
                rooms.Find(x => x.center == currentRoomCenter).type = "E";
            }
            
            tempTextBox = Instantiate(textPrefab, new Vector3(currentRoomCenter.x, currentRoomCenter.y), Quaternion.identity) as Text;
            tempTextBox.transform.SetParent(renderCanvas.transform, false);
            tempTextBox.fontSize = 6;
            tempTextBox.text = currentRoomCenter.ToString() + " " + rooms.Find(x => x.center == currentRoomCenter).type;
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = currentRoomCenter;
        corridor.Add(pos);

        while (pos.y != destination.y)
        {
            if (destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if (destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }

        while (pos.x != destination.x)
        {
            if (destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if (destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach(var pos in roomCenters)
        {
            float currentDistance = Vector2.Distance(pos, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = pos;
            }
        }

        // return center pos of closest room
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col=offset; col<room.size.x-offset; col++)
            {
                for (int row=offset; row<room.size.y-offset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }

        return floor;
    }
}
