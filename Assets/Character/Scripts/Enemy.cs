using UnityEngine;

public class Enemy : Character
{
	[SerializeField] protected EnemyWeapon Weapon { get; set; }

	protected override void Start()
	{
		base.Start();
		Weapon = GetComponent<EnemyWeapon>();
	}

	public override void Die()
	{
		Destroy(gameObject);
	}
}
