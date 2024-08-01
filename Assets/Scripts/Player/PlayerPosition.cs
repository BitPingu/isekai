using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPosition : MonoBehaviour
{
    public Vector2Int prevPos, currentPos;
    public string currentArea;

    // Call other functions when player position changes
    public delegate void OnPosChange();
    public OnPosChange PosChange;

    private void Update()
    {
        // Retrieve coordinates of player
        currentPos = Vector2Int.FloorToInt(transform.position);
        TempData.tempPlayerPos = new Vector3(transform.position.x, transform.position.y);

        // Position check
        if (currentPos != prevPos)
        {
            PosChange(); // Call delegate (and any methods tied to it)
            prevPos = currentPos;
        }
    }
}
