using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon
{
    public Vector2 dunCenter;
    public List<Room> rooms = new List<Room>();

    public Dungeon(Vector2 center)
    {
        dunCenter = center;
    }
}

public class Room
{
    public Vector2Int center;
    public HashSet<Vector2Int> floor;
    public string type;

    public Room(Vector2Int c, HashSet<Vector2Int> f)
    {
        center = c;
        floor = f;
    }
}

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField]
    private PoissonDiscSamplingGenerator sampling;
    [SerializeField]
    private CorridorFirstDungeonGeneration generator;
    private List<Dungeon> dungeons = new List<Dungeon>();

    private TileGrid grid;
    public GameObject dungeonEntrance;

    // public GameObject renderCanvas;
    // public Text textPrefab;

    private List<GameObject> structures = new List<GameObject>();

    public void Initialize(TileGrid g)
    {
        // Get grid
        grid = g; 

        if (TempData.loadGame)
        {
            // Load dungeon data
            List<int> dungeonCoordsX = SaveSystem.Load().saveDungeonCoordsX;
            List<int> dungeonCoordsY = SaveSystem.Load().saveDungeonCoordsY;

            for (int i=0; i<dungeonCoordsX.Count; i++)
            {
                dungeons.Add(new Dungeon(new Vector2(dungeonCoordsX[i], dungeonCoordsY[i])));
            }
        }
        else
        {
            // Generate dungeon coords
            List<Vector2> dunPoints = sampling.GeneratePoints(grid.GetTilemap(TilemapType.Ground));

            // Generate dungeons
            int dungeonCount = 0;
            for (int i=0; i<dunPoints.Count; i++)
            {
                // Skip obstructed coords
                if (!grid.CheckLand(dunPoints[i]) || !grid.CheckCliff(dunPoints[i]))
                {
                    continue;
                }

                // Add new dungeon to list
                dungeons.Add(new Dungeon(dunPoints[i]));
                dungeonCount++;

                // if (dungeonCount == 1)
                    // break;
            }            
        }

        // Save dun data
        TempData.tempDungeons = dungeons;

        // Prep dungeon underground
        for (int x = 0; x < grid.GetTilemap(TilemapType.DungeonUnderground).width; x++)
        {
            for (int y = 0; y < grid.GetTilemap(TilemapType.DungeonUnderground).height; y++)
            {
                grid.GetTilemap(TilemapType.DungeonUnderground).SetTile(x, y, (int)GroundTileType.Fog, setDirty: false);
            }
        }

        // Generate dungeons
        foreach (Dungeon dun in dungeons)
        {
            // Place dungeon entrance in overworld
            SpawnDungeonEntrance(dun.dunCenter);

            // Generate dungeon
            generator.Initialize(grid, new Vector2Int((int)dun.dunCenter.x, (int)dun.dunCenter.y), dun.rooms);
        }

        // Render tiles
        grid.GetTilemap(TilemapType.Dungeon).UpdateTiles();
        grid.GetTilemap(TilemapType.DungeonUnderground).UpdateTiles();
    }

    private void SpawnDungeonEntrance(Vector2 centerPos)
    {
        // Entrance floor
        for (int i=Mathf.FloorToInt(centerPos.x)-2; i<=Mathf.FloorToInt(centerPos.x)+2; i++)
        {
            for (int j=Mathf.FloorToInt(centerPos.y)-2; j<=Mathf.FloorToInt(centerPos.y)+2; j++)
            {
                if (i == Mathf.FloorToInt(centerPos.x)-2 || i == Mathf.FloorToInt(centerPos.x)+2 || j == Mathf.FloorToInt(centerPos.y)-2 || j == Mathf.FloorToInt(centerPos.y)+2)
                {
                    var random = TempData.tempRandom;
                    if (random.Next(0,2) == 1)
                    {
                        grid.GetTilemap(TilemapType.Dungeon).SetTile(i, j, (int)GroundTileType.DungeonEntrance, setDirty : false);
                    }
                }
                else
                {
                    grid.GetTilemap(TilemapType.Dungeon).SetTile(i, j, (int)GroundTileType.DungeonEntrance, setDirty : false);
                }
            }
        }

        // Spawn dungeon entrance
        var d = Instantiate(dungeonEntrance, new Vector3(centerPos.x+.5f, centerPos.y), Quaternion.identity, transform);
        d.SetActive(false);
        structures.Add(d);
    }

    public void GetDungeonStructure(int x, int y, GameObject chu)
    {
        foreach (GameObject structure in structures)
        {
            if (Vector3Int.FloorToInt(structure.transform.position) == new Vector3(x,y))
            {
                structure.transform.parent = chu.transform;
                break;
            }
        }
    }
}
