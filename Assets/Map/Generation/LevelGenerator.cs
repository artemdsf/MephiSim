using UnityEngine;

public class LevelGenerator : TileGenerator
{
	[Min(1)]
	[SerializeField] private int _roomCountToSpawn;
	[Range(0, 100)]
	[SerializeField] private int _chanceToChangeRoom = 0;
	[Min(1)]
	[SerializeField] private int _hallwayMaxLenght = 2;

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
		//Generate Start room
		Room _lastRoom = AddStartRoom();

		//Generate Enemy rooms
		for (int roomCount = 0; roomCount < _roomCountToSpawn; roomCount++)
		{
			_lastRoom = AddNewHallway(_lastRoom, _hallwayMaxLenght, _chanceToChangeRoom);
			_lastRoom = AddNewRoomNextToHallway(_lastRoom, RoomType.EnemyRoom);
		}

		//Generate Special rooms
		AddSpecialRoom(RoomType.LibRoom, _hallwayMaxLenght, isBigRoom: false);
		AddSpecialRoom(RoomType.BossRoom, _hallwayMaxLenght);

		GenerateFloor();
	}
}