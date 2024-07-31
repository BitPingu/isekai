using System.Collections.Generic;
using System.Linq;
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
    public List<string> saveVillageSequences;
    public List<int> saveDungeonCoordsX;
    public List<int> saveDungeonCoordsY;
    public List<int> saveCampCoordsX;
    public List<int> saveCampCoordsY;
    public bool saveElf;


    public SaveData ()
    {
        savePlayerPos = new float[2];
        savePlayerPos[0] = TempData.tempPlayerPos.x;
        savePlayerPos[1] = TempData.tempPlayerPos.y;

        saveElfPos = new float[2];
        saveElfPos[0] = TempData.tempElfPos.x;
        saveElfPos[1] = TempData.tempElfPos.y;

        saveSeed = TempData.tempSeed;

        saveDays = TempData.tempDays;
        saveTime = TempData.tempTime;
        saveIsDay = TempData.tempIsDay;

        List<int> fogX = new List<int>();
        List<int> fogY = new List<int>();
        foreach (Vector2 clearFog in TempData.tempFog.clearFogCoords)
        {
            fogX.Add((int)clearFog.x);
            fogY.Add((int)clearFog.y);
        }
        saveClearFogCoordsX = fogX;
        saveClearFogCoordsY = fogY;

        List<int> vilsX = new List<int>();
        List<int> vilsY = new List<int>();
        List<string> vilSeq = new List<string>();
        foreach (Village vil in TempData.tempVillages)
        {
            vilsX.Add((int)vil.vilCenter.x);
            vilsY.Add((int)vil.vilCenter.y);
            vilSeq.Add(vil.vilSequence);
        }
        saveVillageCoordsX = vilsX;
        saveVillageCoordsY = vilsY;
        saveVillageSequences = vilSeq;

        List<int> dunsX = new List<int>();
        List<int> dunsY = new List<int>();
        foreach (Vector2 dungeon in TempData.tempDungeons)
        {
            dunsX.Add((int)dungeon.x);
            dunsY.Add((int)dungeon.y);
        }
        saveDungeonCoordsX = dunsX;
        saveDungeonCoordsY = dunsY;

        List<int> campX = new List<int>();
        List<int> campY = new List<int>();
        foreach (Vector2 camp in TempData.tempCamps)
        {
            campX.Add((int)camp.x);
            campY.Add((int)camp.y);
        }
        saveCampCoordsX = campX;
        saveCampCoordsY = campY;

        saveElf = TempData.elfSaved;
    }
}
