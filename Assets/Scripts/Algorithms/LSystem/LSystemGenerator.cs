using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/LSystem/Generator")]
public class LSystemGenerator : ScriptableObject
{
    public Rule[] rules; // Rule1 (scriptable object)
    public string rootSentence; // [F]--F
    [Range(0,10)]
    public int iterationLimit = 1; // 2

    public bool randomIgnoreRuleModifier = true; // by default
    [Range(0,1)]
    public float chanceToIgnoreRule = 0.3f; // 0.3

    private void Start()
    {
        Debug.Log(GenerateSentence());
    }

    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = rootSentence;
        }
        return GrowRecursive(word);
    }

    private string GrowRecursive(string word, int iterationIndex = 0)
    {
        if (iterationIndex >= iterationLimit)
        {
            return word;
        }
        StringBuilder newWord = new StringBuilder();

        foreach (var character in word)
        {
            newWord.Append(character);
            ProcessRulesRecursivelly(newWord, character, iterationIndex);
        }

        return newWord.ToString();
    }

    private void ProcessRulesRecursivelly(StringBuilder newWord, char character, int iterationIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == character.ToString())
            {
                if (randomIgnoreRuleModifier && iterationIndex > 1)
                {
                    if (Random.value < chanceToIgnoreRule)
                    {
                        return;
                    }
                }
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}
