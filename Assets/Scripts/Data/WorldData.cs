using System.Collections.Generic;

[System.Serializable]
public class WorldData
{
    public int savedSeed;
    public float savedTime;
    public bool savedIsDay;
    public List<int> savedClearFogCoordsX, savedClearFogCoordsY, savedVillageCoordsX, savedVillageCoordsY, savedDungeonCoordsX, savedDungeonCoordsY;

    // Constructor
    public WorldData (TileGrid grid, DayAndNightCycle dayNight, FogData fog, BuildingData building)
    {
        savedSeed = grid.seed;
        savedTime = dayNight.time;
        savedIsDay = dayNight.isDay;
        fog.GetClearFog();
        savedClearFogCoordsX = fog.clearFogCoordsX;
        savedClearFogCoordsY = fog.clearFogCoordsY;
        savedVillageCoordsX = building.villageCoordsX;
        savedVillageCoordsY = building.villageCoordsY;
        savedDungeonCoordsX = building.dungeonCoordsX;
        savedDungeonCoordsY = building.dungeonCoordsY;
    }
}
