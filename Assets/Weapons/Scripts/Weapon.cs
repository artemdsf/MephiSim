using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _weaponEnd;

    public Transform WeaponEnd => _weaponEnd;

    public virtual void Shoot(Vector2 direction, WeaponStats stats)
    {
        if (stats == null)
        {
            Debug.LogError("It looks like no weapon stats has been assigned!", this);
            return;
        }

        direction = direction.normalized;

        GameObject projectileInstance = Instantiate(stats.Projectile, _weaponEnd.position, Quaternion.LookRotation(Vector3.forward, new Vector2(-direction.y, direction.x)));

        projectileInstance.gameObject.GetComponent<Rigidbody2D>().velocity = direction * stats.ProjectileSpeed;
        projectileInstance.GetComponent<Projectile>().Damage = stats.Damage;
    }
}
