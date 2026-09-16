public class Player
{
    public Location PlayerLocation;
    public readonly string Name;
    public Dictionary<Item, int> inventory;
    public Weapon EquippedWeapon;
    public int CurrentHitPoints;
    public int MaximumHitPoints;

    public Player(string name, Location location, Weapon equippedWeapon = null)
    {
        Name = name;
        inventory = new Dictionary<Item, int>();
        PlayerLocation = location;
        EquippedWeapon = equippedWeapon;
        MaximumHitPoints = 50;
        CurrentHitPoints = MaximumHitPoints;
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
        while (true)
        {
            Console.Clear();

            if (inventory.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
            }
            else
            {
                Console.WriteLine("Inventory: ");
                foreach (var kvp in inventory)
                {
                    Console.WriteLine($"{kvp.Key.Name} x{kvp.Value}");
                }
            }

            Console.WriteLine($"Equipped Weapon: {(EquippedWeapon != null ? EquippedWeapon.Name : "None")}");
            Console.WriteLine();
            Console.WriteLine("Press Y to equip a weapon, or Enter to return to the main menu.");

            string choice = Console.ReadLine();

            if (choice == null)
            {
                choice = "";
            }

            if (choice.ToUpper() == "Y")
            {
                EquipWeaponMenu();
            }
            else if (choice == "")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice. Press any key to try again.");
                Console.ReadKey();
            }
        }
    }

    private void EquipWeaponMenu()
    {
        List<Weapon> weaponsInInventory = new List<Weapon>();

        foreach (var kvp in inventory)
        {
            if (kvp.Key is Weapon weapon)
            {
                weaponsInInventory.Add(weapon);
            }
        }

        if (weaponsInInventory.Count == 0)
        {
            Console.WriteLine("You have no weapons in your inventory.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Which weapon would you like to equip?");

        for (int i = 0; i < weaponsInInventory.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {weaponsInInventory[i].Name}");
        }

        string input = Console.ReadLine();

        if (int.TryParse(input, out int choiceIndex) &&
            choiceIndex - 1 >= 0 &&
            choiceIndex - 1 < weaponsInInventory.Count)
        {
            EquippedWeapon = weaponsInInventory[choiceIndex - 1];
            Console.WriteLine($"You have equipped {EquippedWeapon.Name}.");
        }
        else
        {
            Console.WriteLine("Invalid weapon choice.");
        }

        Console.ReadKey();
    }
}