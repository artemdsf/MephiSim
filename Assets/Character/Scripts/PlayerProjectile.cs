using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().TakeDamage(base.Damage);
            Destroy(gameObject);
        }

        base.OnTriggerEnter2D(collision);
    }
}
