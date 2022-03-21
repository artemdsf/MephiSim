using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float _maxDistance = 5;
    [SerializeField] float _speedMult = 10;

    private Player _player;

	private void Start()
	{
        _player = CharactersManager.instance.GetPlayer();

        transform.position = _player.transform.position + Vector3.back;
    }

	private void Update()
    {
        Vector2 playerPos = (Vector2)_player.transform.position;

        if (Vector2.Distance(playerPos, (Vector2)transform.position) > _maxDistance)
        {
            transform.position += (Vector3)(playerPos - (Vector2)transform.position) * _speedMult * Time.deltaTime;
        }
    }
}
