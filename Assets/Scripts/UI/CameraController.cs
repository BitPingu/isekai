using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public int xOffset, yOffset;

    private void Start()
    {
        if (target)
            transform.position = target.transform.position + new Vector3(xOffset, yOffset, -10);
    }

    // Update is called once per frame
    private void Update()
    {
        // if (target)
        //     transform.position = target.transform.position + new Vector3(xOffset, yOffset, -10);
    }
}
