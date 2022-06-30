using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName= "ObjectGeneration", menuName= "Algorithms/ObjectGeneration")]
public class ObjectGeneration : AlgorithmBase
{
    [SerializeField]
    private ObjectConfiguration[] objectSelection;

    [Serializable]
    class ObjectConfiguration
    {
        public ObjectTileType obj;
        public GroundTileType[] spawnOnGrounds;
        [Range(0, 100)]
        public int spawnChancePerCell;
    }

    public override void Apply(TilemapStructure tilemap)
    {
        var groundTilemap = tilemap.grid.GetTilemap(TilemapType.Ground);
        var random = new System.Random(tilemap.grid.seed);
        for (int x=0; x<tilemap.width; x++)
        {
            for (int y=0; y<tilemap.height; y++)
            {
                foreach (var obj in objectSelection)
                {
                    var groundTile = groundTilemap.GetTile(x, y);
                    if (obj.spawnOnGrounds.Any(tile => (int)tile == groundTile))
                    {
                        // Do a random chance check
                        if (random.Next(0, 100) <= obj.spawnChancePerCell)
                        {
                            tilemap.SetTile(x, y, (int)obj.obj, setDirty: false);
                        }
                    }
                }
            }
        }
    }
}
