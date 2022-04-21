using UnityEngine;

public abstract class Character : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _characterBottom;

	[SerializeField] protected Stats Stats;

	public Vector3 Velocity { private set; get; }

	public Vector2 Size => _characterBottom.size;

	public LayerMask CollidableLayerMask { get; private set; }

	private Animator _animator;
	
	private const float CHECK_MULTIPLIER = 0.1f;

	public float MaxHP => Stats.MaxHP;
	public float DefaultDef => Stats.DefaultDef;
	public float DefaultSpeed => Stats.DefaultSpeed;

	public float HP
	{
		get => _hp;
		private set
		{
			if (value < 0)
			{
				_hp = 0;
			}
			else if (value > Stats.MaxHP)
			{
				_hp = Stats.MaxHP;
			}
			else
			{
				_hp = value;
			}
		}
	}
	public float Defense
	{
		get => _defense;
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
	public float Speed
	{
		get => _speed;
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

	private float _hp;
	private float _defense;
	private float _speed;

	public void ResetStats()
	{
		ResetHP();
		ResetDef();
		ResetSpeed();
	}

	public void ResetHP()
	{
		HP = Stats.MaxHP;
	}

	public void ResetDef()
	{
		Defense = Stats.DefaultDef;
	}

	public void ResetSpeed()
	{
		Speed = Stats.DefaultSpeed;
	}

	public virtual void TakeDamage(float damage)
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

	public virtual void Heal(float hp)
	{
		if (hp < 0)
		{
			hp = 0;
		}
		else if (_hp + hp > Stats.MaxHP)
		{
			_hp = Stats.MaxHP;
		}
		else
		{
			_hp += hp;
		}
	}

	public bool IsSpaceFree(Vector2 pos)
	{
		Collider2D collider = Physics2D.OverlapBox(pos, _characterBottom.size, 0, CollidableLayerMask);
		return !collider || collider.isTrigger;
	}

	public void Move(Vector2 direction)
	{
		Vector3 finalDirection = direction.normalized * _speed * Time.deltaTime;

		if (IsSpaceFree(_characterBottom.transform.position + finalDirection + (Vector3)direction.normalized * CHECK_MULTIPLIER) == false)
		{

			if (IsSpaceFree(_characterBottom.transform.position + (finalDirection.x + (direction.normalized * CHECK_MULTIPLIER).x) * Vector3.right) == false)
			{
				finalDirection.x = 0;
			}

			if (IsSpaceFree(_characterBottom.transform.position + (finalDirection.y + (direction.normalized * CHECK_MULTIPLIER).y) * Vector3.up) == false)
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

	public abstract void Die();

	protected virtual void Awake()
	{
		ResetStats();
	}

	protected virtual void Start()
	{
		CollidableLayerMask = LayerMask.GetMask("Obstacle");
		_animator = GetComponent<Animator>();

		CheckParameters();

		ResetDef();
		ResetHP();
		ResetSpeed();
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
		if (Stats.MaxHP < 0)
		{
			Debug.LogError("maxHP parametr cannot have a negative value", this);
			Stats.MaxHP = 0;
		}

		if (Stats.DefaultSpeed < 0)
		{
			Debug.LogError("defaultSpeed parametr cannot have a negative value", this);
			Stats.DefaultSpeed = 0;
		}

		if (Stats.DefaultDef < 0)
		{
			Debug.LogError("defaultSpeed parametr cannot have a negative value", this);
			Stats.DefaultDef = 0;
		}

		if (Stats.DefaultDef > 100)
		{
			Debug.LogError("defaultSpeed parametr cannot exceed 100", this);
			Stats.DefaultDef = 100;
		}
	}
}