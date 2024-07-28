using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureHelper : MonoBehaviour
{
    public HouseType[] buildingTypes;
    public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach (var freeSpot in freeEstateSpots)
        {
            if (blockedPositions.Contains(freeSpot.Key))
            {
                // for big structures
                continue;
            }
            for (int i=0; i<buildingTypes.Length; i++)
            {
                // no limit for number of buildings of this type
                if (buildingTypes[i].quantity == -1)
                {
                    // Spawn building
                    var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key);
                    structuresDictionary.Add(freeSpot.Key, building);
                    break;
                }
                // check if more buildings to place of this type
                if (buildingTypes[i].IsBuildingAvailable())
                {
                    if (buildingTypes[i].sizeRequired > 1)
                    {
                        // big structure
                        var halfSize = Mathf.CeilToInt(buildingTypes[i].sizeRequired/2.0f);
                        List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                        if (VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
                        {
                            blockedPositions.AddRange(tempPositionsBlocked);

                            // Spawn building
                            var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key);

                            // Add to dictionary
                            structuresDictionary.Add(freeSpot.Key, building);
                            foreach (var pos in tempPositionsBlocked)
                            {
                                // point blocked pos to big structure
                                structuresDictionary.Add(pos, building);
                            }
                        } 
                    }
                    else
                    {
                        // Spawn building
                        var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key);

                        // Add to dictionary
                        structuresDictionary.Add(freeSpot.Key, building);
                    }
                    break;
                }
            }
        }
    }

    private bool VerifyIfBuildingFits(
        int halfSize, 
        Dictionary<Vector3Int, Direction> freeEstateSpots, 
        KeyValuePair<Vector3Int, Direction> freeSpot,
        List<Vector3Int> blockedPositions,
        ref List<Vector3Int> tempPositionsBlocked)
    {
        // Debug.Log("buildingfit at " + freeSpot.Key);
        Vector3Int direction = Vector3Int.zero;
        if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            // road is up or down
            direction = Vector3Int.right;
        }
        else
        {
            // road is right or left
            direction = Vector3Int.up;
        }
        // i=0 is current pos
        for (int i=1; i<=halfSize; i++)
        {
            // pos right
            var pos1 = freeSpot.Key + direction * i;
            // pos left
            var pos2 = freeSpot.Key - direction * i;
            // check free position for big structures
            if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2)
                || blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
            {
                return false;
            }
            // reserve space for big structure
            tempPositionsBlocked.Add(pos1);
            tempPositionsBlocked.Add(pos2);
        }
        return true;

    }

    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position)
    {
        var newStructure = Instantiate(prefab, position, Quaternion.identity, transform);
        return newStructure;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            // Debug.Log("current road pos: " + position);
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
            foreach (var dir in neighbourDirections)
            {
                // Debug.Log("get neighdirs: " + dir);
            }
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (neighbourDirections.Contains(direction) == false)
                {
                    // Debug.Log("get offset from dir: " + direction);
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    // Debug.Log("free spot at "+ newPosition);
                    freeSpaces.Add(newPosition, Direction.Right);
                }
            }
        }
        return freeSpaces;
    }
}
