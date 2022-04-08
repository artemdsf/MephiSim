using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private PlayerWeapon _weapon;
    private float _mana;

    public float Mana
    {
        get
        {
            return _mana;
        }

        private set 
        {
            if (_mana >= 0)
            {
                _mana = value;
            }
            else
            {
                _mana = 0;
            }
        }
    }

    [SerializeField] private float _maxMana;

    public float MaxMana
    {
        get
        {
            return _maxMana;
        }
    }


    public delegate void PlayerChangedRoom(Room room);
    public event PlayerChangedRoom ChangedRoom;

    protected override void Start()
    {

        _weapon = GetComponent<PlayerWeapon>();
        if (_weapon == null) 
        {
            Debug.LogWarning("There is no weapon script assigned to player", this);
        }

        base.Start();
    }

    private void Update()
    {
        //Movement
        Vector2 MovementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(MovementDirection);
        
        //Shooting
        Teacher nearestEnemy = CharactersManager.instance.FindNearestTeacher(transform.position);

        if (_weapon && nearestEnemy != null && MovementDirection == Vector2.zero)
        {
            _weapon.Shoot(nearestEnemy.transform.position - _weapon.WeaponEnd.position);
        }

        //invoke event
        Room currentRoom = LevelMap.GetRoom(LevelMap.WorldCoordsToGrid(transform.position));
        if (currentRoom != null && currentRoom.IsVisited == false)
        {
            ChangedRoom(currentRoom);
            currentRoom.IsVisited = true;
        }

    }

    public override void Die() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}