using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSpawner", menuName = "Algorithms/BuildingSpawner")]
public class BuildingSpawner : AlgorithmBase
{
    //[SerializeField]
    //private BuildingTypes.BuildingData[] buildings;
    [SerializeField]
    private BuildingTileType buildingType;
    [SerializeField]
    private PoissonDiscSampling algorithm;

    [SerializeField]
    private HashSet<Vector2> points;
    private int currentTile;

    private void Awake()
    {
        //Debug.Log("awake");
        if (MainMenu.loadGame)
        {
            //Debug.Log("load");
            switch ((int)buildingType)
            {
                case (int)BuildingTileType.House:
                    // Load village data
                    List<int> villageCoordsX = SaveSystem.LoadWorld().savedVillageCoordsX;
                    List<int> villageCoordsY = SaveSystem.LoadWorld().savedVillageCoordsY;

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
                    List<int> dungeonCoordsX = SaveSystem.LoadWorld().savedDungeonCoordsX;
                    List<int> dungeonCoordsY = SaveSystem.LoadWorld().savedDungeonCoordsY;

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
        else
        {
            //Debug.Log("generate");
            // Determine spawn points using Poisson Disc Sampling
            List<Vector2> listPoints = algorithm.GeneratePoints();

            // Convert to hash set
            points = new HashSet<Vector2>(listPoints);
        }
    }

    public override void Apply(TilemapStructure tilemap)
    {
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
}
