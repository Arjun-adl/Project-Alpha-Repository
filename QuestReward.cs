public static class QuestReward
{
    public static bool HasReward(Quest quest)
    {
        if (quest.RewardWeapon != null)
        {
            return true;
        }

        if (quest.RewardPotion != null)
        {
            return true;
        }

        if (quest.RewardMaximumHitPoints > 0)
        {
            return true;
        }

        return false;
    }

    public static void GiveReward(Player player, Quest quest)
    {
        if (!quest.IsComplete)
        {
            return;
        }

        if (quest.RewardIsClaimed)
        {
            return;
        }

        if (!HasReward(quest))
        {
            return;
        }

        Console.WriteLine($"Your reward is: {quest.GetRewardInfo()}");

        if (quest.RewardWeapon != null)
        {
            player.AddItem(quest.RewardWeapon);
        }

        if (quest.RewardPotion != null)
        {
            player.AddItem(quest.RewardPotion);
        }

        if (quest.RewardMaximumHitPoints > 0)
        {
            player.IncreaseMaximumHitPoints(quest.RewardMaximumHitPoints);
        }

        quest.RewardIsClaimed = true;
    }
}
