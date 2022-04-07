using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : TileGenerator
{
	[Min(0)]
	[SerializeField] private int _roomCountToSpawn;
	[Range(0, 100)]
	[SerializeField] private int _chanceToChangeRoom = 0;

	private Room _lastRoom;
	private Vector3Int _directionToSpawn;

	private bool _isCorrectRemove;

	private int _hallwayLenght = 0;
	private const int _hallwayMaxLenght = 2;

	private void Awake()
	{
		grid = GetComponent<Transform>();
	}

	private void Start()
	{
		GenerateLevel();
	}

	/// <summary>
	/// Generate level
	/// </summary>
	private void GenerateLevel()
	{
		int maxCountInterations = 1000;
		int countIterations = 0;

		_lastRoom = AddStartRoom();

		for (int roomCount = 0; roomCount < _roomCountToSpawn || _hallwayLenght != 0;)
		{
			while (countIterations < maxCountInterations)
			{
				countIterations++;

				if (_hallwayLenght == 0)
				{
					_lastRoom = GetRandomLastRoom(_lastRoom, _chanceToChangeRoom);

					_directionToSpawn = GetRandomDirection(_lastRoom.Position);

					if (_directionToSpawn == Vector3Int.zero)
					{
						_isCorrectRemove = freeToSpawnRooms.Remove(_lastRoom);

						if (_isCorrectRemove == false)
							throw new UnityException("Invalid remove 1");

						_lastRoom = GetRandomLastRoom(_lastRoom, 100);

						continue;
					}

					_lastRoom = AddNextRoom(_lastRoom, _directionToSpawn, RoomType.Hallway);
					_hallwayLenght++;
					break;
				}
				else if (_hallwayLenght < _hallwayMaxLenght)
				{
					_directionToSpawn = GetRandomDirectionWithFreeArea(_lastRoom.Position);

					if (_directionToSpawn == Vector3Int.zero)
					{
						LevelMap.Map.Remove(_lastRoom);
						_lastRoom = GetRandomLastRoom(_lastRoom, 100);
						_hallwayLenght = 0;
						continue;
					}

					_lastRoom = AddNextRoom(_lastRoom, _directionToSpawn, RoomType.Hallway);
					_hallwayLenght++;
					break;
				}
				else
				{
					_directionToSpawn = GetRandomDirectionWithFreeArea(_lastRoom.Position);

					if (_directionToSpawn == Vector3Int.zero)
					{

						_lastRoom = AddNextRoom(_lastRoom, _directionToSpawn, RoomType.EnemyRoom);
						roomCount++;
						_hallwayLenght = 0;

						_isCorrectRemove = freeToSpawnRooms.Remove(_lastRoom);

						if (_isCorrectRemove == false)
							throw new UnityException("Invalid remove 3");

						_lastRoom = GetRandomLastRoom(_lastRoom, 100);

						break;
					}

					_lastRoom = AddNextRoom(_lastRoom, _directionToSpawn, RoomType.EnemyRoom);
					roomCount++;
					_hallwayLenght = 0;
					break;
				}
			}

			if (countIterations >= maxCountInterations)
			{
				throw new UnityException("A lot of iterations");
			}
		}

		AddSpecialRooms();
		LevelMap.CountMapSize();
		GenerateFloor();
	}

	private void AddSpecialRooms()
	{
		//Add boss room
		foreach (var room in freeToSpawnRooms)
		{
			if (room.IsExtreme && room.RoomType == RoomType.EnemyRoom)
			{
				LevelMap.Map.Remove(room);

				if (CanSpawnBigRoom(room.Position, out Vector3Int lastPosition))
				{
					AddBossRoom(room.Position, lastPosition, room.DistanceFromStart + 1);
					break;
				}

				LevelMap.Map.Add(room);
			}
		}
	}

	private void AddBossRoom(Vector3Int firstPosition, Vector3Int lastPosition, int distance)
	{
		int left, right, up, down;

		if (firstPosition.x < lastPosition.x)
		{
			left = firstPosition.x;
			right = lastPosition.x;
		}
		else
		{
			left = lastPosition.x;
			right = firstPosition.x;
		}
		if (firstPosition.y < lastPosition.y)
		{
			down = firstPosition.y;
			up = lastPosition.y;
		}
		else
		{
			down = lastPosition.y;
			up = firstPosition.y;
		}

		for (int i = left; i <= right; i++)
		{
			for (int j = down; j <= up; j++)
			{
				new Room(new Vector3Int(i, j, 0), RoomType.BossRoom, distance);
			}
		}
	}
}