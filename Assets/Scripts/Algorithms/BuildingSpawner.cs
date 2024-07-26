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

    public override void Apply(TilemapStructure tilemap)
    {
        LoadPoints();
        var groundTilemap = tilemap.grid.GetTilemap(TilemapType.Ground);
        var foilageTilemap = tilemap.grid.GetTilemap(TilemapType.Overworld);

        foreach (Vector2 point in points)
        {
            // Check tile
            currentTile = groundTilemap.GetTile((int)point.x, (int)point.y);

            // Check valid tile
            if (currentTile == (int)GroundTileType.Land)
            {
                // Clear foilage
                foilageTilemap.SetTile((int)point.x, (int)point.y, (int)GroundTileType.Empty);

                if (building.gameObject.name.Equals("Camp"))
                {
                    // Clear area for camp
                    if (groundTilemap.GetTile((int)point.x+1, (int)point.y) == (int)GroundTileType.Land)
                    {
                        foilageTilemap.SetTile((int)point.x+1, (int)point.y, (int)FoilageTileType.Empty);
                    }
                    if (groundTilemap.GetTile((int)point.x-1, (int)point.y) == (int)GroundTileType.Land)
                    {
                        foilageTilemap.SetTile((int)point.x-1, (int)point.y, (int)FoilageTileType.Empty);
                    }
                }

                // Instantiate building
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
                    List<int> villageCoordsX = saveData.saveVillageCoordsX;
                    List<int> villageCoordsY = saveData.saveVillageCoordsY;

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
                    List<int> dungeonCoordsX = saveData.saveDungeonCoordsX;
                    List<int> dungeonCoordsY = saveData.saveDungeonCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedDungeonCoords = dungeonCoordsX.Zip(dungeonCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedDungeonCoords)
                    {
                        points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                    }
                }
                else if (building.name.Contains("Camp"))
                {
                    // Load camp data
                    List<int> campCoordsX = saveData.saveCampCoordsX;
                    List<int> campCoordsY = saveData.saveCampCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedCampCoords = campCoordsX.Zip(campCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedCampCoords)
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
            else if (building.name.Contains("Camp"))
            {
                // Load camp data
                List<int> campCoordsX = new List<int>();
                List<int> campCoordsY = new List<int>();
                foreach (Vector3 camp in TempData.tempCamps)
                {
                    campCoordsX.Add((int)camp.x);
                    campCoordsY.Add((int)camp.y);
                }

                // Combine lists
                points = new HashSet<Vector2>();
                var combinedCampCoords = campCoordsX.Zip(campCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                foreach (var coord in combinedCampCoords)
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
