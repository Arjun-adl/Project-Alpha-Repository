public class Quest
{
    public int ID;
    public string Name;
    public string Description;
    public bool IsActive;
    public bool IsComplete;
    public Weapon? RewardWeapon;
    public Potion? RewardPotion;
    public int RewardMaximumHitPoints;
    public bool RewardIsClaimed;

    public Quest(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
        IsActive = false;
        IsComplete = false;
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

        return $"{Name} - {Description} [{status}] Reward: {GetRewardInfo()}";
    }

    public string GetRewardInfo()
    {
        string info = "";

        if (RewardWeapon != null)
        {
            info = info + RewardWeapon.Name;
        }

        if (RewardPotion != null)
        {
            if (info != "")
            {
                info = info + " and ";
            }

            info = info + RewardPotion.Name;
        }

        if (RewardMaximumHitPoints > 0)
        {
            if (info != "")
            {
                info = info + " and ";
            }

            info = info + RewardMaximumHitPoints + " extra maximum hit points";
        }

        if (info == "")
        {
            info = "None";
        }

        return info;
    }
}