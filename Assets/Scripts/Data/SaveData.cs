using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float[] saveSpawnPoint;
    public int saveSeed;
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
        saveSpawnPoint = new float[3];
        saveSpawnPoint[0] = TempData.tempPos.x;
        saveSpawnPoint[1] = TempData.tempPos.y;
        saveSpawnPoint[2] = TempData.tempPos.z;

        saveSeed = TempData.tempSeed;

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
