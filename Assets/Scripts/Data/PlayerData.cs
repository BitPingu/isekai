[System.Serializable]
public class PlayerData
{
    public int savedPosX, savedPosY;

    // Constructor
    public PlayerData(PlayerPosition position)
    {
        savedPosX = position.currentPos.x;
        savedPosY = position.currentPos.y;
    }
}
