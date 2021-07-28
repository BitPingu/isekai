using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceWater", menuName = "Algorithms/PlaceWater")]
public class PlaceWater : AlgorithmBase
{
    public List<int> x, y, value;
    public override void Apply(TilemapStructure tilemap)
    {
        for (int i=0; i<x.Count; i++)
        {
            tilemap.SetTile(x[i], y[i], value[i]);
        }
        x.Clear();
        y.Clear();
        value.Clear();
    }
}
