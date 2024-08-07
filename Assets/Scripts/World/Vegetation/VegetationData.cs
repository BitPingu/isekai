using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationData : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player") || 
            (collision.gameObject.tag.Contains("Enemy") && collision.gameObject.GetComponent<EnemyPosition>().CheckPlayer()))
        {
            FindObjectOfType<AudioManager>().PlayFx("Tree");
        }
    }
}
