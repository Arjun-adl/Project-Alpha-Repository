public static class Program
{
    public static void Main()
    {
        Player player = new Player(
            "Hero",
            World.LocationByID(World.LOCATION_ID_HOME));

        player.AddItem(
            World.WeaponByID(World.WEAPON_ID_RUSTY_SWORD));

        player.AddItem(
            World.PotionByID(World.POTION_ID_HEALING_POTION));

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("================================");
        Console.WriteLine("       WELCOME TO PROJECT ALPHA");
        Console.WriteLine("================================");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Press any key to continue...");
        Console.ResetColor();

        Console.ReadKey();

        while (true)
        {
            Console.Clear();

            // Show map
            Location.ShowMap(player);

            Console.WriteLine();

            // Game title
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================");
            Console.WriteLine("         PROJECT ALPHA");
            Console.WriteLine("================================");
            Console.ResetColor();

            Console.WriteLine();

            // Player information
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Location: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(player.PlayerLocation.Name);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("HP: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(
                $"{player.CurrentHitPoints}/{player.MaximumHitPoints}");

            Console.WriteLine();

            // Menu
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("What would you like to do?");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Show location info");
            Console.WriteLine("2. Move (N/E/S/W)");
            Console.WriteLine("3. Show Quest Log");
            Console.WriteLine("4. Show Inventory");
            Console.WriteLine("5. Exit");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Choose an option: ");
            Console.ResetColor();

            string? choice = Console.ReadLine();

            if (choice == null)
            {
                choice = "";
            }

            switch (choice)
            {
                case "1":
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("======= LOCATION INFO =======");
                    Console.ResetColor();

                    Console.WriteLine();

                    player.PlayerLocation.ShowLocationInfo();

                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to return...");
                    Console.ResetColor();

                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();

                    Location.ShowMap(player);

                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter direction (N/E/S/W): ");
                    Console.ResetColor();

                    string? direction = Console.ReadLine();

                    if (direction == null)
                    {
                        direction = "";
                    }

                    direction = direction.ToUpper();

                    if (direction.Length == 1)
                    {
                        Location? nextLocation =
                            player.PlayerLocation.Move(direction[0]);

                        if (nextLocation != null)
                        {
                            player.PlayerLocation = nextLocation;

                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(
                                $"You moved to {nextLocation.Name}.");
                            Console.ResetColor();

                            Quest? quest =
                                player.PlayerLocation.QuestAvailableHere;

                            if (quest != null &&
                                player.PlayerLocation.MonsterLivingHere == null &&
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
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.WriteLine();
                            Console.WriteLine(
                                "You cannot move in that direction.");

                            Console.ResetColor();
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
                    break;

                case "3":
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("======== QUEST LOG ========");
                    Console.ResetColor();

                    Console.WriteLine();

                    World.QuestLog();

                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to return...");
                    Console.ResetColor();

                    Console.ReadKey();
                    break;

                case "4":
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("======== INVENTORY ========");
                    Console.ResetColor();

                    Console.WriteLine();

                    player.PrintInventory();

                    Console.ReadKey();
                    break;

                case "5":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Thanks for playing!");
                    Console.ResetColor();

                    return;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine();
                    Console.WriteLine("Invalid choice.");

                    Console.ResetColor();

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