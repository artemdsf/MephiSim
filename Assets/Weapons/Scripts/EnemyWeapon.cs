using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyWeapon : Weapon
{
    [SerializeField] private List<WeaponStats> _statsList;

    public WeaponStats GetWeaponStats(int index) 
    {
        return _statsList[index];
    }

    public WeaponStats GetWeaponStats(string name)
    {
        foreach (WeaponStats stats in _statsList)
        {
            if (stats.Name == name)
            {
                return stats;
            }
            
        }

        Debug.LogError($"there are no WeaponStats with name {name}", this);
        return null;
    }

    public void Shoot(Vector2 direction)
    {
        Shoot(direction, _statsList[0]);
    }

}
