using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float _speed = 10;

	private Transform _player;
	private float _offset;

	private void Start()
	{
		_player = CharactersManager.instance.GetPlayer().transform;
		_offset = transform.position.z;
		transform.position = _player.position + Vector3.forward * transform.position.z;
	}

	private void Update()
	{
		transform.position = (Vector3)Vector2.Lerp(transform.position, _player.position, _speed * Time.deltaTime) + Vector3.forward * _offset;
	}
}
