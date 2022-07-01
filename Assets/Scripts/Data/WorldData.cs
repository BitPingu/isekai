
[System.Serializable]
public class WorldData
{
    public int seed;
    public float time;
    public bool isDay;

    // Constructor
    public WorldData (TileGrid world, DayAndNightCycle dayNight)
    {
        // Store world data in variables
        seed = world.seed;
        time = dayNight.time;
        isDay = dayNight.isDay;
    }
}
