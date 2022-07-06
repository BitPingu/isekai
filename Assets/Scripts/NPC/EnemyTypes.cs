using System;
using UnityEngine;

public class EnemyTypes
{
    [SerializeField]
    public EnemyData[] Enemies;

    [Serializable]
    public class EnemyData
    {
        public GameObject enemy;
        public bool nightEnemy;
    }
}
