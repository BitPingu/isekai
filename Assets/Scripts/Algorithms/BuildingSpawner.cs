using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSpawner", menuName = "Algorithms/BuildingSpawner")]
public class BuildingSpawner : AlgorithmBase
{
    [SerializeField]
    private PoissonDiscSampling algorithm;

    [SerializeField]
    private HashSet<Vector2> points;
    private int currentTile;

    public GameObject building;
    public List<int> villageCoordsX;
    public List<int> villageCoordsY;
    public List<int> dungeonCoordsX;
    public List<int> dungeonCoordsY;


    public override void Apply(TilemapStructure tilemap)
    {
        LoadPoints();
        var groundTilemap = tilemap.grid.GetTilemap(TilemapType.Ground);

        foreach (Vector2 point in points)
        {
            // Check tile
            currentTile = groundTilemap.GetTile((int)point.x, (int)point.y);

            // Check valid tile
            if (currentTile == (int)GroundTileType.Land)
            {
                // Instantiate building
                tilemap.SetTile((int)point.x, (int)point.y, (int)GroundTileType.Empty);
                Vector3 buildingPos = new Vector3((int)point.x + .5f, (int)point.y + .5f);
                GameObject newBuilding = Instantiate(building, buildingPos, Quaternion.identity);
                newBuilding.transform.parent = tilemap.gameObject.transform;
            }
        }
    }

    private void LoadPoints()
    {
        if (TempData.initBuilding)
        {
            if (TempData.newGame)
            {
                // Determine spawn points using Poisson Disc Sampling
                List<Vector2> listPoints = algorithm.GeneratePoints();

                // Convert to hash set
                points = new HashSet<Vector2>(listPoints);
            }
            else
            {
                SaveData saveData = SaveSystem.Load();
                if (building.name.Contains("Village"))
                {
                    // Load village data
                    villageCoordsX = saveData.saveVillageCoordsX;
                    villageCoordsY = saveData.saveVillageCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedVillageCoords = villageCoordsX.Zip(villageCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedVillageCoords)
                    {
                        points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                    }
                }
                else if (building.name.Contains("Dungeon"))
                {
                    // Load dungeon data
                    dungeonCoordsX = saveData.saveDungeonCoordsX;
                    dungeonCoordsY = saveData.saveDungeonCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedDungeonCoords = dungeonCoordsX.Zip(dungeonCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedDungeonCoords)
                    {
                        points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                    }
                }
                else
                {
                    Debug.Log("cannot load building :()");
                }
            }
        }
        else
        {
            if (building.name.Contains("Village"))
            {
                // Load village data
                List<int> villageCoordsX = new List<int>();
                List<int> villageCoordsY = new List<int>();
                foreach (Vector3 village in TempData.tempVillages)
                {
                    villageCoordsX.Add((int)village.x);
                    villageCoordsY.Add((int)village.y);
                }

                // Combine lists
                points = new HashSet<Vector2>();
                var combinedVillageCoords = villageCoordsX.Zip(villageCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                foreach (var coord in combinedVillageCoords)
                {
                    points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                }
            }
            else if (building.name.Contains("Dungeon"))
            {
                 // Load dungeon data
                List<int> dungeonCoordsX = new List<int>();
                List<int> dungeonCoordsY = new List<int>();
                foreach (Vector3 dungeon in TempData.tempDungeons)
                {
                    dungeonCoordsX.Add((int)dungeon.x);
                    dungeonCoordsY.Add((int)dungeon.y);
                }

                // Combine lists
                points = new HashSet<Vector2>();
                var combinedDungeonCoords = dungeonCoordsX.Zip(dungeonCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                foreach (var coord in combinedDungeonCoords)
                {
                    points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                }
            }
            else
            {
                Debug.Log("cannot load building2 :()");
            }
        }
    }
}
