
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
