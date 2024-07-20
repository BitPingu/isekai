using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FogData : MonoBehaviour
{
    public List<int> clearFogCoordsX = new List<int>();
    public List<int> clearFogCoordsY = new List<int>();

    private BoundsInt fogBounds;
    private TilemapStructure tilemap;
    private PlayerPosition player;

    private void Awake()
    {
        // Retrieve tilemap and player components
        tilemap = GetComponent<TilemapStructure>();
        player = FindObjectOfType<PlayerPosition>();

        if (TempData.initFog)
        {
            TempData.initFog = false;
        }
    }

    private void OnEnable()
    {
        // Attach delegates
        player.PosChange += ClearFog;
    }

    private void OnDisable()
    {
        // Detatch delegates
        player.PosChange -= ClearFog;
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
    public void SaveFog()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
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
            TempData.tempFog = this;
        }
    }
}
