public class Potion : Item
{
    public int AmountToHeal;

    public Potion(int id, string name, int amountToHeal) : base(id, name)
    {
        AmountToHeal = amountToHeal;
    }
}