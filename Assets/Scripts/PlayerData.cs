
[System.Serializable]
public class PlayerData
{
    public int[] savedPos;

    // Constructor
    public PlayerData (PlayerController player)
    {
        // Store player data in variables
        savedPos = new int[2];
        savedPos[0] = player.currentPos.x;
        savedPos[1] = player.currentPos.y;
    }
}
