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

        saveVillageCoordsX = TempData.tempBuilding.villageCoordsX;
        saveVillageCoordsY = TempData.tempBuilding.villageCoordsY;
        saveDungeonCoordsX = TempData.tempBuilding.dungeonCoordsX;
        saveDungeonCoordsY = TempData.tempBuilding.dungeonCoordsY;
    }
}
