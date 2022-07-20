using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public int seed;
    public float time;
    public bool isDay;
    public List<int> clearFogCoordsX, clearFogCoordsY;
    public List<int> dungeonCoordsX, dungeonCoordsY;

    // Constructor
    public WorldData (TileGrid world, DayAndNightCycle dayNight, FogData fog, DungeonData dungeon)
    {
        // Store world data in variables
        seed = world.seed;
        time = dayNight.time;
        isDay = dayNight.isDay;
        clearFogCoordsX = fog.clearFogCoordsX;
        clearFogCoordsY = fog.clearFogCoordsY;
        dungeonCoordsX = dungeon.dungeonCoordsX;
        dungeonCoordsY = dungeon.dungeonCoordsY;
    }
}
