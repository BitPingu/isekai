using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    private void Update()
    {
        transform.position = target.transform.position + new Vector3(0, 0, -10);
    }
}
