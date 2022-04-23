using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] protected List<Spell> _spells;
    [SerializeField] private EffectArea _effectArea;

    public delegate void SpellsChanched(List<Spell> spells);
    public event SpellsChanched OnSpellsChanged;

    public virtual bool AddSpell(Spell spell)
    {
        bool succeed = Check.AddToListWithoutDuplicates(_spells, spell);

        if (succeed)
        {
            OnSpellsChanged(_spells);
        }

        return succeed;
    }

    public int GetSpellsCount()
    {
        return _spells.Count;
    }

    public Spell GetSpell(int index)
    {
        return _spells[index];
    }

    public Spell GetSpell(string name)
    {
        foreach (Spell spell in _spells)
        {
            if (spell.Name == name)
            {
                return spell;
            }

        }

        Debug.LogError($"there is no Spell with name {name}", this);
        return null;
    }

    public void CastAreaSpell(Spell spell, Vector3 target)
    {
        EffectArea effectArea = Instantiate(_effectArea, target, Quaternion.identity);
        effectArea.Effect = spell.EffectObject;
    }
    
}
