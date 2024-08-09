using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkMapGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPos = Vector2Int.zero;

    [SerializeField]
    private int iterations = 10;
    [SerializeField]
    public int walkLen = 10;
    [SerializeField]
    public bool startRandomlyEachIter = true;

    public RandomWalk walk;
    public TilemapVisualizer tilemapVisualizer;

    private void Awake()
    {
        RunProceduralGeneration();
    }

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPos = startPos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        for (int i=0; i<iterations; i++)
        {
            var path = walk.SimpleRandomWalk(currentPos, walkLen);
            floorPos.UnionWith(path);
            if (startRandomlyEachIter)
                currentPos = floorPos.ElementAt(Random.Range(0,floorPos.Count));
        }
        return floorPos;
    }
}
