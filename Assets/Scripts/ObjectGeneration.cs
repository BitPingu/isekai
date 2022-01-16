using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName= "ObjectGeneration", menuName= "Algorithms/ObjectGeneration")]
public class ObjectGeneration : AlgorithmBase
{
    [SerializeField]
    private ObjectConfiguration[] ObjectSelection;

    [Serializable]
    class ObjectConfiguration
    {
        public ObjectTileType Object;
        public GroundTileType[] SpawnOnGrounds;
        [Range(0, 100)]
        public int SpawnChancePerCell;
    }

    public override void Apply(TilemapStructure tilemap)
    {
        var groundTilemap = tilemap.Grid.Tilemaps[TilemapType.Ground];
        var random = new System.Random(tilemap.Grid.Seed);
        for (int x=0; x<tilemap.Width; x++)
        {
            for (int y=0; y<tilemap.Height; y++)
            {
                foreach (var Object in ObjectSelection)
                {
                    var groundTile = groundTilemap.GetTile(x, y);
                    if (Object.SpawnOnGrounds.Any(tile => (int)tile == groundTile))
                    {
                        // Do a random chance check
                        if (random.Next(0, 100) <= Object.SpawnChancePerCell)
                        {
                            tilemap.SetTile(x, y, (int)Object.Object);
                        }
                    }
                }
            }
        }
    }
}
