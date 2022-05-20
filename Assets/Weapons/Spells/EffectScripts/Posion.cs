
public class Poison : Effect
{
    public Poison() : base(10, 1) { }

    public override void Tick()
    {
        Character.TakeDamage(30);
    }
}