using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float _maxDistance = 5;
	[SerializeField] private float _speedMult = 10;

	private Player _player;

	private void Start()
	{
		_player = CharactersManager.instance.GetPlayer();

		transform.position = _player.transform.position + Vector3.forward * transform.position.z;
	}

	private void Update()
	{
		Vector2 playerPos = _player.transform.position;

		if (Vector2.Distance(playerPos, transform.position) > _maxDistance)
		{
			transform.position += (Vector3)(playerPos - (Vector2)transform.position) * _speedMult * Time.deltaTime;
		}
	}
}
