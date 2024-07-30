using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize, origCamSize; // 1 1 50 5
    
    [SerializeField]
    private GameObject playerIcon;
    private GameObject pIcon;

    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        // enabled = false;
    }

    private void Update()
    {
        PanCamera();
    }

    private void OnEnable()
    {
        // Show points of interest
        // ShowPointsOfInterest();
        // Set map cam size
        cam.orthographicSize = maxCamSize/2;
    }

    private void OnDisable()
    {
        // Hide points of interest
        HidePointsOfInterest();
        // Reset size
        cam.orthographicSize = origCamSize;   
    }

    private void ShowPointsOfInterest()
    {
        pIcon = Instantiate(playerIcon, FindObjectOfType<PlayerPosition>().transform.position, Quaternion.identity);
        pIcon.transform.parent = FindObjectOfType<PlayerPosition>().transform;
    }

    private void HidePointsOfInterest()
    {
        if (pIcon)
            Destroy(pIcon);
    }

    private void PanCamera()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Save position of mouse in world space when drag starts
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            // Calculate distance between drag origin and new position if still held down
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            // Move camera to position
            cam.transform.position += difference;
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
