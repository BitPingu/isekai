using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Vector3Int spawnPoint;
    private int currentTile;

    [SerializeField]
    private TileGrid grid;
    private TilemapStructure groundMap;

    private void Awake()
    {
        if (MainMenu.loadGame)
        {
            // Load player position
            PlayerData data = SaveSystem.LoadPlayer();
            spawnPoint.x = data.savedPos[0];
            spawnPoint.y = data.savedPos[1];
        }
        else
        {
            // Retrieve tilemap component
            groundMap = grid.GetTilemap(TilemapType.Ground);

            do
            {
                // Choose random spawn point
                spawnPoint.x = Random.Range(0, grid.width);
                spawnPoint.y = Random.Range(0, grid.height);

                // Check tile
                currentTile = groundMap.GetTile(spawnPoint.x, spawnPoint.y);
            }
            while (currentTile != (int)GroundTileType.Land);
        }

        // Move to spawn point
        transform.position = spawnPoint;
    }
}
