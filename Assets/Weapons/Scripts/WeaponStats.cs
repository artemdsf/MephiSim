using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Characters/Weapon")]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private GameObject _projectile;

    [SerializeField] private string _name;
    [SerializeField] private string _description;

	[SerializeField] private float _damage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _distanceFromPlayer = 1;

    public Sprite WeaponSprite => _weaponSprite;
    public GameObject Projectile => _projectile;
    
    public string Name => _name;
    public string Description => _description;

	public float Damage => _damage;
    public float ProjectileSpeed => _projectileSpeed;
    public float Cooldown => _cooldown;
    public float MovementSpeed => _movementSpeed;
    public float DistanceFromPlayer => _distanceFromPlayer;
}
