using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public enum SpellType
    { 
        AOE,
        Projectile,
        OnMyself,
        OnCharacter
    }

    [SerializeField] private SpellType _type;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _projectileSprite;
    [SerializeField] private string _name;

    public SpellType Type => _type;
    public Sprite Sprite => _sprite;
    public Sprite ProjectileSprite => _projectileSprite;
    public string Name => _name;
}

[CreateAssetMenu(fileName = "New Effect", menuName = "ScriptableObjects/Spells/Effect")]
public class Effect : Spell
{
    [Header("Effect Properties")]
    [SerializeField] private string _effectName;

    public string EffectName => _effectName;
}


