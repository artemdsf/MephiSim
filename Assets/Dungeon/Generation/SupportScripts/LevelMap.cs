using System.Collections.Generic;
using UnityEngine;

public static class LevelMap
{
	public static int LeftChunk = 0;
	public static int RightChunk = 0;
	public static int UpChunk = 0;
	public static int DownChunk = 0;

	public static readonly Vector3Int ChunkSize = new Vector3Int(18, 10, 0);

	public static List<Room> Map = new List<Room>();

	public static Room GetRoom(Vector3Int position)
	{
		foreach (var room in Map)
		{
			if (room.Position == position)
				return room;
		}

		return null;
	}

	public static int LeftPosition()
	{
		return (LeftChunk - 1) * ChunkSize.x;
	}
	public static int RightPosition()
	{
		return (RightChunk + 2) * ChunkSize.x;
	}
	public static int DownPosition()
	{
		return (DownChunk - 1) * ChunkSize.y;
	}
	public static int UpPosition()
	{
		return (UpChunk + 2) * ChunkSize.y;
	}
	public static Vector3Int WorldCoordsToGrid(Vector3 pos)
	{
		return new Vector3Int((int)(pos.x / ChunkSize.x), (int)(pos.y / ChunkSize.y), 0);
	}

	public static Vector3 GridCoordsToWorld(Vector3Int pos)
	{
		return new Vector3(pos.x * ChunkSize.x, pos.y * ChunkSize.y, 0);
	}

	public static Vector3 GetChunkCenter(Vector3Int posInGrid)
	{
		return GridCoordsToWorld(posInGrid) + new Vector3(ChunkSize.x, ChunkSize.y) / 2;
	}
}