using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 50;
    [SerializeField] private string _tagToDealDamage = "Player";

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
        if (collision.tag == "CollideWithProjectiles")
        {
            Destroy(gameObject);
        }
        else if (collision.tag == _tagToDealDamage)
        {
            collision.GetComponent<Character>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
