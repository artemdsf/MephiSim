using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Projectile _projectile;
    [SerializeField] protected Transform _weaponEnd;
    [SerializeField] protected float _speed = 20;

    public Transform WeaponEnd
    {
        get
        {
            return _weaponEnd;
        }
    }

    public virtual void Shoot(Vector2 direction)
    {
        direction = direction.normalized;

        Projectile projectile = Instantiate(_projectile, _weaponEnd.position, Quaternion.LookRotation(Vector3.forward, new Vector2(-direction.y, direction.x)));
        projectile.gameObject.GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }
}
