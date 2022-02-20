using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Transform _characterBottom;

    //Default parameters
    [SerializeField] private float _maxHP = 100;
    [SerializeField] private float _defaultDef = 10;
    [SerializeField] private float _defaultSpeed = 5;
    [SerializeField] private float _width = 1;

    private List<Tilemap> _collidableTilemaps = new List<Tilemap>();

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

    public virtual void Start()
    {
        CheckParameters();

        ResetDef();
        ResetHP();
        ResetSpeed();

        FindCollidTiles();
    }

    public void Init(float maxHP, float defaultSpeed, float defaultDefense, float width)
    {
        _width = width;
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
            damage = 0;

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
        Vector3 finalDirection = direction.normalized * _speed * Time.deltaTime;
        Vector3 charRightBorder = Vector3.right * _width / 2;

        bool isOccupiedRight = IsOccupied(_characterBottom.position + finalDirection + charRightBorder);
        bool isOccupiedLeft = IsOccupied(_characterBottom.position + finalDirection - charRightBorder);

        if (isOccupiedLeft || isOccupiedRight)
        {
            bool isOccupiedRightX = IsOccupied(_characterBottom.position + finalDirection.x * Vector3.right + charRightBorder);
            bool isOccupiedLeftX = IsOccupied(_characterBottom.position + finalDirection.x * Vector3.right - charRightBorder);

            if (isOccupiedRightX || isOccupiedLeftX)
            {
                finalDirection.x = 0;
            }

            bool isOccupiedRightY = IsOccupied(_characterBottom.position + finalDirection.y * Vector3.up + charRightBorder);
            bool isOccupiedLeftY = IsOccupied(_characterBottom.position + finalDirection.y * Vector3.up - charRightBorder);

            if (isOccupiedRightY || isOccupiedLeftY)
            {
                finalDirection.y = 0;
            }

            if (finalDirection.x != 0 && finalDirection.y != 0)
            {
                finalDirection.x = 0;
            }
        }

        transform.position += finalDirection;
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

    private void FindCollidTiles()
	{
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

    public abstract void Die();
}