using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class CorridorFirstDungeonGeneration : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLen = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = .8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        // Create corridors
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        // Create rooms
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // Dead ends
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        // Merge created corridors with rooms
        floorPositions.UnionWith(roomPositions);

        // Increase corridor size
        for (int i=0; i<corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            // corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        wall.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var pos in deadEnds)
        {
            // Check if no room is touching
            if (roomFloors.Contains(pos) == false)
            {
                // Create room
                var room = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(room);
            }
        }
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

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        // sort rooms by unique id
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPos in roomsToCreate)
        {
            // generate room
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPos = startPos;
        potentialRoomPositions.Add(currentPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i=0; i<corridorCount; i++)
        {
            var corridor = walk.RandomWalkCorridor(currentPos, corridorLen);
            corridors.Add(corridor);
            currentPos = corridor[corridor.Count-1];
            potentialRoomPositions.Add(currentPos);
            floorPositions.UnionWith(corridor);
        }

        return corridors;
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
}
