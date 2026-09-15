public class Weapon : Item
{
	public int Damage;

	public Weapon(int id, string name, int damage) : base(id, name)
	{
		Damage = damage;
	}
}