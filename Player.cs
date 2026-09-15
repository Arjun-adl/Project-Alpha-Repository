public class Player
{
    public Location PlayerLocation;
    public readonly string Name;
    public Dictionary<Item, int> inventory;

    public Player(string name, Location location)
    {
        Name = name;
        inventory = new Dictionary<Item, int>();
        PlayerLocation = location;
    }

    public void AddItem(Item item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory[item] = amount;
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
        foreach (var kvp in inventory)
        {
            Console.WriteLine($"{kvp.Key.Name} x{kvp.Value}");
        }
    }
}