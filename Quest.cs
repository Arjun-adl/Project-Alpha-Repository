public class Quest
{
    public int ID;
    public string Name;
    public string Description;
    public bool IsActive;
    public bool IsComplete;

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

        return $"{Name} - {Description} [{status}]";
    }
}