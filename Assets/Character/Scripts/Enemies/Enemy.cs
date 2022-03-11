using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected EnemyWeapon WeaponInstance { get; private set; }

    protected override void Start()
    {
        base.Start();
        WeaponInstance = GetComponent<EnemyWeapon>();
    }

    protected virtual void Update()
    {
        
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
