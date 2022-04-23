using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectArea : MonoBehaviour
{
    public Effect Effect;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameManager.TagsDictionary[Tag.PlayerBottom] || collision.tag == GameManager.TagsDictionary[Tag.EnemyBottom])
        {
            collision.GetComponentInParent<EffectsManager>().AddEffect(Effect);
        }
    }
}
