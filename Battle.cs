public class Monster
{
    public int ID;
    public string Name;
    public int MinimumDamage;
    public int MaximumDamage;
    public int CurrentHitPoints;

    public Monster(int id, string name, int minimumDamage, int maximumDamage, int currentHitPoints)
    {
        ID = id;
        Name = name;
        MinimumDamage = minimumDamage;
        MaximumDamage = maximumDamage;
        CurrentHitPoints = currentHitPoints;
    }
}

public static class Battle
{
    public static bool StartBattle(Player player, Monster monster)
    {
        Console.Clear();

        Console.WriteLine($"A {monster.Name} appears!");
        Console.WriteLine();

        while (monster.CurrentHitPoints > 0 && player.CurrentHitPoints > 0)
        {
            Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}");
            Console.WriteLine();
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Use Item");
            Console.WriteLine("3. Flee");
            Console.WriteLine();

            string? choice = Console.ReadLine();

            if (choice == null)
            {
                choice = "";
            }

            if (choice == "1")
            {
                int playerDamage = 1;

                if (player.EquippedWeapon != null)
                {
                    playerDamage = player.EquippedWeapon.Damage;
                }
                else
                {
                    playerDamage = World.RandomGenerator.Next(1, 6);
                }

                monster.CurrentHitPoints -= playerDamage;

                if (monster.CurrentHitPoints < 0)
                {
                    monster.CurrentHitPoints = 0;
                }

                Console.WriteLine();
                Console.WriteLine($"You hit the {monster.Name} for {playerDamage} damage.");
                Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}");
                Console.WriteLine();

                if (monster.CurrentHitPoints <= 0)
                {
                    Console.WriteLine($"You defeated the {monster.Name}!");

                    CompleteQuest(monster);

                    Console.ReadKey();
                    return true;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();

                MonsterAttack(player, monster);
            }
            else if (choice == "2")
            {
                UseItem(player);

                if (player.CurrentHitPoints > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}");
                    Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
                    Console.WriteLine();

                    MonsterAttack(player, monster);
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine();
                Console.WriteLine($"You fled from the {monster.Name}.");
                Console.ReadKey();
                return false;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                Console.Clear();
            }

            if (player.CurrentHitPoints <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("You were defeated.");
                Console.WriteLine("You return home.");

                player.PlayerLocation = World.LocationByID(World.LOCATION_ID_HOME);

                Console.ReadKey();
                return false;
            }
        }

        return false;
    }

    private static void MonsterAttack(Player player, Monster monster)
    {
        int monsterDamage = World.RandomGenerator.Next(
            monster.MinimumDamage,
            monster.MaximumDamage + 1);

        player.CurrentHitPoints -= monsterDamage;

        if (player.CurrentHitPoints < 0)
        {
            player.CurrentHitPoints = 0;
        }

        Console.WriteLine($"The {monster.Name} attacks you for {monsterDamage} damage.");
        Console.WriteLine();
        Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
        Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}");
        Console.WriteLine();

        if (player.CurrentHitPoints > 0)
        {
            Console.WriteLine("Press any key to continue...");
        }

        Console.ReadKey();
        Console.Clear();
    }

    private static void UseItem(Player player)
    {
        List<Potion> potionsInInventory = new List<Potion>();

        foreach (var kvp in player.inventory)
        {
            if (kvp.Key is Potion potion && kvp.Value > 0)
            {
                potionsInInventory.Add(potion);
            }
        }

        if (potionsInInventory.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("You have no usable items.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Items:");

        for (int i = 0; i < potionsInInventory.Count; i++)
        {
            Potion potion = potionsInInventory[i];
            Console.WriteLine($"{i + 1}. {potion.Name} x{player.inventory[potion]}");
        }

        Console.WriteLine("0. Cancel");

        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            return;
        }

        if (choice == 0)
        {
            return;
        }

        if (choice < 1 || choice > potionsInInventory.Count)
        {
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            return;
        }

        Potion selectedPotion = potionsInInventory[choice - 1];

        int oldHP = player.CurrentHitPoints;

        player.CurrentHitPoints += selectedPotion.AmountToHeal;

        if (player.CurrentHitPoints > player.MaximumHitPoints)
        {
            player.CurrentHitPoints = player.MaximumHitPoints;
        }

        int actualHealing = player.CurrentHitPoints - oldHP;

        player.inventory[selectedPotion]--;

        Console.WriteLine();
        Console.WriteLine($"You used a {selectedPotion.Name}.");
        Console.WriteLine($"You recovered {actualHealing} HP.");
        Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
        Console.ReadKey();
        Console.Clear();
    }

    private static void CompleteQuest(Monster monster)
    {
        Quest? quest = null;

        if (monster.ID == World.MONSTER_ID_RAT)
        {
            quest = World.QuestByID(World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN);
        }
        else if (monster.ID == World.MONSTER_ID_SNAKE)
        {
            quest = World.QuestByID(World.QUEST_ID_CLEAR_FARMERS_FIELD);
        }
        else if (monster.ID == World.MONSTER_ID_GIANT_SPIDER)
        {
            quest = World.QuestByID(World.QUEST_ID_COLLECT_SPIDER_SILK);
        }

        if (quest != null && quest.IsActive)
        {
            quest.IsActive = false;
            quest.IsComplete = true;

            Console.WriteLine($"Quest complete: {quest.Name}");
        }
    }
}