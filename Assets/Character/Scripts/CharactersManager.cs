using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
	public static CharactersManager instance = null;

	private List<Enemy> _enemies = new List<Enemy>();
	private Player _player;

	public int EnemiesCount()
	{
		return _enemies.Count;
	}

	public int EnemiesCount(Room room)
	{
		int count = 0;
		foreach (var enemy in _enemies)
		{
			if (enemy.Room == room)
			{
				count++;
			}
		}

		return count;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance == this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	}

	public Enemy FindNearestEnemy(Vector3 pos)
	{
		Enemy nearestEnemy;
		float closestDistSqr = 0;

		if (_enemies.Count != 0)
		{
			nearestEnemy = _enemies[0];
			closestDistSqr = Vector3.SqrMagnitude(pos - _enemies[0].transform.position);
		}
		else
		{
			nearestEnemy = null;
		}

		foreach (Enemy enemy in _enemies)
		{
			float distSqr = Vector3.SqrMagnitude(pos - enemy.transform.position);
			if (distSqr < closestDistSqr)
			{
				closestDistSqr = distSqr;
				nearestEnemy = enemy;
			}
		}

		return nearestEnemy;
	}

	public Player GetPlayer()
	{
		return _player;
	}

	public void AddCharacter(Character character)
	{
		if (character is Player)
		{
			_player = character as Player;
		}
		else if (character is Enemy)
		{
			Check.AddToListWithoutDuplicates(_enemies, character as Enemy);
		}
	}

	public void DeleteCharacter(Character character)
	{
		if (character is Player)
		{
			_player = null;
		}
		else if (character is Enemy)
		{
			_enemies.Remove(character as Enemy);
		}
	}
}
