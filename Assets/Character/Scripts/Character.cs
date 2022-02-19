using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private float _defaultDef = 10;
    [SerializeField] private float _defaultSpeed = 5;
    [SerializeField] private float _maxHP = 100;

    public float HP 
    { 
        get; 
        private set; 
    }

    public float Defense
    {
        get
        {
            return Defense;
        }
        set
        {
            if (value < 0)
            {
                Defense = 0;
            }
            else if (value > 100)
            {
                Defense = 100;
            }
            else
            {
                Defense = value;
            }
        }
    }

    public float Speed
    {
        get
        {
            return Speed;
        }
        set
        {
            if (value < 0)
            {
                Speed = 0;
            }
            else
            {
                Speed = value;
            }
        }
    }

    //Ќам точно нужен конструктор? [SerializeField] нормально работает
    public Character(float maxHP, float defaultSpeed, float defaultDefense, float width)
    {
        _width = width;
        _defaultDef = defaultDefense;
        _defaultSpeed = defaultSpeed;
        _maxHP = maxHP;

        ResetHP();
        ResetDef();
        ResetSpeed();
    }

    public Character() { }

    public virtual void Start()
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

        Speed = _defaultSpeed;
        HP = _maxHP;
        Defense = _defaultDef;

        //Must be changed
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
        float decreasedHP = damage * (1 - Defense / 100);

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

    [SerializeField] private Transform _characterBottom;
    [SerializeField] private float _width = 1;
    private List<Tilemap> _collidableTilemaps = new List<Tilemap>();

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
        Vector3 finalDirection = direction.normalized * Speed * Time.deltaTime;
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

    public abstract void Die();
}