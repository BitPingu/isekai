using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Serializable]
    public class Prompt
    {
        [TextArea(3, 10)]
        public string sentence;
        public string[] options;
    }
    public string name;
    public Prompt[] prompts;
}
