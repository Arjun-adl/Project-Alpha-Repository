public class Player
{
    public Location PlayerLocation;
    public readonly string Name;

    public Player(string name, Location location)
    {
        Name = name;
        PlayerLocation = location;
    }

}