using UnityEngine;

public class MapIconLocator : MonoBehaviour
{
    [SerializeField]
    private Transform icon;

    private void OnEnable()
    {
        transform.position = icon.position;
    }
}
