using System.Collections.Generic;
using UnityEngine;

public class FogData : MonoBehaviour
{
    public List<int> clearFogCoordsX, clearFogCoordsY;

    [SerializeField]
    private PlayerPosition player;

    private BoundsInt fogBounds;
    private TilemapStructure tilemap;

    private void Awake()
    {
        // Retrieve tilemap component
        tilemap = GetComponent<TilemapStructure>();

        // Attach delegates
        player.PosChange += ClearFog;
    }

    // Looks at area around player in a 10x10 square and clears fog
    public void ClearFog()
    {
        // Set bounds
        fogBounds.min = new Vector3Int(player.currentPos.x - 10, player.currentPos.y - 10, 0);
        fogBounds.max = new Vector3Int(player.currentPos.x + 10, player.currentPos.y + 10, 0);

        // Update fog
        for (int x = fogBounds.min.x; x < fogBounds.max.x; x++)
        {
            for (int y = fogBounds.min.y; y < fogBounds.max.y; y++)
            {
                tilemap.SetTile(x, y, (int)GroundTileType.Empty, true);
            }
        }
    }

    // Retrieves coordinates with no fog
    public void GetClearFog()
    {
        for (int x=0; x<tilemap.width; x++)
        {
            for (int y=0; y<tilemap.height; y++)
            {
                // Only add unique coords (using hashset)
                if (tilemap.GetTile(x, y) == (int)GroundTileType.Empty)
                {
                    clearFogCoordsX.Add(x);
                    clearFogCoordsY.Add(y);
                }
            }
        }
    }
}
