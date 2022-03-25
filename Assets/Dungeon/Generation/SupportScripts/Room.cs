using UnityEngine;

public enum RoomType
{
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

	public bool IsRightOpen = false;
	public bool IsLeftOpen = false;
	public bool IsUpOpen = false;
	public bool IsDownOpen = false;

	public bool IsExtreme = true;

	public bool IsVisited = false;

	public int DistanceFromStart;
}