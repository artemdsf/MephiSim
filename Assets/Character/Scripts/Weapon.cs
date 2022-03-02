using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Projectile _projectile;
    [SerializeField]
    private Transform _weaponEnd;
    [SerializeField]
    private float _speed = 20;
    [SerializeField]
    private float _coolDown = 0.5F;

    private float _timeLeft = 0;

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

    protected virtual void Update()
    {
        TimeLeft -= Time.deltaTime;
    }

    public Transform WeaponEnd {
        get 
        {
            return _weaponEnd;
        }
    }

    public void Shoot(Vector2 direction) 
    {
        direction = direction.normalized;

        if (TimeLeft <= 0)
        {
            Projectile projectile = Instantiate(_projectile, _weaponEnd.position, Quaternion.LookRotation(Vector3.forward, new Vector2(-direction.y, direction.x)));
            projectile.gameObject.GetComponent<Rigidbody2D>().velocity = direction * _speed;
            _timeLeft = _coolDown;
        }
    }

    
}
