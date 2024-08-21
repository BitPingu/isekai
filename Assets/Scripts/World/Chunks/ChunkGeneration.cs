using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    public GameObject chunk;
    private GameObject chu;

    public void Initialize(TileGrid grid, int width, int height)
    {
        // init structures
        GetComponent<VillageGeneration>().Initialize(grid);
        GetComponent<DungeonGeneration>().Initialize(grid);
        GetComponent<CampGeneration>().Initialize(grid);
        GetComponent<TreeGeneration>().Initialize(grid, width, height);

        Vector2Int currentPoint = new Vector2Int();
        int chunkSize = 16, pivotPoint = 0;

        while (currentPoint.x <= width-1 || currentPoint.y <= height-1)
        {
            if (currentPoint.x >= width-1 && currentPoint.y >= height-1)
            {
                break;
            } 
            else if (currentPoint.x >= width-1)
            {
                pivotPoint+=chunkSize;
                currentPoint.x = 0;
            }
            currentPoint.y = pivotPoint;

            // Divide into chunks
            chu = Instantiate(chunk, new Vector3(currentPoint.x, currentPoint.y), Quaternion.identity, transform);
            chu.GetComponent<BoxCollider2D>().size = new Vector2(chunkSize*3, chunkSize*3);
            chu.GetComponent<BoxCollider2D>().offset = new Vector2(chunkSize/2, chunkSize/2);

            for (int x=0; x<chunkSize; x++)
            {
                if (currentPoint.x > width-1)
                {
                    break;
                }
                currentPoint.y=pivotPoint;
                for (int y=0; y<chunkSize; y++)
                {
                    if (currentPoint.y > height-1)
                    {
                        break;
                    }

                    // Place structure at this position
                    GetComponent<VillageGeneration>().GetVillageStructure(currentPoint.x, currentPoint.y, chu);
                    GetComponent<DungeonGeneration>().GetDungeonStructure(currentPoint.x, currentPoint.y, chu);
                    GetComponent<CampGeneration>().GetCampStructure(currentPoint.x, currentPoint.y, chu);
                    GetComponent<TreeGeneration>().GetTree(currentPoint.x, currentPoint.y, chu);

                    currentPoint.y++;
                }
                currentPoint.x++;
            }
        }

    }
}
