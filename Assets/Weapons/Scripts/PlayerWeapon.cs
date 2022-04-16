using UnityEngine;

public class PlayerWeapon : Weapon
{
	[SerializeField] private WeaponStats _stats;

	public delegate void WeaponChanged(WeaponStats stats);
	public event WeaponChanged OnWeaponChanging;

	public WeaponStats Stats => _stats;

	private float _timeLeft = 0;

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

	private void Start()
	{
		InitWeapon();
	}

	private void Update()
	{
		_timeLeft -= Time.deltaTime;
	}

	public void GiveWeapon(WeaponStats stats)
	{
		_stats = stats;
		InitWeapon();
	}

	public virtual void Shoot(Vector2 direction)
	{
		if (TimeLeft <= 0 && _stats != null)
		{
			Shoot(direction, _stats);
			_timeLeft = _stats.Cooldown;
		}
	}

	private void InitWeapon()
	{
		if (_stats != null)
		{
			_timeLeft = _stats.Cooldown;
		}

		OnWeaponChanging(_stats);
	}
}
