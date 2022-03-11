using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{

    [SerializeField] private float _coolDown = 0.5F;

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

    public override void Shoot(Vector2 direction) 
    {

        if (TimeLeft <= 0)
        {
            base.Shoot(direction);
            _timeLeft = _coolDown;
        }
    }

    
}
