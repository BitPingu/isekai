using System;
using UnityEngine;

public class VillagerTypes
{
    [SerializeField]
    public VillagerData[] Villagers;

    [Serializable]
    public class VillagerData
    {
        public GameObject villager;
        public string occupation;
    }
}
