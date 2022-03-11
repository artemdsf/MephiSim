using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : Enemy
{
    [SerializeField] protected Weapon Weapon { get; set; }
    protected override void Start()
    {
        base.Start();
        Weapon = GetComponent<Weapon>();
    }

    protected virtual void Update()
    {
        
    }

}
