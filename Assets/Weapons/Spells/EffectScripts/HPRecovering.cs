
public class HPRecovering : Effect
{
    public HPRecovering() : base(3, 1) {}

    public override void Tick()
    {
        Character.Heal(10);
    }
}
