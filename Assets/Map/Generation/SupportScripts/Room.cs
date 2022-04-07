using UnityEngine;

public enum RoomType
{
	BossRoom,
	StartRoom,
	EnemyRoom,
	Hallway
}

public class Room
{
	public Room(Vector3Int position, RoomType roomType, int distance)
	{
		Position = position;
		DistanceFromStart = distance;
		RoomType = roomType;

		LevelMap.Map.Add(this);
	}

	public Vector3 CenterPos 
	{
		get
		{
			return LevelMap.GetChunkCenter(Position);
		}
	}

	public Vector3Int Position;
	public RoomType RoomType;

	public bool IsExtreme = true;

	public bool IsVisited = false;

	public int DistanceFromStart { get; }
}