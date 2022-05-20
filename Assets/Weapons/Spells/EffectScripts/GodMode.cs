
public class GodMode : Effect
{
    public GodMode() : base(float.PositiveInfinity) { }

    public override float TakingDamage(float damage)
    {
        return 0;
    }

    public override float SpendingMana(float spentMana)
    {
        return 0;
    }
}
