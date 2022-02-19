using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
	public Character(float maxHP, float speed, float defense)
	{
		_maxHP = maxHP;
		HP = maxHP;
		_speed = speed;
		_defense = defense;
	}

	private float _maxHP;
	private float _speed;
	private float _defense;

	public float HP { get; private set; }

	public float Speed
	{
		get
		{
			return _speed;
		}
		private set
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
		private set
		{
			if (value < 0)
			{
				_defense = 0;
			}
			else if (value > 1)
			{
				_defense = 1;
			}
			else
			{
				_defense = value;
			}
		}
	}

	public void TakeDamage(float damage)
	{
		float decreasedHP = damage * (1 - _defense);
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

	public abstract void Die();
}