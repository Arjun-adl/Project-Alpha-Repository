public class Player
{
    public Location PlayerLocation;
    public readonly string Name;
    public Dictionary<Item, int> inventory;
    public Weapon EquippedWeapon;
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    private char lastInvalidDirection;
    private int invalidMoveCount;

    public Player(string name, Location location, Weapon equippedWeapon = null)
    {
        Name = name;
        inventory = new Dictionary<Item, int>();
        PlayerLocation = location;
        EquippedWeapon = equippedWeapon;
        MaximumHitPoints = 50;
        CurrentHitPoints = MaximumHitPoints;
        lastInvalidDirection = '\0';
        invalidMoveCount = 0;
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

    public void Move(char direction)
    {
        Console.Clear();

        Location.ShowMap(this);

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter direction (W/A/S/D): ");
        Console.ResetColor();

        string? enteredDirection = direction.ToString();

        if (enteredDirection == null)
        {
            enteredDirection = "";
        }

        enteredDirection = enteredDirection.ToUpper();

        if (enteredDirection.Length == 1)
        {
            char locationDirection = enteredDirection[0] switch
            {
                'W' => 'N',
                'A' => 'W',
                'S' => 'S',
                'D' => 'E',
                _ => '\0'
            };

            Location? nextLocation =
                PlayerLocation.Move(locationDirection);

            if (nextLocation != null &&
                (nextLocation.MonsterLivingHere == null ||
                (nextLocation.QuestAvailableHere != null &&
                nextLocation.QuestAvailableHere.IsActive)))
            {
                PlayerLocation = nextLocation;
                lastInvalidDirection = ' ';
                invalidMoveCount = 0;

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"You moved to {nextLocation.Name}.");
                Console.ResetColor();

                Quest? quest =
                    PlayerLocation.QuestAvailableHere;

                if (quest != null &&
                    PlayerLocation.MonsterLivingHere == null &&
                    !quest.IsComplete)
                {
                    if (!quest.IsActive && !quest.IsComplete)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor =
                            ConsoleColor.Green;

                        Console.WriteLine(
                            $"Quest available: {quest.Name}");

                        Console.ResetColor();

                        Console.WriteLine(quest.Description);
                        Console.WriteLine();

                        Console.ForegroundColor =
                            ConsoleColor.Yellow;

                        Console.Write("Accept quest? (Y/N): ");

                        Console.ResetColor();

                        string? answer = Console.ReadLine();

                        if (answer != null &&
                            answer.ToUpper() == "Y")
                        {
                            quest.IsActive = true;

                            Console.WriteLine();

                            Console.ForegroundColor =
                                ConsoleColor.Green;

                            Console.WriteLine(
                                $"Quest accepted: {quest.Name}");

                            Console.ResetColor();
                        }
                    }
                }

                if (quest != null && quest.IsComplete)
                {
                    Console.WriteLine();

                    Console.ForegroundColor =
                        ConsoleColor.Green;

                    Console.WriteLine("Quest completed!");

                    Console.ResetColor();

                    Quest.GiveReward(this, quest);
                }

                if (PlayerLocation.MonsterLivingHere != null &&
                    quest != null &&
                    quest.IsActive)
                {
                    Battle.StartBattle(this, PlayerLocation.MonsterLivingHere);

                    bool allQuestsComplete = true;

                    foreach (Quest worldQuest in World.Quests)
                    {
                        if (!worldQuest.IsComplete)
                        {
                            allQuestsComplete = false;
                            break;
                        }
                    }

                    if (allQuestsComplete)
                    {
                        Console.Clear();

                        Console.ForegroundColor =
                            ConsoleColor.Green;

                        Console.WriteLine();
                        Console.WriteLine(
                            "================================");
                        Console.WriteLine(
                            "            YOU WIN!");
                        Console.WriteLine(
                            "================================");

                        Console.ResetColor();

                        Console.WriteLine();
                        Console.WriteLine(
                            "You completed all three quests!");
                        Console.WriteLine();

                        Console.ForegroundColor =
                            ConsoleColor.DarkGray;

                        Console.WriteLine(
                            "Press any key to exit...");

                        Console.ResetColor();

                        Console.ReadKey();

                        return;
                    }
                }
            }
            else
            {
                if (nextLocation != null &&
                    nextLocation.MonsterLivingHere != null &&
                    (nextLocation.QuestAvailableHere == null ||
                    !nextLocation.QuestAvailableHere.IsActive))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "You must accept the designated quest before entering this area.");
                }

                if (lastInvalidDirection == locationDirection)
                {
                    invalidMoveCount++;
                }
                else
                {
                    lastInvalidDirection = locationDirection;
                    invalidMoveCount = 1;
                }

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine();
                Console.WriteLine(
                    "You cannot move in that direction.");

                Console.ResetColor();

                if (invalidMoveCount >= 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("You tripped and died.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine(
                "You cannot move in that direction.");

            Console.ResetColor();
        }

        Console.ReadKey();
        return;
    }
}