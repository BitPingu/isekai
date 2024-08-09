using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    public Transform target;

    public void Initialize(Transform targetPos)
    {
        target = targetPos;
    }

    private void Update()
    {
        if (target)
            transform.position = new Vector3(target.position.x, target.position.y + 5);
    }
}
