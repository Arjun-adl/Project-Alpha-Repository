public class Monster
{
    public int ID;
    public string Name;
    public int MinimumDamage;
    public int MaximumDamage;
    public int CurrentHitPoints;
    public int MaximumHitPoints;

    public Monster(int id, string name, int minimumDamage, int maximumDamage, int currentHitPoints, int maximumHitPoints)
    {
        ID = id;
        Name = name;
        MinimumDamage = minimumDamage;
        MaximumDamage = maximumDamage;
        CurrentHitPoints = currentHitPoints;
        MaximumHitPoints = maximumHitPoints;
    }
}

public static class Battle
{
    public const int HIT_CHANCE = 70;
    public const int BLOCK_CHANCE = 15;

    public static bool StartBattle(Player player, Monster monster)
    {
        Console.Clear();

        Console.WriteLine($"A {monster.Name} appears!");
        Console.WriteLine();

        while (monster.CurrentHitPoints > 0 && player.CurrentHitPoints > 0)
        {
            Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}/{monster.MaximumHitPoints}\n");
            Console.WriteLine($"Equipped Weapon: {(player.EquippedWeapon != null ? player.EquippedWeapon.Name : "None")} (DMG: {(player.EquippedWeapon !=  null ? player.EquippedWeapon.Damage : 0)})");
            Console.WriteLine();
            Console.WriteLine($"(Hit chance: {HIT_CHANCE}%, Block chance: {BLOCK_CHANCE}%, Missing chance: {100 - HIT_CHANCE - BLOCK_CHANCE}%)");
            Console.WriteLine($"1. Attack");
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

                string outcome = RollOutcome();
                playerDamage = GetDamageForOutcome(outcome, playerDamage);

                monster.CurrentHitPoints -= playerDamage;

                if (monster.CurrentHitPoints < 0)
                {
                    monster.CurrentHitPoints = 0;
                }

                Console.WriteLine();

                if (outcome == "hit")
                {
                    Console.WriteLine($"You hit the {monster.Name} for {playerDamage} damage.");
                }
                else if (outcome == "blocked")
                {
                    Console.WriteLine($"The {monster.Name} blocked your attack. You deal {playerDamage} damage.");
                }
                else
                {
                    Console.WriteLine($"You missed the {monster.Name}. You deal 0 damage.");
                }

                Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}/{monster.MaximumHitPoints}");
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
                bool usedItem = UseItem(player);

                if (usedItem && player.CurrentHitPoints > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}/{monster.MaximumHitPoints}");
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

    public static string RollOutcome()
    {
        int roll = World.RandomGenerator.Next(1, 101);

        if (roll <= HIT_CHANCE)
        {
            return "hit";
        }

        if (roll <= HIT_CHANCE + BLOCK_CHANCE)
        {
            return "blocked";
        }

        return "miss";
    }

    public static int GetDamageForOutcome(string outcome, int damage)
    {
        if (outcome == "miss")
        {
            return 0;
        }

        if (outcome == "blocked")
        {
            return damage / 2;
        }

        return damage;
    }

    private static void MonsterAttack(Player player, Monster monster)
    {
        int monsterDamage = World.RandomGenerator.Next(
            monster.MinimumDamage,
            monster.MaximumDamage + 1);

        string outcome = RollOutcome();
        monsterDamage = GetDamageForOutcome(outcome, monsterDamage);

        player.CurrentHitPoints -= monsterDamage;

        if (player.CurrentHitPoints < 0)
        {
            player.CurrentHitPoints = 0;
        }

        if (outcome == "hit")
        {
            Console.WriteLine($"The {monster.Name} attacks you for {monsterDamage} damage.");
        }
        else if (outcome == "blocked")
        {
            Console.WriteLine($"You blocked the {monster.Name}'s attack. You take {monsterDamage} damage.");
        }
        else
        {
            Console.WriteLine($"The {monster.Name} missed you. You take 0 damage.");
        }
        Console.WriteLine();
        Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
        Console.WriteLine($"{monster.Name} HP: {monster.CurrentHitPoints}/{monster.MaximumHitPoints}");
        Console.WriteLine();

        if (player.CurrentHitPoints > 0)
        {
            Console.WriteLine("Press any key to continue...");
        }

        Console.ReadKey();
        Console.Clear();
    }

    private static bool UseItem(Player player)
    {
        return player.UseInventoryItem();
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

			Console.WriteLine($"Go to the quest giver to collect your reward.");

            Console.WriteLine($"Quest complete: {quest.Name}");
        }
    }
}