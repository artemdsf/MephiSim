using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private GameObject _projectile;

    [SerializeField] private string _name;
    [SerializeField] private string _description;

	[SerializeField] private float _damage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _cooldown;

    public Sprite WeaponSprite => _weaponSprite;
    public GameObject Projectile => _projectile;
    
    public string Name => _name;
    public string Description => _description;

	public float Damage => _damage;
    public float ProjectileSpeed => _projectileSpeed;
    public float Cooldown => _cooldown;
}
