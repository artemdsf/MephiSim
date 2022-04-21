using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
	public static CharactersManager instance = null;

	private List<Enemy> _teachers = new List<Enemy>();
	private Player _player;

	public int TeachersCount()
	{
		return _teachers.Count;
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
		Enemy foundTeacher;
		float closestDistSqr = 0;

		if (_teachers.Count != 0)
		{
			foundTeacher = _teachers[0];
			closestDistSqr = Vector3.SqrMagnitude(pos - _teachers[0].transform.position);
		}
		else
		{
			foundTeacher = null;
		}

		foreach (Enemy teachers in _teachers)
		{
			float distSqr = Vector3.SqrMagnitude(pos - teachers.transform.position);
			if (distSqr < closestDistSqr)
			{
				closestDistSqr = distSqr;
				foundTeacher = teachers;
			}
		}

		return foundTeacher;
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
			Check.AddToListWithoutDuplicates(_teachers, character as Enemy);
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
			_teachers.Remove(character as Enemy);
		}
	}
}
