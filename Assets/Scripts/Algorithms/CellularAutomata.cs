using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="CellularAutomata", menuName="Algorithms/CellularAutomata")]
public class CellularAutomata : AlgorithmBase
{
    public int minAlive, repititions;

    [Tooltip("If this is checked, ReplacedBy will have no effect.")]
    public bool replaceByDominantTile;

    public ObjectTileType targetTile, replacedBy;

    public override void Apply(TilemapStructure tilemap)
    {
        int targetTileId = (int)targetTile;
        int replaceTileId = (int)replacedBy;
        for (int i = 0; i < repititions; i++)
        {
            for (int x = 0; x < tilemap.width; x++)
            {
                for (int y = 0; y < tilemap.height; y++)
                {
                    // Check if the current tile is target tile
                    var tile = tilemap.GetTile(x, y);
                    if (tile == targetTileId)
                    {
                        // Retrieve all 8 neighbors of current tile
                        var neighbors = tilemap.GetNeighbors(x, y);

                        // Count all the neightbors that are of type target tile
                        int targetTilesCount = neighbors.Count(a => a.Value == targetTileId);

                        // If the min alive count is not reached, replace the tile
                        if (targetTilesCount < minAlive)
                        {
                            if (replaceByDominantTile)
                            {
                                // Group tiles on tiletype, then order them in descending order based on group size
                                // Select the group's key which is the tiletype because that's what is grouped on
                                // And select the first one (first group's key) because that's the dominant tile type 
                                var dominantTile = neighbors
                                    .GroupBy(a => a.Value)
                                    .OrderByDescending(a => a.Count())
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
