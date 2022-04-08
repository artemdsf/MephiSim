using UnityEngine;

public class LevelGenerator : TileGenerator
{
	[Min(0)]
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
		Room _lastRoom = AddStartRoom();

		for (int roomCount = 0; roomCount < _roomCountToSpawn; roomCount++)
		{
			_lastRoom = AddNewHallway(_lastRoom, _chanceToChangeRoom, _hallwayMaxLenght);
			_lastRoom = AddNewRoomNextToHallway(_lastRoom, RoomType.EnemyRoom);
		}

		AddSpecialRoom(RoomType.BossRoom);
		AddSpecialRoom(RoomType.LibRoom, isBigRoom:false);

		LevelMap.CountMapSize();
		GenerateFloor();
	}
}