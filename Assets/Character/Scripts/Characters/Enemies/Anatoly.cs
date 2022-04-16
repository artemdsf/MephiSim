using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWeapon))]
public class Anatoly : Teacher
{
    [SerializeField] float _shootingCooldown;
    [SerializeField] float _timeBeforeChangeDirection;
    [SerializeField] int _shotingDirections = 8;
    [SerializeField] float _angleOffset = 20;

    Vector2 _movementDirection = Vector2.right;

    protected override void Awake()
    {
        StartCorutines();
        base.Awake();
    }

    private void StartCorutines()
    {
        StartCoroutine(ShootPerTime());
        StartCoroutine(ChangeDirectionPerTime());
    }

    private void ChangeDirection()
    {
        _movementDirection = Quaternion.Euler(0, 0, Random.Range(-180, 180)) * _movementDirection;
    }

    private IEnumerator ShootPerTime()
    {
        float startAngle = 0;

        while (true)
        {
            yield return new WaitForSeconds(_shootingCooldown);

            for (int i = 0; i < _shotingDirections; i++)
            {
                Vector2 direction = Quaternion.Euler(0, 0, startAngle + 360 * i / _shotingDirections) * Vector2.up;
                Weapon.Shoot(direction);
            }

            startAngle = (startAngle + _angleOffset) % 360;
        }
    }

    private IEnumerator ChangeDirectionPerTime()
    {
        while (true)
        {
            ChangeDirection();
            yield return new WaitForSeconds(_timeBeforeChangeDirection);
        }
    }

    protected override void OnEnable()
    {
        StartCorutines();
        base.OnEnable();
    }

    protected override void Update()
    {
        if (IsSpaceFree((Vector2)transform.position + _movementDirection))
        {
            Move(_movementDirection);
        }
        else
        {
            ChangeDirection();
        }
    }
}
