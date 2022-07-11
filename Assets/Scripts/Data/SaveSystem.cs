using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Class can't be instantiated (duplicated)
public static class SaveSystem
{
    public static AllData gameData;

    // Can be called anywhere without an instance
    public static void SaveAllData (PlayerController player, TileGrid world, DayAndNightCycle dayNight, FogData fog)
    {
        // Create binary file to save to
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData.isekai";
        FileStream stream = new FileStream(path, FileMode.Create);

        // Create game data
        PlayerData playerData = new PlayerData(player);
        WorldData worldData = new WorldData(world, dayNight, fog);

        // Combine game data
        AllData allData = new AllData();
        allData.playerData = playerData;
        allData.worldData = worldData;

        // Write data to file
        formatter.Serialize(stream, allData);

        // Close stream
        stream.Close();
    }

    public static void LoadAllData ()
    {
        string path = Application.persistentDataPath + "/saveData.isekai";
        if (File.Exists(path))
        {
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

    public static PlayerData LoadPlayer ()
    {
        return gameData.playerData;
    }

    public static WorldData LoadWorld()
    {
        return gameData.worldData;
    }
}
