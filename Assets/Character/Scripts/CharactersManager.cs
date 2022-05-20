using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactersManager : MonoBehaviour
{
	public bool WereBossesActivated { get; private set;  } = false;
	public UnityEvent OnBossesDeath;
	public static CharactersManager Instance { get; private set; } = null;
	private List<Enemy> _enemies = new List<Enemy>();
	private List<Enemy> _bosses = new List<Enemy>();
	private Player _player;

	public int ActiveEnemiesCount()
	{
		int count = 0;
		foreach (var enemy in _enemies)
		{
			if (enemy.gameObject.activeInHierarchy)
			{
				count++;
			}
		}

		return count;
	}

	public int ActiveEnemiesCount(Room room)
	{
		int count = 0;
		foreach (var enemy in _enemies)
		{
			if (enemy.Room == room && enemy.gameObject.activeInHierarchy)
			{
				count++;
			}
		}

		return count;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance == this)
		{
			Destroy(gameObject);
		}

	}

	public Enemy FindActiveNearestEnemy(Vector3 pos)
	{
		Enemy nearestEnemy = null;
		float closestDistSqr = 0;
		bool found = false;

		foreach (Enemy enemy in _enemies)
		{
			float distSqr = Vector3.SqrMagnitude(pos - enemy.transform.position);
			if ((distSqr < closestDistSqr || found == false) && enemy.gameObject.activeInHierarchy)
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
			if ((character as Enemy).IsBoss)
			{
				Check.AddToListWithoutDuplicates(_bosses, character as Enemy);
				(character as Enemy).OnEnemyDeath.AddListener(AreBossesDead);
			}

			Check.AddToListWithoutDuplicates(_enemies, character as Enemy);
		}
	}

	public void BossActivated()
	{
		WereBossesActivated = true;
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

			if ((character as Enemy).IsBoss)
			{
				_bosses.Remove(character as Enemy);
			}
		}
	}

	private void AreBossesDead()
    {
		int count = 0;
		foreach (Enemy enemy in _bosses)
        {
			if (enemy.gameObject.activeInHierarchy == false)
            {
				count++;
            }
        }

		if (count == _bosses.Count)
        {
			OnBossesDeath?.Invoke();
        }
    }
}
