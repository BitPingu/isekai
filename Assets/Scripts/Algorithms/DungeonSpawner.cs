using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSpawner", menuName = "Algorithms/DungeonSpawner")]
public class DungeonSpawner : AlgorithmBase
{
    [SerializeField]
    private PoissonDiscSampling algorithm;
    [SerializeField]
    private List<Vector2> points;

    private int currentTile;

    private void Awake()
    {
        // Determine spawn points using Poisson Disc Sampling
        points = algorithm.GeneratePoints();
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
