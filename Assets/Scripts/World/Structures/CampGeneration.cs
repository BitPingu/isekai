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

        if (TempData.loadGame)
        {
            // Load camp points
            List<int> campCoordsX = SaveSystem.Load().saveCampCoordsX;
            List<int> campCoordsY = SaveSystem.Load().saveCampCoordsY;

            for (int i=0; i<campCoordsX.Count; i++)
            {
                campPoints.Add(new Vector2(campCoordsX[i], campCoordsY[i]));
            }
        }
        else
        {
            // Generate camp coords
            campPoints = sampling.GeneratePoints(tilemap);
        }

        // Save camp points
        TempData.tempCamps = campPoints;

        // Generate camps
        foreach (Vector2 point in campPoints)
        {
            // Skip water coords
            if (groundMap.GetTile(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)) == (int)GroundTileType.Sea)
                continue;

            // Set dungeon centerpoint
            // vilCenter = new Vector3(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));

            // Generate procedural dungeon area here saved on another tilemap

            // Spawn camp
            Instantiate(camp, new Vector3(point.x+.5f, point.y+.5f), Quaternion.identity, transform);
        }
    }
}
