using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : Enemy
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

}
