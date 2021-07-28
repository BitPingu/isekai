using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CellularAutomata", menuName = "Algorithms/CellularAutomata")]
public class CellularAutomata : AlgorithmBase
{
    public int MinAlive, Repititions;

    [Tooltip("If this is checked, ReplacedBy will have no effect.")]
    public bool ReplaceByDorminantTile;

    public ObjectTileType TargetTile, ReplacedBy;

    public override void Apply(TilemapStructure tilemap)
    {
        int targetTileId = (int)TargetTile;
        int replaceTileId = (int)ReplacedBy;
        for (int i=0; i<Repititions; i++)
        {
            for (int x=0; x<tilemap.Width; x++)
            {
                for (int y=0; y<tilemap.Height; y++)
                {
                    // Check if current tile is target tile
                    var tile = tilemap.GetTile(x, y);
                    if (tile == targetTileId)
                    {
                        // Retrieve all 8 neighbors of current tile 
                        var neighbors = tilemap.GetNeighbors(x, y);

                        // Count all the neighbors that are of type target tile
                        int targetTilesCount = neighbors.Count(a => a.Value == targetTileId);

                        // If min alive count is not reached, replace tile
                        if (targetTilesCount < MinAlive)
                        {
                            if (ReplaceByDorminantTile)
                            {
                                // Group tiles on tiletype, descending order based on group size
                                // Select first group's key (dominant tile type)
                                var dominantTile = neighbors
                                    .GroupBy(a => a.Value).OrderByDescending(a => a.Count())
                                    .Select(a => a.Key)
                                    .First();

                                tilemap.SetTile(x, y, dominantTile);
                            }
                            else
                            {
                                tilemap.SetTile(x, y, replaceTileId);
                            }
                        }
                    }
                }
            }
        }
    }
}
