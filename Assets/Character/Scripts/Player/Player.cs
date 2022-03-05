using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private PlayerWeapon _weapon;

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
        Vector2 MovementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(MovementDirection);

        Enemy nearestEnemy = CharactersManager.instance.FindNearestEnemy(transform.position);
        if (_weapon && nearestEnemy != null && MovementDirection == Vector2.zero)
        {
            _weapon.Shoot(nearestEnemy.transform.position - _weapon.WeaponEnd.position);
        }

    }

    public override void Die() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}