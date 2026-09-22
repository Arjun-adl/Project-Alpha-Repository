public class Quest
{
    public int ID;
    public string Name;
    public string Description;
    public bool IsActive;
    public bool IsComplete;
    public bool RewardClaimed;

    public Quest(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
        IsActive = false;
        IsComplete = false;
        RewardClaimed = false;
    }

    public string GetInfo()
    {
        string status = "Not started";

        if (IsActive)
        {
            status = "Active";
        }

        if (IsComplete)
        {
            status = "Complete";
        }

        return $"{Name} - {Description} [{status}]";
    }

    public int CalculateRewardAmount(int remainingHealth)
    {
        int minimumReward = 20;
        int reward = minimumReward;

        reward += Math.Max(0, remainingHealth * 2);

        return reward;
    }

    public void giveReward(Item reward)
    {
        if (IsComplete && !RewardClaimed)
        {
            Console.WriteLine($"You have received your reward for completing the quest: {reward.Name}");
            RewardClaimed = true;
        }
        else if (RewardClaimed)
        {
            Console.WriteLine($"You have already claimed your reward for the quest: {Name}");
        }
        else
        {
            Console.WriteLine($"You cannot claim the reward for the quest: {Name} because it is not complete.");
        }
    }

    public static void GiveReward(Player player, Quest quest, int remainingHealth = 0)
    {
        if (quest == null || player == null)
        {
            return;
        }

        if (!quest.IsComplete || quest.RewardClaimed)
        {
            if (quest.RewardClaimed)
            {
                Console.WriteLine($"You have already claimed your reward for the quest: {quest.Name}");
            }
            else
            {
                Console.WriteLine($"You cannot claim the reward for the quest: {quest.Name} because it is not complete.");
            }

            return;
        }

        int moneyEarned = quest.CalculateRewardAmount(remainingHealth);
        player.Money += moneyEarned;
        Console.WriteLine($"You earned {moneyEarned} coins for completing {quest.Name}.");

        Item reward = quest.ID switch
        {
            World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN => World.PotionByID(World.POTION_ID_HEALING_POTION),
            World.QUEST_ID_CLEAR_FARMERS_FIELD => World.WeaponByID(World.WEAPON_ID_CLUB),
            World.QUEST_ID_COLLECT_SPIDER_SILK => World.PotionByID(World.POTION_ID_HEALING_POTION),
            _ => null
        };

        if (reward != null)
        {
            player.AddItem(reward);
            quest.giveReward(reward);
        }
        else
        {
            Console.WriteLine($"Quest reward item unavailable for: {quest.Name}");
        }

        quest.RewardClaimed = true;
    }
}