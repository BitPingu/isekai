using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypes
{
    [SerializeField]
    public BuildingData[] Buildings;

    [Serializable]
    public class BuildingData
    {
        public BuildingTileType building;
        public List<int> coordsX, coordsY;
    }
}
