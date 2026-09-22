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
                UseItem(player);

                if (player.CurrentHitPoints > 0)
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

    private static void UseItem(Player player)
    {
        List<Potion> potionsInInventory = new List<Potion>();
        List<Weapon> weaponsInInventory = new List<Weapon>();

        foreach (var kvp in player.inventory)
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
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Items:");

        int index = 1;

        foreach (Potion potion in potionsInInventory)
        {
            Console.WriteLine($"{index}. {potion.Name} x{player.inventory[potion]} (Potion - Healing: {potion.AmountToHeal})");
            index++;
        }

        foreach (Weapon weapon in weaponsInInventory)
        {
            Console.WriteLine($"{index}. {weapon.Name} x{player.inventory[weapon]} (Weapon - Damage: {weapon.Damage})");
            index++;
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

        if (choice < 1 || choice > potionsInInventory.Count + weaponsInInventory.Count)
        {
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            return;
        }

        if (choice <= potionsInInventory.Count)
        {
            Potion selectedPotion = potionsInInventory[choice - 1];

            int oldHP = player.CurrentHitPoints;

            player.CurrentHitPoints += selectedPotion.AmountToHeal;

            if (player.CurrentHitPoints > player.MaximumHitPoints)
            {
                player.CurrentHitPoints = player.MaximumHitPoints;
            }

            int actualHealing = player.CurrentHitPoints - oldHP;


            if(player.inventory[selectedPotion] == 1)
            {
                player.inventory.Remove(selectedPotion);
            }
            else
            {
                player.inventory[selectedPotion]--;
            }

            Console.WriteLine();
            Console.WriteLine($"You used a {selectedPotion.Name}.");
            Console.WriteLine($"You recovered {actualHealing} HP.");
            Console.WriteLine($"Your HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
        }
        else
        {
            Console.WriteLine("You selected a weapon to equip. Are you sure you want to equip it? (Y/N)");
            string confirm = Console.ReadLine().ToUpper();
            if(confirm == null)
            {
                confirm = "";
            }
            if(confirm == "Y")
            {
                int weaponIndex = choice - potionsInInventory.Count - 1;
                Weapon selectedWeapon = weaponsInInventory[weaponIndex];

                player.EquippedWeapon = selectedWeapon;

                Console.WriteLine();
                Console.WriteLine($"You have equipped {selectedWeapon.Name}.");  
            }
            if(confirm == "N")
            {
                return;
            }
        }

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

			Console.WriteLine($"Go to the quest giver to collect your reward.");

            Console.WriteLine($"Quest complete: {quest.Name}");
        }
    }
}