using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    [SerializeField]
    public TileInfo[] Tiles;

    [Serializable]
    public class TileInfo
    {
        public TileBase tile;
        public float walkingSpeed;
    }
}
