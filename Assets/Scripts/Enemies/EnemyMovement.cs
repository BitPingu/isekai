using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private void Awake()
    {
        // Goblin spawn point
        transform.position = new Vector3(Noise.halfWidth + 1.5f, Noise.halfHeight + 10.6f, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
