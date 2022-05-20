using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _player;
	//[SerializeField] private GameObject _boss;

	private Room _bossRoom;

	private void Start()
	{
		Vector3 startPosition = (Vector3)LevelInfo.ChunkSize * 0.5f;
		Instantiate(_player, startPosition, Quaternion.identity);
		
		/*
		_bossRoom = LevelInfo.GetRoomOfType(RoomType.BossRoom);
		GameObject bossInstance = Instantiate(_boss, _bossRoom.CenterPos, Quaternion.identity);
		bossInstance.transform.SetParent(_bossRoom.FindObject(Tag.Enemies).transform);
		*/

	}
}