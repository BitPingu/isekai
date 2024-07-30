using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGeneration : MonoBehaviour
{
    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    private List<Vector2> campPoints = new List<Vector2>();

    private TilemapStructure groundMap;
    public GameObject camp;

    public void Initialize(TilemapStructure tilemap)
    {
        // Get tilemap structure
        groundMap = tilemap;

        // Generate dungeon coords
        campPoints = sampling.GeneratePoints(tilemap);

        // Generate dungeons
        foreach (Vector2 point in campPoints)
        {
            // Skip water coords
            if (groundMap.GetTile(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)) == (int)GroundTileType.Water)
                continue;

            // Set dungeon centerpoint
            // vilCenter = new Vector3(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));

            // Generate procedural dungeon area here saved on another tilemap

            // Spawn dungeon
            Instantiate(camp, new Vector3(point.x+.5f, point.y+.5f), Quaternion.identity, transform);
        }
    }
}
