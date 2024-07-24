using System;
using System.Collections.Generic;
using UnityEngine;

public class EventIconData : MonoBehaviour
{
    [Serializable]
    public class IconTypes
    {
        public string type;
        public Sprite sprite;
    }

    public IconTypes[] icons;

    private void Awake()
    {
        
    }

    public void SetIcon(string type)
    {
        foreach (IconTypes icon in icons)
        {
            if (icon.type.Equals(type))
                GetComponent<SpriteRenderer>().sprite = icon.sprite;
        }
    }
}
