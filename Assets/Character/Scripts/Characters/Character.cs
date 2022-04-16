using UnityEngine;

public abstract class Character : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _characterBottom;

	//Default parameters
	[SerializeField] private float _maxHP = 100;
	[SerializeField] private float _defaultDef = 10;
	[SerializeField] private float _defaultSpeed = 10;

	public float MaxHP
	{
		get
		{
			return _maxHP;
		}
	}
	public float DefaultDef
	{
		get
		{
			return _defaultDef;
		}
	}

	public float DefaultSpeed
	{
		get
		{
			return _defaultSpeed;
		}
	}

	private LayerMask _collidable;
	private Animator _animator;
	public Vector3 Velocity { private set; get; }

	public Vector2 Size
	{
		get
		{
			return _characterBottom.size;
		}
	}

	public LayerMask CollidableLayerMask
	{
		get
		{
			return _collidable;
		}
	}

	public float HP
	{
		get
		{
			return _hp;
		}
		private set
		{
			if (value < 0)
			{
				_hp = 0;
			}
			else if (value > _maxHP)
			{
				_hp = _maxHP;
			}
			else
			{
				_hp = value;
			}
		}
	}

	private float _hp;

	public float Defense
	{
		get
		{
			return _defense;
		}
		set
		{
			if (value < 0)
			{
				_defense = 0;
			}
			else if (value > 100)
			{
				_defense = 100;
			}
			else
			{
				_defense = value;
			}
		}
	}

	private float _defense;

	public float Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			if (value < 0)
			{
				_speed = 0;
			}
			else
			{
				_speed = value;
			}
		}
	}

	private float _speed;

	protected virtual void Start()
	{
		_collidable = LayerMask.GetMask("Obstacle");
		_animator = GetComponent<Animator>();

		CheckParameters();

		ResetDef();
		ResetHP();
		ResetSpeed();
	}

	protected virtual void Awake()
	{
		CharactersManager.instance.AddCharacter(this);
	}

	protected virtual void OnDestroy()
	{
		CharactersManager.instance.DeleteCharacter(this);
	}

	protected virtual void OnEnable()	
    {
		CharactersManager.instance.AddCharacter(this);
	}

	protected virtual void OnDisable()
	{
		CharactersManager.instance.DeleteCharacter(this);
	}

	public void Init(float maxHP, float defaultSpeed, float defaultDefense, Vector2 size)
	{
		_defaultDef = defaultDefense;
		_defaultSpeed = defaultSpeed;
		_maxHP = maxHP;

		ResetHP();
		ResetDef();
		ResetSpeed();
	}

	public void ResetHP()
	{
		HP = _maxHP;
	}

	public void ResetDef()
	{
		Defense = _defaultDef;
	}

	public void ResetSpeed()
	{
		Speed = _defaultSpeed;
	}

	public void TakeDamage(float damage)
	{
		float decreasedHP = damage * (1 - _defense / 100);

		if (damage < 0)
		{
			damage = 0;
		}

		if (_hp <= decreasedHP)
		{
			_hp = 0;
			Die();
		}
		else
		{
			_hp -= decreasedHP;
		}
	}

	public void Heal(float hp)
	{
		if (_hp + hp > _maxHP)
		{
			_hp = _maxHP;
		}
		else
		{
			_hp += hp;
		}
	}


	public bool IsSpaceFree(Vector2 pos)
	{
		Collider2D collider = Physics2D.OverlapBox(pos, _characterBottom.size, 0, _collidable);
		return !collider || collider.isTrigger;
	}

	[SerializeField] private float _checkMultiplier = 2;

	public void Move(Vector2 direction)
	{
		Vector3 finalDirection = direction.normalized * _speed * Time.deltaTime;

		if (IsSpaceFree(_characterBottom.transform.position + finalDirection * _checkMultiplier) == false)
		{

			if (IsSpaceFree(_characterBottom.transform.position + finalDirection.x * Vector3.right * _checkMultiplier) == false)
			{
				finalDirection.x = 0;
			}

			if (IsSpaceFree(_characterBottom.transform.position + finalDirection.y * Vector3.up * _checkMultiplier) == false)
			{
				finalDirection.y = 0;
			}

			if (finalDirection.x != 0 && finalDirection.y != 0)
			{
				finalDirection.x = 0;
			}
		}

		if (_animator != null)
		{
			WalkingAnim(finalDirection);
		}

		Velocity = finalDirection;
		transform.position += finalDirection;
	}

	private void WalkingAnim(Vector2 direction)
	{
		_animator.SetFloat("SpeedX", direction.x / Time.deltaTime);
		_animator.SetFloat("SpeedY", direction.y / Time.deltaTime);
		_animator.SetBool("IsWalking", direction != Vector2.zero);

		if (direction != Vector2.zero)
		{
			_animator.SetFloat("LastNonZeroSpeedX", direction.x / Time.deltaTime);
			_animator.SetFloat("LastNonZeroSpeedY", direction.y / Time.deltaTime);
		}
	}

	private void CheckParameters()
	{
		if (_maxHP < 0)
		{
			Debug.LogError("maxHP parametr cannot have a negative value", this);
			_maxHP = 0;
		}

		if (_defaultSpeed < 0)
		{
			Debug.LogError("defaultSpeed parametr cannot have a negative value", this);
			_defaultSpeed = 0;
		}

		if (_defaultDef < 0)
		{
			Debug.LogError("defaultSpeed parametr cannot have a negative value", this);
			_defaultDef = 0;
		}

		if (_defaultDef > 100)
		{
			Debug.LogError("defaultSpeed parametr cannot exceed 100", this);
			_defaultDef = 100;
		}
	}

	public abstract void Die();
}