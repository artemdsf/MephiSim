using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 50;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    

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

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        /*
        if (CharactersManager.instance.GetPlayer().transform.position.y <= transform.position.y)
        {
            _renderer.sortingLayerName = "Bullets Top";
        }
        else
        {
            _renderer.sortingLayerName = "Bullets Bottom";
        }
        */

    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collidable Tilemap")
        {
            Destroy(gameObject);
        }
    }
}
