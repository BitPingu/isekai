using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public bool interactable;
    [SerializeField]
    private float maxDistance; // default is 0.5f
    public GameObject icon;
    private GameObject newIcon;

    private void Awake()
    {
        // Set event icon
        Vector3 iconPos = new Vector3(transform.position.x, transform.position.y + 1);
        newIcon = Instantiate(icon, iconPos, Quaternion.identity);
        newIcon.transform.parent = gameObject.transform;
        newIcon.GetComponent<EventIconData>().SetIcon("Enter");
    }

    private void Update()
    {
        // if (interactable && CheckPlayer())
        // {
        //     newIcon.SetActive(true);
        // }
        // else
        // {
        //     newIcon.SetActive(false);
        // }
    }

    private bool CheckPlayer()
    {
        // Calculate current distance from player
        float distance = Vector3.Distance(FindObjectOfType<PlayerPosition>().transform.position, transform.position);

        if (distance < maxDistance)
        {
            return true;
        }

        return false;
    }
}
