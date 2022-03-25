using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : TileGenerator
{
	[SerializeField] private int _roomCountToSpawn;
	[Range(0, 100)]
	[SerializeField] private float _chanceToChangeRoom;
	[SerializeField] private int _hallwayMaxLenght;

	private Room _lastRoom;
	private Vector3Int _direction;
	private RoomType _roomType;
	private bool _correctRemove;
	private List<Room> _freeRooms;
	private int _hallwayLenght = 0;

	private void Start()
	{
		grid = GetComponent<Transform>();

		GenerateLevel();
	}

	/// <summary>
	/// Generate level
	/// </summary>
	private void GenerateLevel()
	{
		_lastRoom = AddStartRoom();

		for (int i = 0; i < _roomCountToSpawn; i++)
		{
			SetFreeRoomLists();

			while (true)
			{
				if (_hallwayLenght >= _hallwayMaxLenght)
				{
					_hallwayLenght = 0;
					_roomType = RoomType.EnemyRoom;
					_freeRooms = freeToSpawnHallways;

					_lastRoom = GetRandomRoom(_freeRooms, _lastRoom, _chanceToChangeRoom);
				}
				else if (_hallwayLenght == 0)
				{
					_hallwayLenght++;
					_roomType = RoomType.Hallway;
					_freeRooms = freeToSpawnRooms;

					_lastRoom = GetRandomRoom(_freeRooms, _lastRoom, 0);
				}
				else
				{
					_hallwayLenght++;
					_roomType = RoomType.Hallway;
					_freeRooms = freeToSpawnHallways;

					_lastRoom = GetRandomRoom(_freeRooms, _lastRoom, 0);
				}


				if (_lastRoom == null)
					break;

				_direction = GetRandomDirection(_lastRoom.Position);

				if (_direction != Vector3Int.zero)
				{
					Debug.Log($"{_roomType}");
					_lastRoom = AddNextRoom(_lastRoom, _direction, _roomType);
					break;
				}

				Debug.Log("2");
				_correctRemove = _freeRooms.Remove(_lastRoom);

				if (_correctRemove == false)
					throw new UnityException("Invalid remove");
			}
		}

		CountMapSize();

		GenerateFloor();
	}
}