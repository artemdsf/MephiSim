using System.Collections.Generic;
using UnityEngine;

public static class LevelMap
{
	public static readonly int MapSize = 15;
	public static readonly Vector3Int ChunkSize = new Vector3Int(18, 10, 0);
	public static readonly Vector3Int StartPosition = new Vector3Int(MapSize / 2, MapSize / 2, 0);

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

	public static readonly int RightExtremeTilePosition = (MapSize + 1) * ChunkSize.x;
	public static readonly int UpExtremeTilePosition = (MapSize + 1) * ChunkSize.y;
}