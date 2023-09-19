
[System.Serializable]
public class AllData
{
    public PlayerData player;
    public WorldData world;

    public AllData(PlayerData p, WorldData w)
    {
        player = p;
        world = w;
    }
}
