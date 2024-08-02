using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationData : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        if (collision.gameObject.name.Contains("Player") || 
            (collision.gameObject.tag.Contains("Enemy") && collision.gameObject.GetComponent<EnemyPosition>().CheckPlayer()))
        {
            FindObjectOfType<AudioManager>().PlayFx("Tree");
        }
    }
}
