using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float[] savePlayerPos;
    public float[] saveElfPos;
    public int saveSeed;
    public int saveDays;
    public float saveTime;
    public bool saveIsDay;
    public List<int> saveClearFogCoordsX;
    public List<int> saveClearFogCoordsY;
    public List<int> saveVillageCoordsX;
    public List<int> saveVillageCoordsY;
    public List<int> saveDungeonCoordsX;
    public List<int> saveDungeonCoordsY;
    public List<int> saveCampCoordsX;
    public List<int> saveCampCoordsY;


    public SaveData ()
    {
        savePlayerPos = new float[3];
        savePlayerPos[0] = TempData.tempPlayerPos.x;
        savePlayerPos[1] = TempData.tempPlayerPos.y;
        savePlayerPos[2] = TempData.tempPlayerPos.z;

        saveElfPos = new float[3];
        saveElfPos[0] = TempData.tempElfPos.x;
        saveElfPos[1] = TempData.tempElfPos.y;
        saveElfPos[2] = TempData.tempElfPos.z;

        saveSeed = TempData.tempSeed;

        saveDays = TempData.tempDays;
        saveTime = TempData.tempTime;
        saveIsDay = TempData.tempIsDay;

        saveClearFogCoordsX = TempData.tempFog.clearFogCoordsX;
        saveClearFogCoordsY = TempData.tempFog.clearFogCoordsY;

        List<int> vilsX = new List<int>();
        List<int> vilsY = new List<int>();
        foreach (Vector3 village in TempData.tempVillages)
        {
            vilsX.Add((int)village.x);
            vilsY.Add((int)village.y);
        }
        saveVillageCoordsX = vilsX;
        saveVillageCoordsY = vilsY;

        List<int> dunsX = new List<int>();
        List<int> dunsY = new List<int>();
        foreach (Vector3 dungeon in TempData.tempDungeons)
        {
            dunsX.Add((int)dungeon.x);
            dunsY.Add((int)dungeon.y);
        }
        saveDungeonCoordsX = dunsX;
        saveDungeonCoordsY = dunsY;

        List<int> campX = new List<int>();
        List<int> campY = new List<int>();
        foreach (Vector3 camp in TempData.tempCamps)
        {
            campX.Add((int)camp.x);
            campY.Add((int)camp.y);
        }
        saveCampCoordsX = campX;
        saveCampCoordsY = campY;
    }
}
