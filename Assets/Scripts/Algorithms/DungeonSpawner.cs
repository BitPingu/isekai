using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSpawner", menuName = "Algorithms/DungeonSpawner")]
public class DungeonSpawner : AlgorithmBase
{
    [SerializeField]
    private PoissonDiscSampling algorithm;
    [SerializeField]
    private HashSet<Vector2> points;

    private int currentTile;

    private void Awake()
    {
        if (MainMenu.loadGame)
        {
            // Load dungeon data
            WorldData data = SaveSystem.LoadWorld();
            List<int> dungeonCoordsX = data.dungeonCoordsX;
            List<int> dungeonCoordsY = data.dungeonCoordsY;

            // Combine lists
            points = new HashSet<Vector2>();
            var combinedCoords = dungeonCoordsX.Zip(dungeonCoordsY, (x, y) => new { xCoord = x, yCoord = y });
            foreach (var coord in combinedCoords)
            {
                points.Add(new Vector2Int(coord.xCoord, coord.yCoord));
            }
        }
        else
        {
            // Determine spawn points using Poisson Disc Sampling
            List<Vector2> listPoints = new List<Vector2>();
            listPoints = algorithm.GeneratePoints();

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
                tilemap.SetTile((int)point.x, (int)point.y, (int)ObjectTileType.Dungeon);
            }
        }
    }
}
