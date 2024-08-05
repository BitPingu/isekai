using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFade : MonoBehaviour
{
    private Color treeDefaultColor, treeFadedColor;

    private void Awake()
    {
        treeDefaultColor = GetComponent<SpriteRenderer>().color;
        treeFadedColor = treeDefaultColor;
        treeFadedColor.a = .5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // Player enters
            GetComponent<SpriteRenderer>().color = treeFadedColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            GetComponent<SpriteRenderer>().color = treeDefaultColor;
        }
    }
}
