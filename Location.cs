public class Location
{
    public int ID;
    public string Name;
    public string Description;
    public int X;
    public int Y;
    public Location? LocationToNorth;
    public Location? LocationToEast;
    public Location? LocationToSouth;
    public Location? LocationToWest;
    public Quest? QuestAvailableHere;
    public Monster? MonsterLivingHere;

    public Location(int id, string name, string description, Location? locationToNorth, Location? locationToEast)
    {
        ID = id;
        Name = name;
        Description = description;
        LocationToNorth = locationToNorth;
        LocationToEast = locationToEast;
    }

    public Location? Move(char direction)
    {
        Location? nextLocation = direction switch
        {
            'N' => LocationToNorth,
            'E' => LocationToEast,
            'S' => LocationToSouth,
            'W' => LocationToWest,
            _ => null
        };

        return nextLocation;
    }

    public void ShowLocationInfo()
    {
        Console.WriteLine($"You are at {Name}.");
        Console.WriteLine(Description);

        if (LocationToNorth != null)
        {
            Console.WriteLine($"To the North is {LocationToNorth.Name}.");
        }

        if (LocationToEast != null)
        {
            Console.WriteLine($"To the East is {LocationToEast.Name}.");
        }

        if (LocationToSouth != null)
        {
            Console.WriteLine($"To the South is {LocationToSouth.Name}.");
        }

        if (LocationToWest != null)
        {
            Console.WriteLine($"To the West is {LocationToWest.Name}.");
        }
    }

    public static void ShowMap(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Map (X = player)");

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("          [");
        PrintMapSymbol(World.LOCATION_ID_ALCHEMISTS_GARDEN, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("          [");
        PrintMapSymbol(World.LOCATION_ID_ALCHEMIST_HUT, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("[");
        PrintMapSymbol(World.LOCATION_ID_FARM_FIELD, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]--[");

        PrintMapSymbol(World.LOCATION_ID_FARMHOUSE, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]--[");

        PrintMapSymbol(World.LOCATION_ID_TOWN_SQUARE, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]--[");

        PrintMapSymbol(World.LOCATION_ID_GUARD_POST, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]--[");

        PrintMapSymbol(World.LOCATION_ID_BRIDGE, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]--[");

        PrintMapSymbol(World.LOCATION_ID_SPIDER_FIELD, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("          [");
        PrintMapSymbol(World.LOCATION_ID_HOME, player);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("]");

        Console.ResetColor();
    }

    public static char MapSymbol(int locationId, Player player)
    {
        if (player.PlayerLocation.ID == locationId)
        {
            return 'X';
        }

        return locationId switch
        {
            World.LOCATION_ID_HOME => 'H',
            World.LOCATION_ID_TOWN_SQUARE => 'T',
            World.LOCATION_ID_GUARD_POST => 'G',
            World.LOCATION_ID_ALCHEMIST_HUT => 'A',
            World.LOCATION_ID_ALCHEMISTS_GARDEN => 'A',
            World.LOCATION_ID_FARMHOUSE => 'F',
            World.LOCATION_ID_FARM_FIELD => 'F',
            World.LOCATION_ID_BRIDGE => 'B',
            World.LOCATION_ID_SPIDER_FIELD => 'S',
            _ => '?'
        };
    }
    public static void PrintMapSymbol(int locationId, Player player)
    {
        char symbol = MapSymbol(locationId, player);

        if (symbol == 'X')
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(symbol);
            Console.ResetColor();
        }
        else
        {
            Console.Write(symbol);
        }
    }
}

