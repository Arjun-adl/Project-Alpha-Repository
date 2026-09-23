public static class Program
{
    public static void Main()
    {
        Player player = new Player("Hero", World.LocationByID(World.LOCATION_ID_HOME));

        player.AddItem(World.WeaponByID(World.WEAPON_ID_RUSTY_SWORD));

        player.AddItem(World.PotionByID(World.POTION_ID_HEALING_POTION));

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

            List<string> availableDirections = new List<string>();

            if (player.PlayerLocation.LocationToNorth != null)
            {
                availableDirections.Add("W = north");
            }

            if (player.PlayerLocation.LocationToWest != null)
            {
                availableDirections.Add("A = west");
            }

            if (player.PlayerLocation.LocationToSouth != null)
            {
                availableDirections.Add("S = south");
            }

            if (player.PlayerLocation.LocationToEast != null)
            {
                availableDirections.Add("D = east");
            }

            if (availableDirections.Count > 0)
            {
                Console.WriteLine(string.Join(", ", availableDirections));
            }
            else
            {
                Console.WriteLine("No directions available.");
            }

            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Show location info");
            Console.WriteLine("2. Show Quest Log");
            Console.WriteLine("3. Show Inventory");
            Console.WriteLine("4. Visit Town Square Shop");
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

            if (choice.Length == 1 && "WASD".Contains(choice.ToUpper()))
            {
                player.Move(choice.ToUpper()[0]);
                continue;
            }
            else
            {
                choice = choice.Trim();
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

                case "3":
                    Console.Clear();
                    player.UseInventoryItem();

                    

                    break;

                case "4":
                    if (player.PlayerLocation.ID == World.LOCATION_ID_TOWN_SQUARE)
                    {
                        player.PlayerLocation.ShowShop(player);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("The shop is only available in the Town Square.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
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

    public static bool AllQuestsComplete()
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