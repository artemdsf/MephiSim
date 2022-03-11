using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public static CharactersManager instance = null;

    private List<Teacher> _teachers = new List<Teacher>();
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

        InitManager();
    }

  
    private void InitManager()
    {
        
    }


    public Teacher FindNearestTeacher(Vector3 pos) 
    {
        Teacher foundTeacher;
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

        foreach (Teacher teachers in _teachers)
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
        else if (character is Teacher)
        {
           UsefulFuncs.AddToListWithoutDuplicates(_teachers, character as Teacher);
        }
    }

    public void DeleteCharacter(Character character)
    {
        if (character is Player)
        {
            _player = null;
        }
        else if (character is Teacher)
        {
            _teachers.Remove(character as Teacher);
        }
    }
}
