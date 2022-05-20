using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private List<Spell> _spells;
    [SerializeField] private EffectArea _effectArea;

    

    public virtual bool AddSpell(Spell spell)
    {
        return Check.AddToListWithoutDuplicates(_spells, spell);
    }

    public void SetSpell(int index, Spell spell)
    {
        _spells[index] = spell;
    }

    public int GetSpellsCount()
    {
        return _spells.Count;
    }

    public Spell GetSpell(int index)
    {
        if (index >= _spells.Count || index < 0)
        {
            return null;
        }
        return _spells[index];
    }

    public Spell GetSpell(string name)
    {
        foreach (Spell spell in _spells)
        {
            if (spell.SpellName == name)
            {
                return spell;
            }

        }

        return null;
    }

    public void CastAreaSpell(Spell spell, Vector3 target)
    {
        EffectArea effectArea = Instantiate(_effectArea, target, Quaternion.identity);
        effectArea.Effect = spell.EffectObject;
    }

}
