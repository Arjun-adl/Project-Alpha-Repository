public static class Program
{

    public static void Main()
    {
        Player player = new Player("Hero", World.LocationByID(World.LOCATION_ID_HOME));

        Console.Clear();
        Console.WriteLine("Welcome to Project Alpha");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"Current location: {player.PlayerLocation.Name}");
            Console.WriteLine("1. Show location info");
            Console.WriteLine("2. Move (N/E/S/W)");
            Console.WriteLine("3. Show Quest Log");
            Console.WriteLine("4. Exit");

            var choice = Console.ReadLine();
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
                    Location.ShowMap(player);

                    Console.Write("Enter direction (N/E/S/W): ");
                    string direction = Console.ReadLine();
                    if (direction == null)
                    {
                        direction = "";
                    }

                    direction = direction.ToUpper();
                    if (direction.Length == 1)
                    {
                        var nextLocation = player.PlayerLocation.Move(direction[0]);

                        if (nextLocation != null)
                        {
                            player.PlayerLocation = nextLocation;
                            Console.WriteLine($"You moved to {nextLocation.Name}.");
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
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    
}