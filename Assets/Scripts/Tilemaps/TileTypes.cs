using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTypes
{
    [Serializable]
    public class GroundTiles : TileData<GroundTileType> { }

    [Serializable]
    public class FoilageTiles : TileData<FoilageTileType> { }

    [Serializable]
    public class BuildingTiles : TileData<BuildingTileType> { }

    public abstract class TileData<T> : TileData
        where T : Enum

    {
        public T tileType;
        public override int tileTypeId { get { return Convert.ToInt32(tileType); } }
    }

    public abstract class TileData
    {
        public Sprite sprite;
        public TileBase tile;
        public virtual int tileTypeId { get; }
    }
}
