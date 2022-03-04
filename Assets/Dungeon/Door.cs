using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _doorCollider;
    [SerializeField] private TaskScript _task;

    private bool alreadyUsed = false;

    private void Start()
    {   

    }

    private void Update()
    {
        if (alreadyUsed && CharactersManager.instance.EnemiesCount() == 0)
        {
            Open();
        }
    }

    private void Close()
    {
        _doorCollider.isTrigger = false;
        _doorCollider.gameObject.layer = LayerMask.NameToLayer("Collidable Tilemap");
        alreadyUsed = true;
    }

    private void Open()
    {
        _doorCollider.isTrigger = true;
        _doorCollider.gameObject.layer = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !alreadyUsed)
        {
            _task.gameObject.SetActive(true);
            Close();
        }
    }


}
