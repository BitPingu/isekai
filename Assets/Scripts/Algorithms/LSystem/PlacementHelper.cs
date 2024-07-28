using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirections = new List<Direction>();
        if (collection.Contains(position + Vector3Int.right))
        {
            // if road to right
            neighbourDirections.Add(Direction.Right);
        }
        if (collection.Contains(position - Vector3Int.right))
        {
            // if road to left
            neighbourDirections.Add(Direction.Left);
        }
        if (collection.Contains(position + Vector3Int.up))
        {
            // if road to up 
            neighbourDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - Vector3Int.down))
        {
            // if road to down
            neighbourDirections.Add(Direction.Down);
        }
        return neighbourDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3Int.up;
            case Direction.Down:
                return Vector3Int.down;
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("no such direction as " + direction);
    }
}
