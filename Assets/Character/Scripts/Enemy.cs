using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        CharactersManager.instance.DeleteCharacter(this);
        Destroy(gameObject);
    }

}
