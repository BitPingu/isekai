using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/WallGenerator")]
public class WallGenerator : ScriptableObject
{
    // In future, maybe make entire tilemap full of walls, then generate corridors and rooms in between using rule tiles
    public void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach (var pos in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(pos);
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
