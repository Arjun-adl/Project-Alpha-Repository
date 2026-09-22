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

    public Location(
        int id,
        string name,
        string description,
        Location? locationToNorth,
        Location? locationToEast)
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
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"===== {Name} =====");
        Console.ResetColor();

        Console.WriteLine(Description);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;

        if (LocationToNorth != null)
        {
            Console.WriteLine(
                $"NORTH: {LocationToNorth.Name}");
        }

        if (LocationToEast != null)
        {
            Console.WriteLine(
                $"EAST: {LocationToEast.Name}");
        }

        if (LocationToSouth != null)
        {
            Console.WriteLine(
                $"SOUTH: {LocationToSouth.Name}");
        }

        if (LocationToWest != null)
        {
            Console.WriteLine(
                $"WEST: {LocationToWest.Name}");
        }

        Console.ResetColor();
    }

    public static void ShowMap(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Map (X = player)");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("          [");
        PrintMapSymbol(
            World.LOCATION_ID_ALCHEMISTS_GARDEN,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("          [");
        PrintMapSymbol(
            World.LOCATION_ID_ALCHEMIST_HUT,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("[");

        PrintMapSymbol(
            World.LOCATION_ID_FARM_FIELD,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("]--[");

        PrintMapSymbol(
            World.LOCATION_ID_FARMHOUSE,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("]--[");

        PrintMapSymbol(
            World.LOCATION_ID_TOWN_SQUARE,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("]--[");

        PrintMapSymbol(
            World.LOCATION_ID_GUARD_POST,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("]--[");

        PrintMapSymbol(
            World.LOCATION_ID_BRIDGE,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("]--[");

        PrintMapSymbol(
            World.LOCATION_ID_SPIDER_FIELD,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("]");

        Console.WriteLine("           |");

        Console.Write("          [");

        PrintMapSymbol(
            World.LOCATION_ID_HOME,
            player);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("]");

        Console.ResetColor();
    }

    public static char MapSymbol(
        int locationId,
        Player player)
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

    public static void PrintMapSymbol(
        int locationId,
        Player player)
    {
        char symbol = MapSymbol(locationId, player);

        if (symbol == 'X')
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(symbol);
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.Write(symbol);
        }
    }

    public void ShowShop(Player player)
    {
        Console.Clear();
        Console.WriteLine("===== TOWN SQUARE SHOP =====");
        Console.WriteLine($"Your money: {player.Money} coins");
        Console.WriteLine("No refunds. All sales final.");
        Console.WriteLine();

        for (int i = 0; i < World.ShopItems.Count; i++)
        {
            var item = World.ShopItems[i];
            Console.WriteLine($"{i + 1}. {item.Item.Name} - {item.Price} coins");
        }

        Console.WriteLine("0. Leave shop");
        Console.Write("Choose an item: ");

        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int choice) || choice < 0 || choice > World.ShopItems.Count)
        {
            Console.WriteLine("Invalid selection.");
            Console.ReadKey();
            return;
        }

        if (choice == 0)
        {
            return;
        }

        var selectedItem = World.ShopItems[choice - 1];

        if (!player.TrySpendMoney(selectedItem.Price))
        {
            Console.WriteLine($"You do not have enough money for {selectedItem.Item.Name}.");
            Console.ReadKey();
            return;
        }

        player.AddItem(selectedItem.Item);
        Console.WriteLine($"You bought {selectedItem.Item.Name} for {selectedItem.Price} coins.");
        Console.ReadKey();
    }
}