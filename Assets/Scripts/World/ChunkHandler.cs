using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler : MonoBehaviour
{
    private GameObject player;
    public GameObject[] chunkParents;
    public float activationDistance = 50.0f;
    public float checkInterval = 1f; // modify to check only when player moves

    public void Initialize(GameObject p)
    {
        player = p;
        StartCoroutine(CheckChunkDistance());
    }

    IEnumerator CheckChunkDistance()
    {
        while (true)
        {
            foreach (GameObject chunkParent in chunkParents)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, chunkParent.transform.position);

                if (distanceToPlayer <= activationDistance)
                {
                    // Activate chunk close to player
                    chunkParent.SetActive(true);
                }
                else
                {
                    // Deactivate far away chunk
                    chunkParent.SetActive(false);
                }
            }

            yield return new WaitForSeconds(checkInterval);

        }
    }
}
