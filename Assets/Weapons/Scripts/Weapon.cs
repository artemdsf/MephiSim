using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField] private Transform _weaponEnd;

	public Transform WeaponEnd => _weaponEnd;

	public virtual void Shoot(Vector2 direction, WeaponStats stats)
	{
		if (stats == null)
		{
			throw new UnityException($"It looks like no weapon stats has been assigned! {this}");
		}

		direction = direction.normalized;

		GameObject bullet = PoolManager.GetObject(stats.Projectile.name, _weaponEnd.position, Quaternion.LookRotation(Vector3.forward, new Vector2(-direction.y, direction.x)));

		bullet.gameObject.GetComponent<Rigidbody2D>().velocity = direction * stats.ProjectileSpeed;
		bullet.GetComponent<Projectile>().Damage = stats.Damage;
	}
}
