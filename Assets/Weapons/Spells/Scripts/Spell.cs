using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Characters/Spells/Spell")]
public  class Spell : ScriptableObject
{
    public enum SpellType
    { 
        AOE,
        Projectile,
        OnMyself,
        OnCharacter
    }

    public enum EffectType
    {
        SpeedUp,
        Invincibility,
        HPRecovering,
        Poison,
    }

    [SerializeField] private EffectType Effect;

    [SerializeField] private SpellType _type;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _projectileSprite;
    [SerializeField] private AnimatorController _animatorController;
    [SerializeField] private string _spellName;
    [SerializeField] private float _manaCost;

    private Dictionary<EffectType, System.Type> EffectsDict = new Dictionary<EffectType, System.Type>
    {
        {EffectType.SpeedUp, typeof(SpeedUp)},
        {EffectType.Invincibility, typeof(Invincibility)},
        {EffectType.HPRecovering, typeof(HPRecovering)},
        {EffectType.Poison, typeof(Poison)},
    };

    public Effect EffectObject { get; private set; }

    public SpellType Type => _type;
    public Sprite Icon => _icon;
    public Sprite ProjectileSprite => _projectileSprite;
    public AnimatorController AnimatorController => _animatorController;
    public string SpellName => _spellName;
    public float ManaCost => _manaCost;

    private void OnValidate()
    {
        EffectObject = (Effect)System.Activator.CreateInstance(EffectsDict[Effect]);
    }
}


