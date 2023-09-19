using UnityEngine;

[CreateAssetMenu(fileName = "ExitGen", menuName = "Algorithms/ExitGen")]
public class ExitGen : AlgorithmBase
{
    [SerializeField]
    private BuildingTileType buildingType;

    public override void Apply(TilemapStructure tilemap)
    {
        // Generate inside
        tilemap.SetTile(tilemap.width / 2, 0, (int)buildingType, setDirty: false);
    }
}
