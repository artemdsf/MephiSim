using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private List<Projectile> _projectiles = new List<Projectile>();
    [SerializeField] private Transform _weaponEnd;
    [SerializeField] private float _speed = 20;
    [SerializeField] private float _coolDown = 0.5F;

    private float _timeLeft = 0;

    public float Speed 
    { 
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    public float CoolDown
    {
        get
        {
            return _coolDown;
        }

        set
        {
            if (value < 0)
            {
                _coolDown = 0;
            }
            else
            {
                _coolDown = value;
            }
        }
    }

    private float TimeLeft
    {
        get
        {
            return _timeLeft;
        }

        set
        {
            if (value < 0)
            {
                _timeLeft = 0;
            }
            else
            {
                _timeLeft = value;
            }
        }
    }

    public void AddProjectile(Projectile projectile)
    {
        UsefulFuncs.AddToListWithoutDuplicates(_projectiles, projectile);
    }

    public Transform WeaponEnd
    {
        get
        {
            return _weaponEnd;
        }
    }

    public void Update()
    {
        _timeLeft -= Time.deltaTime;
    }

    public virtual void Shoot(Vector2 direction)
    {
        Shoot(direction, _projectiles[0]);
    }

    public virtual void Shoot(Vector2 direction, Projectile projectile)
    {
        if (TimeLeft <= 0)
        {
            direction = direction.normalized;

            Projectile projectileInstance = Instantiate(projectile, _weaponEnd.position, Quaternion.LookRotation(Vector3.forward, new Vector2(-direction.y, direction.x)));
            projectileInstance.gameObject.GetComponent<Rigidbody2D>().velocity = direction * _speed;
            _timeLeft = _coolDown;
        }
    }
}
