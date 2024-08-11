using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/WalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLen = 10;
    public bool startRandomlyEachIter = true;
}
