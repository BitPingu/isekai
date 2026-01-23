using UnityEngine;

[CreateAssetMenu(menuName ="Algorithms/Dungeon/WalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLen = 10;
    public bool startRandomlyEachIter = true;
}
