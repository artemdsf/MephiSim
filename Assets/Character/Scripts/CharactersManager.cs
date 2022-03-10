using System.Collections;
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

        InitManager();
    }

  
    private void InitManager()
    {
        
    }


    public Enemy FindNearestEnemy(Vector3 pos) 
    {
        Enemy foundEnemy;
        float closestDistSqr = 0;

        if (_enemies.Count != 0)
        {
            foundEnemy = _enemies[0];
            closestDistSqr = Vector3.SqrMagnitude(pos - _enemies[0].transform.position);
        }
        else
        {
            foundEnemy = null;
        }

        foreach (Enemy enemy in _enemies)
        {
            float distSqr = Vector3.SqrMagnitude(pos - enemy.transform.position);
            if (distSqr < closestDistSqr)
            {
                closestDistSqr = distSqr;
                foundEnemy = enemy;
            }
        }

        return foundEnemy;
    }

    public Player GetPlayer()
    {
        return _player;
    }

    private void AddToList<T>(List<T> list, T element) where T : Character
    {
        if (list.Contains(element) == false)
        {
            list.Add(element);
        }
        else
        {
            Debug.LogError("Adding duplicates of characters is not allowed", element);
        }
    }

    public void AddCharacter(Character character)
    {
        if (character is Player)
        {
            _player = character as Player;
        }
        else if (character is Enemy)
        {
            AddToList(_enemies, character as Enemy);
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
