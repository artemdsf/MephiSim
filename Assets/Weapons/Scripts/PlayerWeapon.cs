using UnityEngine;

public class PlayerWeapon : Weapon
{
	[SerializeField] private WeaponStats _stats;
	[SerializeField] private GameObject _weaponPrefab;

	public delegate void WeaponChanged(WeaponStats stats);
	public event WeaponChanged OnWeaponChanging;
	public WeaponStats Stats => _stats;

	private GameObject _weaponGameObj;

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
		if (_stats == null)
        {
			return;
        }

		if (TimeLeft <= 0)
		{
			Shoot(direction + (Vector2)(transform.position - _weaponGameObj.transform.position), _stats, _weaponGameObj.transform);
			_timeLeft = _stats.Cooldown;
		}

		Vector3 weaponGameObjDestination = transform.position + (new Vector3(direction.x, direction.y, 0)).normalized * _stats.DistanceFromPlayer;
		MoveWeapon(weaponGameObjDestination);
	}

	public virtual void Follow(Vector3 direction)
    {
		Vector3 weaponGameObjDestination = transform.position + direction.normalized * _stats.DistanceFromPlayer;
		MoveWeapon(weaponGameObjDestination);
	}

	private void MoveWeapon(Vector3 destination)
    {
		_weaponGameObj.transform.position += (destination - _weaponGameObj.transform.position) * Time.deltaTime * _stats.MovementSpeed;
	}

    private void InitWeapon()
	{
		if (_stats != null)
		{
			_timeLeft = _stats.Cooldown;
		}

		if (_weaponGameObj == null)
        {
			_weaponGameObj = Instantiate(_weaponPrefab);
		}

		_weaponGameObj.GetComponent<SpriteRenderer>().sprite = _stats.WeaponSprite;


		OnWeaponChanging(_stats);
	}
}
