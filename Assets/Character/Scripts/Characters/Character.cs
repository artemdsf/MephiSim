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
    [SerializeField] private float _defaultSpeed = 10;
    [SerializeField] private Vector2 _size = new Vector2(1, 2);

    private LayerMask _collidable;
    private Animator _animator;
    public Vector3 Velocity { private set; get; }
    
    public Vector2 Size 
    {
        get 
        {
            return _size; 
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
        _collidable = LayerMask.GetMask("Collidable Tilemap");
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


    public bool IsSpaceFree(Vector2 pos)
    {
        Collider2D collider = Physics2D.OverlapBox(pos, _size, 0, _collidable);
        return !collider || collider.isTrigger;
    }

    public void Move(Vector2 direction)
    {
        Vector3 finalDirection = direction.normalized * _speed * Time.deltaTime;

        if (!IsSpaceFree(_characterBottom.position + finalDirection))
        {

            if (!IsSpaceFree(_characterBottom.position + finalDirection.x * Vector3.right))
            {
                finalDirection.x = 0;
            }

            if (!IsSpaceFree(_characterBottom.position + finalDirection.y * Vector3.up))
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