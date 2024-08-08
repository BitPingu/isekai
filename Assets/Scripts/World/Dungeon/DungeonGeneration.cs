using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    private List<Vector2> dunPoints = new List<Vector2>();

    public GameObject dungeon;

    public void Initialize(WorldGeneration world, TileGrid grid)
    {
        if (TempData.loadGame)
        {
            // Load dungeon data
            List<int> dungeonCoordsX = SaveSystem.Load().saveDungeonCoordsX;
            List<int> dungeonCoordsY = SaveSystem.Load().saveDungeonCoordsY;

            for (int i=0; i<dungeonCoordsX.Count; i++)
            {
                dunPoints.Add(new Vector2(dungeonCoordsX[i], dungeonCoordsY[i]));
            }
        }
        else
        {
            // Generate dungeon coords
            dunPoints = sampling.GeneratePoints(grid.GetTilemap(TilemapType.Ground));
        }

        // Save dun data
        TempData.tempDungeons = dunPoints;

        // Generate dungeons
        foreach (Vector2 point in dunPoints)
        {
            // Check if safe to spawn
            if (!grid.CheckLand(new Vector2(point.x, point.y)) || !grid.CheckCliff(new Vector2(point.x, point.y)))
                continue;

            // Set dungeon centerpoint
            // vilCenter = new Vector3(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));

            // Generate procedural dungeon area here saved on another tilemap (but still in same scene)

            // Place dungeon entrance in overworld
            for (int i=Mathf.FloorToInt(point.x)-2; i<=Mathf.FloorToInt(point.x)+2; i++)
            {
                for (int j=Mathf.FloorToInt(point.y)-2; j<=Mathf.FloorToInt(point.y)+2; j++)
                {
                    if (i == Mathf.FloorToInt(point.x)-2 || i == Mathf.FloorToInt(point.x)+2 || j == Mathf.FloorToInt(point.y)-2 || j == Mathf.FloorToInt(point.y)+2)
                    {
                        var random = TempData.tempRandom;
                        if (random.Next(0,2) == 1)
                        {
                            grid.GetTilemap(TilemapType.Dungeon).SetTile(i, j, (int)GroundTileType.DungeonEntrance, setDirty : false);
                        }
                    }
                    else
                    {
                        grid.GetTilemap(TilemapType.Dungeon).SetTile(i, j, (int)GroundTileType.DungeonEntrance, setDirty : false);
                    }
                }
            }

            // Spawn dungeon
            Instantiate(dungeon, new Vector3(point.x+.5f, point.y), Quaternion.identity, transform);
        }

        // Render tiles
        grid.GetTilemap(TilemapType.Dungeon).UpdateTiles();
    }
}
