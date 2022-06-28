using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInteraction : MonoBehaviour
{
    [SerializeField]
    private Tilemap objectMap;
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
        Vector3Int gridPosition;
        TileBase clickedTile;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gridPosition = objectMap.WorldToCell(mousePosition);
            clickedTile = objectMap.GetTile(gridPosition);
            if (clickedTile != null)
            {
                bool obstruct = dataFromTiles[clickedTile].obstruction;
                print("Coordinates: " + gridPosition + " | Tile: " + clickedTile + " | obstruct?: " + obstruct);
            }
        }

    }

    public bool tileObstruction(Vector2 worldPosition)
    {
        Vector3Int gridPosition;
        TileBase tile;

        gridPosition = objectMap.WorldToCell(worldPosition);
        tile = objectMap.GetTile(gridPosition);
        if (tile != null && dataFromTiles[tile].obstruction)
        {
            return true;
        }
        return false;
    }

}
