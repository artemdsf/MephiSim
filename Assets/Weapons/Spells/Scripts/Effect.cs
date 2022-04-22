using UnityEngine;

public abstract class Effect
{
    public Character Character;
    public readonly float Cooldown = float.PositiveInfinity;
    public readonly float Lifetime = 0;
    public readonly int Priority = 0;
    public readonly System.Type[] Targets = { typeof(Character) };

    private float _remainingLifetime = 0;
    private float _timeUntilNextTick = 0;

    public float RemainingLifetime 
    { 
        get { return _remainingLifetime; }
        set 
        {
            if (value < 0)
            {
                _remainingLifetime = 0;
                return;
                
            }
            if (value > Lifetime)
            {
                _remainingLifetime = Lifetime;
                return;
            }
            _remainingLifetime = value; 
        }
    }

    public float TimeUntilNextTick
    {
        get { return _timeUntilNextTick; }
        set
        {
            if (value < 0)
            {
                _timeUntilNextTick = 0;
                return;

            }
            if (value > Cooldown)
            {
                _timeUntilNextTick = Cooldown;
                return;
            }
            _timeUntilNextTick = value;
        }
    }

    public Effect() { Reset(); }
    public Effect(float lifetime, float cooldown = float.PositiveInfinity, int priority = 0)
    {
        Lifetime = lifetime;
        Cooldown = cooldown;
        Priority = priority;
        Reset();
    }

    public Effect(float lifetime, System.Type[] targets, float cooldown = float.PositiveInfinity, int priority = 0)
    {
        Lifetime = lifetime;
        Cooldown = cooldown;
        Priority = priority;
        Targets = targets;
        Reset();
    }

    public void Reset()
    {
        TimeUntilNextTick = Cooldown;
        RemainingLifetime = Lifetime;
    }

    public WeaponStats Shooting(WeaponStats weaponStats) { return weaponStats; }
    public virtual float SpendingMana(float spentMana) { return spentMana; }
    public virtual float RestoringMana(float mana) { return mana; }
    public virtual float TakingDamage(float damage) { return damage; }
    public virtual float Healing(float hp) { return hp;}
    public virtual Vector3 Moving(Vector3 direction, ref float speed) { return direction; }
    public virtual void OnActivate() {}
    public virtual void OnDelete() {}
    public virtual void Tick() { Debug.Log(Priority); }

} 
