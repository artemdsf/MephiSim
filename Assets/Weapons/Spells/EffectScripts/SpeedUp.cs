using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Effect
{
    public SpeedUp() : base(3, 0, 0) { }

    
   public override Vector3 Moving(Vector3 direction, ref float speed)
   {
        speed *= 3;
        return direction;
   }
}
