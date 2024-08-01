using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    private List<Vector2> dunPoints = new List<Vector2>();

    private TilemapStructure groundMap;
    public GameObject dungeon;

    public void Initialize(TilemapStructure tilemap)
    {
        // Get tilemap structure
        groundMap = tilemap;

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
            dunPoints = sampling.GeneratePoints(tilemap);
        }

        // Save dun data
        TempData.tempDungeons = dunPoints;

        // Generate dungeons
        foreach (Vector2 point in dunPoints)
        {
            // Skip water coords
            if (groundMap.GetTile(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)) == (int)GroundTileType.Water)
                continue;

            // Set dungeon centerpoint
            // vilCenter = new Vector3(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));

            // Generate procedural dungeon area here saved on another tilemap

            // Spawn dungeon
            Instantiate(dungeon, new Vector3(point.x+.5f, point.y+.5f), Quaternion.identity, transform);
        }
    }
}
