using System;
using UnityEngine;

public class EnemyTypes
{
    [Serializable]
    public class EnemyData
    {
        public GameObject gameObject;
        public string type;
        public Vector3 spawnPoint;
        public bool nightEnemy;
    }

    public GameObject gameObject;
    public string type;
    public Vector3 spawnPoint;
    public bool nightEnemy;

    public EnemyTypes(GameObject gameObject, string type, Vector3 spawnPoint, bool nightEnemy)
    {
        this.gameObject = gameObject;
        this.type = type;
        this.spawnPoint = spawnPoint;
        this.nightEnemy = nightEnemy;
    }
}
