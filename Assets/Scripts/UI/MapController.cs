using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize, size; // 1 1 50 50

    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        enabled = false;
    }

    private void Update()
    {
        PanCamera();
    }

    private void OnEnable()
    {
        // Reset size
        cam.orthographicSize = size;   
    }

    private void PanCamera()
    {
        // Save position of mouse in world space when drag starts
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // Calculate distance between drag origin and new position if still held down
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log("origin" + dragOrigin + " new position " + cam.ScreenToWorldPoint(Input.mousePosition) + " =difference " + difference);
            cam.transform.position += difference;
        }

        if (Input.GetKeyDown("space"))
        {
            cam.transform.position += new Vector3(10f, 0, 0);
            Debug.Log("chamge");
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }
}
