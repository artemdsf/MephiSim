using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
	EmptyRoom,
	BaseRoom,
	StartRoom,
	BossRoom,
	HallwayHoriz,
	HallwayVertic
}

public enum RoomType_new
{
	BaseRoom,
	DoubleRoom,
	Hallway
}

public class Room
{
	public Room()
	{

	}

	public RoomType RoomType = RoomType.EmptyRoom;
	public Vector2Int Position = Vector2Int.one * -1;

	public int DistanceFromStart = 0;
	public bool IsExtreme = true;

	private void Generate()
	{

	}
}