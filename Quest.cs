
using System.Xml.Serialization;

public class Quest
{
	public int ID;
	public string Name;
	public string Description;
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
		return $"Quest #{ID}\nName: {Name}\nDescription: {Description}\nStatus: {(IsCompleted ? "Completed" : "Available")}\n";
	}
	
}
