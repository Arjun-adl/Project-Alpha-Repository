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

    public bool UseInventoryItem()
    {
        List<Potion> potionsInInventory = new List<Potion>();
        List<Weapon> weaponsInInventory = new List<Weapon>();

        foreach (var kvp in inventory)
        {
            if (kvp.Key is Potion potion && kvp.Value > 0)
            {
                potionsInInventory.Add(potion);
            }
            else if (kvp.Key is Weapon weapon && kvp.Value > 0)
            {
                weaponsInInventory.Add(weapon);
            }
        }

        if (potionsInInventory.Count == 0 && weaponsInInventory.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("You have no usable items.");
            Console.ReadKey();
            return false;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Items:");

            int index = 1;

            if (potionsInInventory.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("-- Potions --");

                foreach (Potion potion in potionsInInventory)
                {
                    Console.WriteLine($"{index}. {potion.Name} x{inventory[potion]} (Healing: {potion.AmountToHeal})");
                    index++;
                }
            }

            if (weaponsInInventory.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("-- Weapons --");

                foreach (Weapon weapon in weaponsInInventory)
                {
                    string equippedTag = (weapon == EquippedWeapon) ? " (Equipped)" : "";
                    Console.WriteLine($"{index}. {weapon.Name} x{inventory[weapon]} (Damage: {weapon.Damage}){equippedTag}");
                    index++;
                }
            }
            Console.WriteLine("Choose an item to use or equip by entering its number, or enter 0 to cancel:");

            Console.WriteLine();
            Console.WriteLine("0. Cancel");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            if (choice == 0)
            {
                return false;
            }

            if (choice < 1 || choice > potionsInInventory.Count + weaponsInInventory.Count)
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            if (choice <= potionsInInventory.Count)
            {
                Potion selectedPotion = potionsInInventory[choice - 1];

                int oldHP = CurrentHitPoints;

                CurrentHitPoints += selectedPotion.AmountToHeal;

                if (CurrentHitPoints > MaximumHitPoints)
                {
                    CurrentHitPoints = MaximumHitPoints;
                }

                int actualHealing = CurrentHitPoints - oldHP;

                if (inventory[selectedPotion] == 1)
                {
                    inventory.Remove(selectedPotion);
                }
                else
                {
                    inventory[selectedPotion]--;
                }

                Console.WriteLine();
                Console.WriteLine($"You used a {selectedPotion.Name}.");
                Console.WriteLine($"You recovered {actualHealing} HP.");
                Console.WriteLine($"Your HP: {CurrentHitPoints}/{MaximumHitPoints}");
                Console.ReadKey();
                Console.Clear();
                return true;
            }
            else
            {
                int weaponIndex = choice - potionsInInventory.Count - 1;
                Weapon selectedWeapon = weaponsInInventory[weaponIndex];

                if (selectedWeapon == EquippedWeapon)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{selectedWeapon.Name} is already equipped.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                while (true)
                {
                    Console.WriteLine($"Equip {selectedWeapon.Name}? (Y/N)");
                    string confirm = Console.ReadLine()?.ToUpper() ?? "";

                    if (confirm == "Y")
                    {
                        EquippedWeapon = selectedWeapon;

                        Console.WriteLine();
                        Console.WriteLine($"You have equipped {selectedWeapon.Name}.");
                        Console.ReadKey();
                        Console.Clear();
                        return true;
                    }
                    else if (confirm == "N")
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please enter Y or N.");
                    }
                }
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