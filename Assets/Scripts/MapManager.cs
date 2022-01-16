using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] maps;
    [SerializeField]
    private TileData tileData;

    private Dictionary<TileBase, TileData.TileInfo> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData.TileInfo>();

        foreach (var tile in tileData.Tiles)
        {
            dataFromTiles.Add(tile.tile, tile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int gridPosition = Vector3Int.zero;
        TileBase clickedTile = null;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            foreach (Tilemap map in maps)
            {
                gridPosition = map.WorldToCell(mousePosition);
                clickedTile = map.GetTile(gridPosition);
                if (clickedTile != null)
                {
                    float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;
                    print(gridPosition + ": " + clickedTile + " | " + walkingSpeed);
                }
            }
        }

    }

    public float GetTileWalkingSpeed(Vector2 worldPosition)
    {
        Vector3Int gridPosition;
        TileBase tile = null;

        foreach (Tilemap map in maps)
        {
            gridPosition = map.WorldToCell(worldPosition);
            tile = map.GetTile(gridPosition);
            if (tile != null)
            {
                float walkingSpeed = dataFromTiles[tile].walkingSpeed;
                return walkingSpeed;
            }
        }
        return 0f;
    }

}
