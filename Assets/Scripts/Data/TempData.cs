using System.Collections.Generic;
using UnityEngine;

public class TempData : MonoBehaviour
{
    // init vars are set in mainmenu
    public static bool loadGame;
    public static bool initPlayerSpawn;
    public static Vector2 tempPlayerStartingSpawn;
    public static Vector2 tempPlayerBuildingSpawn;
    public static Vector2 tempPlayerPos;
    public static bool initSeed;
    public static int tempSeed;
    public static int tempWidth;
    public static int tempHeight;
    public static int tempDays;
    public static bool initTime;
    public static float tempTime;
    public static bool tempIsDay;
    public static bool initFog;
    public static FogData tempFog;
    // public static FogData tempFog2;
    public static bool initBuilding;
    public static List<Village> tempVillages;
    public static List<Vector2> tempDungeons;
    public static List<Vector2> tempCamps;

    public static bool initElf;
    public static bool initElfSpawn;
    public static Vector2 tempElfStartingSpawn;
    public static Vector2 tempElfPos;
    public static bool elfSaved;
}
