using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyWeapon))]
public class Hram : Enemy
{	
	private Vector2 _movementDirection = Vector2.right;

	private HramStats _hramStats;

	private void Awake()
	{
		_hramStats = (HramStats)Stats;
	}

	protected override void OnEnable()
	{
		StartCorutines();
		base.OnEnable();
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
			yield return new WaitForSeconds(_hramStats.ShootingCooldown);

			for (int i = 0; i < _hramStats.ShotingDirections; i++)
			{
				Vector2 direction = Quaternion.Euler(0, 0, startAngle + 360 * i / _hramStats.ShotingDirections) * Vector2.up;
				Weapon.Shoot(direction);
			}

			startAngle = (startAngle + _hramStats.AngleOffset) % 360;
		}
	}

	private void Update()
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

	private IEnumerator ChangeDirectionPerTime()
	{
		while (true)
		{
			ChangeDirection();
			yield return new WaitForSeconds(_hramStats.TimeBeforeChangeDirection);
		}
	}
}
