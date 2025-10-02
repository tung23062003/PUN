// Represents damage in game logic
public class Damage
{
    // Amount of damage
    public int Amount { get; private set; }

    public Damage(int amount = 0)
    {
        Amount = amount;
    }

    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }

        // If obj can not be cast to Damage
        Damage dmg = obj as Damage;
        if(dmg == null)
        {
            return false;
        }

        return Amount.Equals(dmg.Amount);
    }

    public bool Equals(Damage dmg)
    {
        if(dmg == null)
        {
            return false;
        }

        return Amount.Equals(dmg.Amount);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }
}