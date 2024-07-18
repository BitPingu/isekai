using UnityEngine;

public class WorldLocator : MonoBehaviour
{
    private Vector3 newPosition;
    private float centerX;
    private float centerY;

    private void LateUpdate()
    {
        // Calculate center 
        centerX = TempData.tempWidth / 2;
        centerY = TempData.tempHeight / 2;
        newPosition = new Vector3(centerX, centerY, transform.position.z);

        // Align in center of the world
        transform.position = newPosition;
    }
}
