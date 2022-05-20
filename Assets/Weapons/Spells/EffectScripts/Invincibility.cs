using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Effect
{
    public Invincibility() : base(5, float.PositiveInfinity, 100) { }

    public override float TakingDamage(float damage)
    {
        return 0;
    }
}

