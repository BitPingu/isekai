using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Algorithms/Dungeon/Generator2")]
public class CorridorFirstDungeonGeneration : ScriptableObject
{
    [SerializeField]
    private SimpleRandomWalkSO randomWalkParameters;
    [SerializeField]
    private int corridorLen = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = .8f;

    private List<Room> rooms;

    private TileGrid grid;
    private Text textPrefab;
    private GameObject renderCanvas;

    public void Initialize(TileGrid g, Vector2Int startPos, List<Room> roo)
    {
        grid = g;
        rooms = roo;

        // textPrefab = t;
        // renderCanvas = r;

        CorridorFirstGeneration(startPos);
    }

    private void CorridorFirstGeneration(Vector2Int startPos)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        // Create corridors
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions, startPos);

        // Create rooms
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // Dead ends
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        // Assign rooms
        AssignRooms();

        // Merge created corridors with rooms
        floorPositions.UnionWith(roomPositions);

        // Increase corridor size
        for (int i=0; i<corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            // corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        PaintFloorTiles(floorPositions);
        CreateWalls(floorPositions, grid.GetTilemap(TilemapType.DungeonUnderground));
    }

    private void AssignRooms()
    {
        for (int i=0; i<rooms.Count; i++)
        {
            if (i == 0)
            {
                rooms[i].type = "Entry";
            }
            else if (i == rooms.Count-1)
            {
                rooms[i].type = "End";
            }
            else if (i >= rooms.Count/2)
            {
                rooms[i].type = "Near";
            }
            else
            {
                rooms[i].type = "E";
            }

            // Text tempTextBox = Instantiate(textPrefab, new Vector3(rooms[i].center.x, rooms[i].center.y-1), Quaternion.identity) as Text;
            // tempTextBox.transform.SetParent(renderCanvas.transform, false);
            // tempTextBox.fontSize = 2;
            // tempTextBox.text = rooms[i].center.ToString() + " " + rooms[i].type + " " + i.ToString();
        }
    }

    private HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLen, TileGrid grid)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for (int i=0; i<walkLen; i++)
        {
            bool dunCollide;
            do
            {
                dunCollide = false;
                var direction = Direction2D.GetRandomCardinalDirection();
                var newPos = prevPos + direction;

                Vector2Int tempPos = newPos + (2*direction);
                var otherDungeonNeighbors = grid.GetTilemap(TilemapType.DungeonUnderground).GetTile(tempPos.x, tempPos.y);
                if (otherDungeonNeighbors == (int)GroundTileType.Cliff)
                {
                    // dunCollide = true;
                }
                else
                {
                    path.Add(newPos);
                    prevPos = newPos;
                }
            }
            while (dunCollide);
        }
        return path;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions, Vector2Int sPos)
    {
        // Start pos
        var currentPos = sPos;
        potentialRoomPositions.Add(currentPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        // Add corridors
        for (int i=0; i<corridorCount; i++)
        {
            // New corridor
            var corridor = RandomWalkCorridor(currentPos, corridorLen, grid);
            corridors.Add(corridor);

            // Move to end of corridor
            currentPos = corridor[corridor.Count-1];

            // New potential room
            potentialRoomPositions.Add(currentPos);

            // Total corridors
            floorPositions.UnionWith(corridor);
        }

        return corridors;
    }

    private List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLen, TileGrid grid)
    {
        List<Vector2Int> corridor;
        bool dunCollide;
        do
        {
            corridor = new List<Vector2Int>();
            dunCollide = false;
            var direction = Direction2D.GetRandomCardinalDirection();
            var currentPos = startPos;
            corridor.Add(currentPos);

            for (int i=0; i<corridorLen; i++)
            {
                currentPos += direction;

                Vector2Int tempPos = currentPos + (2*direction);
                var otherDungeonNeighbors = grid.GetTilemap(TilemapType.DungeonUnderground).GetTile(tempPos.x, tempPos.y);
                if (otherDungeonNeighbors == ((int)GroundTileType.Cliff))
                {
                    // dunCollide = true;
                    break;
                }
                else
                {
                    corridor.Add(currentPos);   
                }
            }
        } while (dunCollide);

        return corridor;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        // sort rooms by unique id
        // List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        List<Vector2Int> roomsToCreate = potentialRoomPositions.Take(roomToCreateCount).ToList();

        // add rooms
        foreach (var roomPos in roomsToCreate)
        {
            // generate room
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos, grid);

            // save room
            rooms.Add(new Room(roomPos, roomFloor));

            // total rooms
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position, TileGrid grid)
    {
        var currentPos = position;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        for (int i=0; i<parameters.iterations; i++)
        {
            var path = SimpleRandomWalk(currentPos, parameters.walkLen, grid);
            floorPos.UnionWith(path);
            if (parameters.startRandomlyEachIter)
                currentPos = floorPos.ElementAt(UnityEngine.Random.Range(0,floorPos.Count));
        }
        return floorPos;
    }

     private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(pos + direction))
                    neighboursCount++;

            }
            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }

        return deadEnds;
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var pos in deadEnds)
        {
            // Check if no room is touching
            if (roomFloors.Contains(pos) == false)
            {
                // Generate room
                var room = RunRandomWalk(randomWalkParameters, pos, grid);

                // save room
                rooms.Add(new Room(pos, room));

                // Total rooms
                roomFloors.UnionWith(room);
            }
        }
    }

    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int prevDirection = Vector2Int.zero;

        for (int i=1; i<corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (prevDirection != Vector2Int.zero && directionFromCell != prevDirection)
            {
                // handle corner
                for (int x=-1; x<2; x++)
                {
                    for (int y=-1; y<2; y++)
                    {
                        newCorridor.Add(corridor[i-1] + new Vector2Int(x,y));
                    }
                }
                prevDirection = directionFromCell;
            }
            else
            {
                // add single cell in direction + 90 degrees
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i-1]);
                newCorridor.Add(corridor[i-1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i=1; i<corridor.Count; i++)
        {
            for (int x=-1; x<2; x++)
            {
                for (int y=-1; y<2; y++)
                {
                    newCorridor.Add(corridor[i-1] + new Vector2Int(x,y));
                }
            }
        }

        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right)
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero;
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var pos in floorPositions)
        {
            grid.GetTilemap(TilemapType.DungeonUnderground).SetTile(pos.x, pos.y, (int)GroundTileType.DungeonEntrance);
        }
    }

    // In future, maybe make entire tilemap full of walls, then generate corridors and rooms in between using rule tiles
    private void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapStructure tilemap)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach (var pos in basicWallPositions)
        {
            tilemap.SetTile(pos.x, pos.y, (int)GroundTileType.Cliff);
        }
    }

    private HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPos = pos + direction;
                if (floorPositions.Contains(neighbourPos) == false)
                {
                    // wall
                    wallPositions.Add(neighbourPos);
                }
            }
        }
        return wallPositions;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0,-1), //DOWN
        new Vector2Int(-1,0) //LEFT
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}
