using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeGeneration", menuName = "Algorithms/TreeGeneration")]
public class TreeGeneration : AlgorithmBase
{
    [SerializeField]
    private TreeConfiguration[] TreeSelection;

    [Serializable]
    private class TreeConfiguration
    {
        public ObjectTileType Tree;
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
                foreach (var tree in TreeSelection)
                {
                    var groundTile = groundTilemap.GetTile(x, y);
                    if (tree.SpawnOnGrounds.Any(tile => (int)tile == groundTile))
                    {
                        // Random chance check
                        if (random.Next(0, 100) <= tree.SpawnChancePerCell)
                        {
                            tilemap.SetTile(x, y, (int)tree.Tree);
                        }
                    }
                }
            }
        }
    }
}
