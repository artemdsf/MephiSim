using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected EnemyWeapon Weapon { get; set; }
    protected override void Start()
    {
        base.Start();
        Weapon = GetComponent<EnemyWeapon>();
    }

    protected virtual void Update()
    {
        
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

}
