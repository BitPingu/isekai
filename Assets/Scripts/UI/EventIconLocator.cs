using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventIconLocator : MonoBehaviour
{
    public static EventIconLocator icon;

    private void Awake()
    {
        // if (icon == null)
        // {
        //     Debug.Log("set");
        //     icon = this;
        // } 
        // else 
        // {
        //     Debug.Log("destroy");
        //     Destroy(gameObject);
        //     return;
        // }
        // if (FindObjectOfType<EventIconLocator>() != null)
        // {
        //     Debug.Log("exists");
        //     Destroy(gameObject);
        // }
    }
}
