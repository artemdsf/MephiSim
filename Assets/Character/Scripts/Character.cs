using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Character : MonoBehaviour
{
	//Ќам точно нужен конструктор? [SerializeField] нормально работает
    public Character(float maxHP, float defaultSpeed, float defaultDefense, float width)
	{
		_width = width;
		_maxHP = maxHP;
		_defaultSpeed = defaultSpeed;
		_defaultDef = defaultDefense;

		ResetHP();
		ResetDef();
		ResetSpeed();
	}

	public Character() { }

	public virtual void Start() {
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

		_speed = _defaultSpeed;
		HP = _maxHP;
		_defense = _defaultDef;

		GameObject[] collidableTilemaps = GameObject.FindGameObjectsWithTag("CollidableTilemap");

		foreach (GameObject gameObject in collidableTilemaps)
        {
			Tilemap tilemap = gameObject.GetComponent<Tilemap>();

			if (tilemap)
				_collidableTilemaps.Add(tilemap);
        }

		if (_collidableTilemaps.Count == 0)
			Debug.LogWarning("there is no collidable tilemaps in the scene", this);

	}

	[SerializeField]
	private float _defaultDef = 10;
	[SerializeField]
	private float _defaultSpeed = 5;
	[SerializeField]
	private float _maxHP = 100;

	public float HP { get; private set; }
	private float _defense;
	private float _speed;


    public void ResetHP()
    {
		HP = _maxHP;
    }

	public void ResetDef()
    {
		_defense = _defaultDef;
    }

	public void ResetSpeed()
	{
		_speed = _defaultSpeed;
	}

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

	public void TakeDamage(float damage)
	{
		float decreasedHP = damage * (1 - _defense / 100);

		if (damage < 0)
			damage = 0;

		if (HP <= decreasedHP)
		{
			HP = 0;
			Die();
		}
		else
		{
			HP -= decreasedHP;
		}
	}

	public void Heal(float hp)
	{
		if (HP + hp > _maxHP)
		{
			HP = _maxHP;
		}
		else
		{
			HP += hp;
		}
	}

	[SerializeField]
	private Transform _characterBottom;
	private List <Tilemap> _collidableTilemaps = new List<Tilemap>();
	[SerializeField]
	private float _width = 1;

	private bool IsOccupied(Vector3 pos) 
	{
		bool flag = false;

		foreach (Tilemap tilemap in _collidableTilemaps)
		{
			if (tilemap.GetTile(tilemap.WorldToCell(pos)))
			{
				flag = true;
				break;
			}
		}

		return flag;
	}

	public void Move(Vector2 direction) 
	{
		Vector3 finalDirection = direction * Speed * Time.deltaTime;
		Vector3 charRightBorder = Vector3.right * _width / 2;

		bool checkRight = IsOccupied(_characterBottom.position + finalDirection + charRightBorder);
		bool checkLeft = IsOccupied(_characterBottom.position + finalDirection - charRightBorder);

		if (checkLeft || checkRight)
		{
			bool checkRightX = IsOccupied(_characterBottom.position + finalDirection.x * Vector3.right + charRightBorder);
			bool checkLeftX = IsOccupied(_characterBottom.position + finalDirection.x * Vector3.right - charRightBorder);

			if (checkRightX || checkLeftX)
			{
				finalDirection.x = 0;
			}

			bool checkRightY = IsOccupied(_characterBottom.position + finalDirection.y * Vector3.up + charRightBorder);
			bool checkLeftY = IsOccupied(_characterBottom.position + finalDirection.y * Vector3.up - charRightBorder);

			if (checkRightY || checkLeftY)
			{
				finalDirection.y = 0;
			}

			if (finalDirection.x != 0 && finalDirection.y != 0)
			{
				finalDirection = Vector3.zero;
			}
		}

		transform.position += finalDirection;
	}

	public abstract void Die();
}