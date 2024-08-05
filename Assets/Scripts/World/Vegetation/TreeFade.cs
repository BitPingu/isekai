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
        treeFadedColor.a = .3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject && (collision.gameObject.name.Contains("Player") || collision.gameObject.name.Contains("Elf")))
        {
            // Player enters
            StartCoroutine(FadeOut());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject && (collision.gameObject.name.Contains("Player") || collision.gameObject.name.Contains("Elf")))
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f, fadeDuration = .1f;
        while (gameObject && elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(treeDefaultColor, treeFadedColor, elapsedTime/fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f, fadeDuration = .1f;
        while (gameObject && elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(treeFadedColor, treeDefaultColor, elapsedTime/fadeDuration);
            yield return null;
        }
    }
}
