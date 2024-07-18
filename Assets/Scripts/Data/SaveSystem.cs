using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Class can't be instantiated (duplicated)
public class SaveSystem
{
    // Can be called anywhere without an instance
    public static void Save()
    {
        // Create binary file to save to
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData.isekai";
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log("saving...");

        // Combine game data
        SaveData saveData = new SaveData();

        // Write data to file
        formatter.Serialize(stream, saveData);

        // Close stream
        stream.Close();
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/saveData.isekai";
        if (File.Exists(path))
        {
            // Save file exists (read data from file)
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Extract game data
            SaveData saveData = formatter.Deserialize(stream) as SaveData;

            // Close stream
            stream.Close();
            
            // Return data
            return saveData;
        }
        else
        {
            // Save file does not exist
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
