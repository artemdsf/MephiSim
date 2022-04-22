using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PoolObject))]
public class Projectile : MonoBehaviour
{
	[SerializeField] private float _damage = 50;
	[SerializeField] private string _tagToDealDamage = "Player";

	public float Damage
	{
		set
		{
			if (value < 0)
			{
				_damage = 0;
			}
			else
			{
				_damage = value;
			}
		}

		get
		{
			return _damage;
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "CollideWithProjectiles")
		{
			GetComponent<PoolObject>().ReturnToPool();
		}
		else if (collision.tag == _tagToDealDamage)
		{
			collision.GetComponent<Character>().TakeDamage(Damage);
			GetComponent<PoolObject>().ReturnToPool();
		}
	}
}
