using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSpawner", menuName = "Algorithms/BuildingSpawner")]
public class BuildingSpawner : AlgorithmBase
{
    [SerializeField]
    private BuildingTileType buildingType;
    [SerializeField]
    private PoissonDiscSampling algorithm;

    [SerializeField]
    private HashSet<Vector2> points;
    private int currentTile;

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
                tilemap.SetTile((int)point.x, (int)point.y, (int)buildingType);
            }
        }
    }

    private void LoadPoints()
    {
        if (TempData.initBuilding)
        {
            if (TempData.newGame)
            {
                // Debug.Log("new building");
                // Determine spawn points using Poisson Disc Sampling
                List<Vector2> listPoints = algorithm.GeneratePoints();

                // Convert to hash set
                points = new HashSet<Vector2>(listPoints);
            }
            else
            {
                SaveData saveData = SaveSystem.Load();
                switch ((int)buildingType)
                {
                    case (int)BuildingTileType.House:
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
                        break;
                    case (int)BuildingTileType.Dungeon:
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
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            // Debug.Log("load building");
            switch ((int)buildingType)
            {
                case (int)BuildingTileType.House:
                    // Load village data
                    List<int> villageCoordsX = TempData.tempBuilding.villageCoordsX;
                    List<int> villageCoordsY = TempData.tempBuilding.villageCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedVillageCoords = villageCoordsX.Zip(villageCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedVillageCoords)
                    {
                        points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                    }
                    break;
                case (int)BuildingTileType.Dungeon:
                    // Load dungeon data
                    List<int> dungeonCoordsX = TempData.tempBuilding.dungeonCoordsX;
                    List<int> dungeonCoordsY = TempData.tempBuilding.dungeonCoordsY;

                    // Combine lists
                    points = new HashSet<Vector2>();
                    var combinedDungeonCoords = dungeonCoordsX.Zip(dungeonCoordsY, (x, y) => new { xCoord = x, yCoord = y });
                    foreach (var coord in combinedDungeonCoords)
                    {
                        points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
