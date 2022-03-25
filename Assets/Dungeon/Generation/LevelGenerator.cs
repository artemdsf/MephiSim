using UnityEngine;

public class LevelGenerator : TileGenerator
{
	private Room _lastRoom;

	private void Start()
	{
		GenerateLevel();
	}

	/// <summary>
	/// Generate level
	/// </summary>
	private void GenerateLevel()
	{
		_lastRoom = AddStartRoom();

		_lastRoom = AddNextRoom(_lastRoom, Vector3Int.right, RoomType.Hallway);

		GenerateFloor();
	}
}