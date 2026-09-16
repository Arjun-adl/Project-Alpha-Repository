public static class Program
{
    public static void Main()
    {
        Player player = new Player("Hero", World.LocationByID(World.LOCATION_ID_HOME));

        player.AddItem(World.WeaponByID(World.WEAPON_ID_RUSTY_SWORD));
        player.AddItem(World.PotionByID(World.POTION_ID_HEALING_POTION));

        Console.Clear();
        Console.WriteLine("Welcome to Project Alpha");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        while (true)
        {
            Console.Clear();
            Location.ShowMap(player);
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"Current location: {player.PlayerLocation.Name}");
            Console.WriteLine($"HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine("1. Show location info");
            Console.WriteLine("2. Move (N/E/S/W)");
            Console.WriteLine("3. Show Quest Log");
            Console.WriteLine("4. Show Inventory");
            Console.WriteLine("5. Exit");

            string? choice = Console.ReadLine();

            if (choice == null)
            {
                choice = "";
            }

            switch (choice)
            {
                case "1":
                    player.PlayerLocation.ShowLocationInfo();
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    Location.ShowMap(player);

                    Console.Write("Enter direction (N/E/S/W): ");
                    string? direction = Console.ReadLine();

                    if (direction == null)
                    {
                        direction = "";
                    }

                    direction = direction.ToUpper();

                    if (direction.Length == 1)
                    {
                        Location? nextLocation = player.PlayerLocation.Move(direction[0]);

                        if (nextLocation != null)
                        {
                            player.PlayerLocation = nextLocation;
                            Quest? quest = player.PlayerLocation.QuestAvailableHere;

                            Console.WriteLine($"You moved to {nextLocation.Name}.");

                            if (quest != null &&
                                player.PlayerLocation.MonsterLivingHere == null &&
                                !quest.IsComplete)
                            {

                                if (!quest.IsActive && !quest.IsComplete)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Quest available: {quest.Name}");
                                    Console.WriteLine(quest.Description);
                                    Console.WriteLine();
                                    Console.WriteLine("Accept quest? (Y/N)");

                                    string? answer = Console.ReadLine();

                                    if (answer != null && answer.ToUpper() == "Y")
                                    {
                                        quest.IsActive = true;

                                        Console.WriteLine();
                                        Console.WriteLine($"Quest accepted: {quest.Name}");
                                    }
                                }   
                            }

                            if (quest != null && quest.IsComplete)
                            {
                                Console.WriteLine();
                                Quest.GiveReward(player, quest);
                            }

                            if (player.PlayerLocation.MonsterLivingHere != null &&
                                quest != null &&
                                quest.IsActive)
                            {
                                Battle.StartBattle(
                                    player,
                                    player.PlayerLocation.MonsterLivingHere);

                                if (AllQuestsComplete())
                                {
                                    Console.Clear();
                                    Console.WriteLine();
                                    Console.WriteLine("================================");
                                    Console.WriteLine("          YOU WIN!");
                                    Console.WriteLine("================================");
                                    Console.WriteLine();
                                    Console.WriteLine("You completed all three quests!");
                                    Console.WriteLine();
                                    Console.WriteLine("Press any key to exit...");
                                    Console.ReadKey();

                                    return;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("You cannot move in that direction.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You cannot move in that direction.");
                    }

                    Console.ReadKey();
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("Quest Log:");
                    World.QuestLog();
                    Console.ReadKey();
                    break;

                case "4":
                    Console.Clear();
                    player.PrintInventory();
                    Console.ReadKey();
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static bool AllQuestsComplete()
    {
        foreach (Quest quest in World.Quests)
        {
            if (!quest.IsComplete)
            {
                return false;
            }
        }

        return true;
    }
}