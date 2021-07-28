using UnityEngine;
using Cinemachine;

public class VCameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam1;

    [SerializeField]
    private CinemachineVirtualCamera vcam2;

    [SerializeField]
    private bool playerVCamera = true;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerInventory>().map)
        {
            FindObjectOfType<PlayerController>().map.performed += _ => SwitchPriority();
        }
    }

    private void SwitchPriority()
    {
        if (playerVCamera)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
            FindObjectOfType<PlayerMovement>().enabled = false;
        } else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
            FindObjectOfType<PlayerMovement>().enabled = true;
        }
        playerVCamera = !playerVCamera;
    }
}
