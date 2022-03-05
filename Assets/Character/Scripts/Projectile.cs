using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 50;

    public float Damage 
    {
        set
        {
            if (value < 0)
            {
                _damage = 0;
            }
            else
            {
                _damage = value;
            }
        }

        get 
        {
            return _damage;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collidable Tilemap")
        {
            Destroy(gameObject);
        }
    }
}
