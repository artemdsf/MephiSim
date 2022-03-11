using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : Generate
{
	[SerializeField] int _baseRoomCount = 10;

	public readonly Vector2Int _startPosition = new Vector2Int(_mapSize / 2, _mapSize / 2);

	private List<Room> _rooms = new List<Room>();

	protected override void GenerateMap()
	{
		GenStartRoom();
		
		for (int i = 0; i < _baseRoomCount; i++)
		{
			Debug.Log("Gen");
			if (TryGenNextBaseRoom() == false)
				break;
		}
	}

	private void GenStartRoom()
	{
		Room room = AddNewRoom(_startPosition, RoomType.StartRoom);
		_rooms.Add(room);
	}

	private bool TryGenNextBaseRoom()
	{
		List<Vector2Int> directions = new List<Vector2Int>();

		Room room;

		do
		{
			if (_rooms.Count == 0)
			{
				return false;
			}

			room = _rooms[Random.Range(0, _rooms.Count)];

			if (room.Position.x + 1 < _mapSize - 1 && IsChunkFree(room.Position + Vector2Int.right * 2))
				directions.Add(Vector2Int.right);
			if (room.Position.x - 1 > 0 && IsChunkFree(room.Position + Vector2Int.left * 2))
				directions.Add(Vector2Int.left);
			if (room.Position.y + 1 < _mapSize - 1 && IsChunkFree(room.Position + Vector2Int.up * 2))
				directions.Add(Vector2Int.up);
			if (room.Position.y - 1 > 0 && IsChunkFree(room.Position + Vector2Int.down * 2))
				directions.Add(Vector2Int.down);

			if (directions.Count == 0)
			{
				_rooms.Remove(room);
			}
		} while (directions.Count == 0);

		Vector2Int direction = directions[Random.Range(0, directions.Count)];

		Room newRoom = ConnectNewRoom(room, direction, RoomType.BaseRoom);
		_rooms.Add(newRoom);

		return true;
	}

	private bool IsChunkFree(Vector2Int position)
	{
		Room room = _map[position.x, position.y];
		return room == null;
	}
}
