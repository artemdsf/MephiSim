using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class Anatoly : Teacher
{

    [SerializeField] float _shootingCooldown;
    [SerializeField] float _changeDirectionAfterSeconds;

    Vector2 _movementDirection = Vector2.right;

    protected override void Awake()
    {
        StartCoroutine(ShootPerTime());
        StartCoroutine(ChangeDirectionPerTime());
        base.Awake();
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

            int directions = 8;

            for (int i = 0; i < directions; i++)
            {
                Vector2 direction = Quaternion.Euler(0, 0, startAngle + 360 * i / directions) * Vector2.up;
                Weapon.Shoot(direction);
            }

            startAngle = (startAngle + 20) % 360;
        }

    }

    private IEnumerator ChangeDirectionPerTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_changeDirectionAfterSeconds);

        }
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
