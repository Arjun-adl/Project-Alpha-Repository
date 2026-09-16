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
    public static bool StartBattle(Monster monster)
    {
        Console.WriteLine();
        Console.WriteLine($"A {monster.Name} appears!");
        Console.WriteLine();

        while (monster.CurrentHitPoints > 0)
        {
            Console.WriteLine($"{monster.Name} has {monster.CurrentHitPoints} HP.");
            Console.WriteLine("Press ENTER to attack.");

            Console.ReadLine();

            int playerDamage = World.RandomGenerator.Next(1, 6);

            monster.CurrentHitPoints -= playerDamage;

            Console.WriteLine($"You hit the {monster.Name} for {playerDamage} damage.");

            if (monster.CurrentHitPoints <= 0)
            {
                Console.WriteLine();
                Console.WriteLine($"You defeated the {monster.Name}!");
                Console.WriteLine();

                CompleteQuest(monster);

                return true;
            }

            int monsterDamage = World.RandomGenerator.Next(
                monster.MinimumDamage,
                monster.MaximumDamage + 1);

            Console.WriteLine(
                $"The {monster.Name} attacks you for {monsterDamage} damage.");
        }

        return false;
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