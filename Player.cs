public class Player
{
    public Location PlayerLocation;
    public readonly string Name;
    public Dictionary<string, int> inventory;

    public Player(string name, Location location)
    {
        Name = name;
        inventory = new Dictionary<string, int>();
        PlayerLocation = location;
    }

    public void AddItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory[itemName] = amount;
        }
    }

    public void PrintInventory()
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine("Inventory is empty.");
            return;
        }
        Console.WriteLine("Inventory: ");
        foreach (var item in inventory)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
    }
}