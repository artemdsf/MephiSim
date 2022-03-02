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
    [SerializeField] private Vector2 _size = new Vector2(1, 0.01f);
    [SerializeField] private LayerMask _collidable;
    


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
        CharactersManager.instance.AddCharacter(this);
        CheckParameters();

        ResetDef();
        ResetHP();
        ResetSpeed();
    }

    public void Init(float maxHP, float defaultSpeed, float defaultDefense, Vector2 size)
    {
        _size = size;
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


    public void Move(Vector2 direction)
    {
        Vector3 finalDirection = direction.normalized * _speed * Time.deltaTime;

        Collider2D collider = Physics2D.OverlapBox(_characterBottom.position + finalDirection, _size, 0, _collidable);

        if (collider && !collider.isTrigger)
        {
            Collider2D colliderX = Physics2D.OverlapBox(_characterBottom.position + finalDirection.x * Vector3.right, _size, 0, _collidable);

            if (colliderX && !colliderX.isTrigger)
            {
                finalDirection.x = 0;
            }

            Collider2D colliderY = Physics2D.OverlapBox(_characterBottom.position + finalDirection.y * Vector3.up, _size, 0, _collidable);

            if (colliderY && !colliderY.isTrigger)
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


    public abstract void Die();
}