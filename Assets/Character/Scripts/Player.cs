using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        Move(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    public override void Die() 
    {
        Debug.LogWarning("Die() function do nothing yet!", this);
    }
}