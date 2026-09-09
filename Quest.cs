
using System.Xml.Serialization;

public class Quest
{
	public int ID;
	public string Name;
	public string Description;
	public int status = 0; // 0 = Available, 1 = In Progress, 2 = Completed
	public bool IsCompleted;

	public Quest(int id, string name, string description)
	{
		ID = id;
		Name = name;
		Description = description;
		IsCompleted = false;
	}

	public string GetInfo()
	{
		return $"Quest #{ID}\nName: {Name}\nDescription: {Description}\nStatus: {(status == 0 ? "Available" : status == 1 ? "In Progress" : "Completed")}\n";
	}
	
}
