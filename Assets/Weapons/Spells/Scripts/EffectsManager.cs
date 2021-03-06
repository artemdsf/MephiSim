using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPrioriy : IComparer<Effect>
{
    public int Compare(Effect effect1, Effect effect2)
    {
        if (effect1.Priority < effect2.Priority)
        {
            return -1;
        }
        else if (effect1.Priority > effect2.Priority)
        {
            return 1;
        }

        return 0;
    }
}

public class EffectsManager : MonoBehaviour
{
    private List<Effect> _effects = new List<Effect>();
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void AddEffect(Effect effect)
    {
            bool isCharacterTarget = false;

        foreach(System.Type type in effect.Targets)
        {
            if (type.IsAssignableFrom(_character.GetType()))
            {
                isCharacterTarget = true;
                break;
            }
        }

        if (!isCharacterTarget)
        {
            return;
        }

        foreach (Effect item in _effects)
        {
            if (effect.GetType() == item.GetType())
            {
                item.RemainingLifetime = item.Lifetime;
                return;
            }
        }

        Effect newEffect = (Effect)System.Activator.CreateInstance(effect.GetType());
        newEffect.Character = _character;
        _effects.Add(newEffect);
        _effects.Sort(new EffectPrioriy());

        newEffect.OnActivate();
    }

    public void RemoveEffect(Effect effect)
    {
        _effects.RemoveAll(item => item.GetType() == effect.GetType());
    }

    public void Update()
    {
        foreach (Effect effect in _effects)
        {
            effect.TimeUntilNextTick -= Time.deltaTime;
            if (effect.TimeUntilNextTick <= 0)
            {
                effect.Tick();
                effect.TimeUntilNextTick = effect.Cooldown;
            }

            effect.RemainingLifetime -= Time.deltaTime;
            if (effect.RemainingLifetime <= 0)
            {
                effect.OnDelete();
            }
        }

        _effects.RemoveAll(effect => effect.RemainingLifetime <= 0);

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log($"Effects on {_character.name}: ");
            foreach (Effect effect in _effects)
            {
                Debug.Log(effect.GetType().Name);
            }
        }
    }

    public float TakingDamage(float damage)
    {
        foreach (Effect effect in _effects)
        {
            damage = effect.TakingDamage(damage);
        }

        return damage;
    }

    public float SpendingMana(float spentMana)
    {
        foreach (Effect effect in _effects)
        {
            spentMana = effect.SpendingMana(spentMana);
        }

        return spentMana;
    }

    public Vector3 Moving(Vector3 direcion, ref float speed)
    {
        foreach(Effect effect in _effects)
        {
            direcion = effect.Moving(direcion, ref speed);
        }

        return direcion;
    }

    public WeaponStats Shooting(WeaponStats weaponStats)
    {
        foreach (var effect in _effects)
        {
            weaponStats = effect.Shooting(weaponStats);
        }

        return weaponStats;
    }

    public float Healing(float hp)
    {
        foreach (var effect in _effects)
        {
            hp = effect.Healing(hp);
        }

        return hp;
    }

    public float RestoringMana(float mana)
    {
        foreach (var effect in _effects)
        {
            mana = effect.RestoringMana(mana);
        }

        return mana;
    }


}
