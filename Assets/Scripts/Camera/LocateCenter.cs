using UnityEngine;

public class LocateCenter : MonoBehaviour
{
    [SerializeField]
    private float y;
    public void Awake()
    {
        // Spawn point position
        transform.position = new Vector3(Noise.halfWidth + 0.5f, Noise.halfHeight + 0.6f, y);
    }
}
