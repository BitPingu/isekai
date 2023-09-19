using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Class can't be instantiated (duplicated)
public class SaveSystem
{
    public static AllData gameData;
    [SerializeField]
    private PlayerPosition position;

    // Can be called anywhere without an instance
    public static void SaveAllData(PlayerPosition position, TileGrid grid, DayAndNightCycle dayNight, FogData fog, BuildingData building)
    {
        // Create binary file to save to
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData.isekai";
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log("save");
        //Debug.Log("pos: " + position);
        /*// Get game data
        position = GameObject.Find("Player").GetComponent<PlayerPosition>();
        grid = GameObject.Find("Wprld").GetComponent<TileGrid>();
        dayNight = GameObject.Find("World").GetComponent<DayAndNightCycle>();
        fog = GameObject.Find("World").GetComponent<FogData>();
        building = GameObject.Find("World").GetComponent<BuildingData>();*/

        // Combine game data
        PlayerData playerData = new PlayerData(position);
        WorldData worldData = new WorldData(grid, dayNight, fog, building);
        AllData allData = new AllData(playerData, worldData);

        // Write data to file
        formatter.Serialize(stream, allData);

        // Close stream
        stream.Close();
    }

    public static void LoadAllData()
    {
        string path = Application.persistentDataPath + "/saveData.isekai";
        if (File.Exists(path))
        {
            Debug.Log("load");
            // Save file exists (read data from file)
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Extract game data
            gameData = formatter.Deserialize(stream) as AllData;

            // Close stream
            stream.Close();
        }
        else
        {
            // Save file does not exist
            Debug.LogError("Save file not found in " + path);
        }
    }

    public static PlayerData LoadPlayer()
    {
        //Debug.Log("pos: " + gameData.player.savedPosX);
        return gameData.player;
    }

    public static WorldData LoadWorld()
    {
        return gameData.world;
    }
}
