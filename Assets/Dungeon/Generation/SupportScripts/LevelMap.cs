using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelMap
{
	public static readonly int MapSize = 15;
	public static readonly Vector3Int ChunkSize = new Vector3Int(18, 10, 0);
	public static readonly Vector2Int StartPosition = new Vector2Int(MapSize / 2, MapSize / 2);

	public static List<Room> Map = new List<Room>();

	public static Room GetRoom(Vector2Int position)
	{
		foreach (var room in Map)
		{
			if (room.Position == position)
				return room;
		}

		return null;
	}

	public static readonly int RightExtremeTilePosition = (MapSize + 1) * ChunkSize.x;
	public static readonly int UpExtremeTilePosition = (MapSize + 1) * ChunkSize.y;
}