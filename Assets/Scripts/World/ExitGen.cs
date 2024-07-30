using UnityEngine;

[CreateAssetMenu(fileName = "ExitGen", menuName = "Algorithms/ExitGen")]
public class ExitGen
{
    public GameObject exit;

    public void Apply(TilemapStructure tilemap)
    {
        // Instantiate exit (temporary)
        tilemap.SetTile(tilemap.width/2, 0, (int)GroundTileType.Empty);
        Vector3 buildingPos = new Vector3(tilemap.width/2 + .5f, .5f);
        // GameObject newBuilding = Instantiate(exit, buildingPos, Quaternion.identity);
        // newBuilding.transform.parent = tilemap.gameObject.transform;
    }
}
