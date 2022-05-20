using UnityEngine;
using UnityEngine.Events;

public class Enemy : Character
{
	[SerializeField] private bool _isBoss;
	[SerializeField] protected EnemyWeapon Weapon { get; set; }
	[SerializeField] public bool IsBoss => _isBoss;

	public UnityEvent OnEnemyDeath;
	public UnityEvent OnEnemyActivation;

	protected override void Start()
	{
		base.Start();
		Weapon = GetComponent<EnemyWeapon>();
	}

	public override void Die()
	{
		Destroy(gameObject);
	}

    protected override void OnDestroy()
    {
		base.OnDestroy();
		OnEnemyDeath?.Invoke();
	}

    protected override void OnEnable()
    {
		OnEnemyActivation?.Invoke();
        base.OnEnable();
    }
}
