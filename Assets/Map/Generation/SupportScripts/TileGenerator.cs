using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TileWithID
{
	public TileWithID(Tile tile, int[] arr)
	{
		this.tile = tile;
		id = arr;
	}

	public Tile tile;
	public int[] id = new int[9];
}

public class TileGenerator : MonoBehaviour
{
	[SerializeField] private RoomPalette _roomPalette;
	[SerializeField] private RoomPrefabContainer _roomPrefabContainer;

	[SerializeField] private Tilemap _FloorTilemap;
	[SerializeField] private Tilemap _WallTilemap;
	[SerializeField] private Tilemap _CollidWallTilemap;

	[SerializeField] private Light2D _light;

	[SerializeField] private Vector2Int _bigRoomSize = new Vector2Int(1, 1);

	[SerializeField] private GameObject _rooms;

	/// <summary>
	/// Grid for generation
	/// </summary>
	protected Transform grid;
	/// <summary>
	/// List of rooms that can connect new room
	/// </summary>
	protected List<Room> freeToSpawnRooms = new List<Room>();

	private List<TileWithID> _wallTiles = new List<TileWithID>();
	private List<Tile> _nonCollidWallTiles = new List<Tile>();
	private List<Tile> _floorTiles = new List<Tile>();

	#region Generation
	protected Room AddStartRoom()
	{
		Room room = new Room(Vector3Int.zero, RoomType.StartRoom, 0);

		freeToSpawnRooms.Add(room);

		return room;
	}

	protected Room AddNextRoom(Room lastRoom, Vector3Int direction, RoomType roomType)
	{
		int distance = lastRoom.DistanceFromStart + 1;
		lastRoom.IsLast = false;

		if (IsDirectionCorrect(direction) == false)
		{
			throw new UnityException("Invalid direction");
		}
		if (roomType == RoomType.StartRoom)
		{
			throw new UnityException("Can't spawn second start room");
		}

		Room room = new Room(lastRoom.Position + direction, roomType, distance);

		if (roomType != RoomType.Hallway)
		{
			freeToSpawnRooms.Add(room);
		}

		return room;
	}

	protected Room AddNewHallway(Room lastRoom, int hallwayMaxLenght, int chanceToChangeRoom = 100)
	{
		int maxCountInterations = 10000;
		int countIterations = 0;

		bool isCorrectRemove;
		Vector3Int directionToSpawn;
		Room lastHallway = null;
		Room newHallway = null;

		while (countIterations < maxCountInterations)
		{
			countIterations++;

			lastRoom = GetRandomRoom(lastRoom, chanceToChangeRoom);

			directionToSpawn = GetRandomDirection(lastRoom.Position);

			if (directionToSpawn == Vector3Int.zero || CountRoomsInArea(lastRoom.Position + directionToSpawn) > 1)
			{
				isCorrectRemove = freeToSpawnRooms.Remove(lastRoom);

				if (isCorrectRemove == false)
				{
					throw new UnityException("Invalid remove");
				}

				lastRoom = GetRandomRoom(lastRoom);

				continue;
			}

			newHallway = AddNextRoom(lastRoom, directionToSpawn, RoomType.Hallway);
			break;
		}

		if (countIterations >= maxCountInterations)
		{
			throw new UnityException("A lot of iterations");
		}
		if (newHallway == null)
		{
			throw new UnityException("Hallway not spawned");
		}

		for (int hallwayLenght = 1; hallwayLenght < hallwayMaxLenght; hallwayLenght++)
		{
			directionToSpawn = GetRandomDirectionWithFreeArea(newHallway.Position);

			if (directionToSpawn == Vector3Int.zero)
			{
				LevelMap.Map.Remove(newHallway);

				if (hallwayLenght == 1)
				{
					return null;
				}
				return lastHallway;
			}

			lastHallway = newHallway;
			newHallway = AddNextRoom(newHallway, directionToSpawn, RoomType.Hallway);
		}

		return newHallway;
	}

	protected Room AddNewHallwayInTheDirection(Room lastRoom, int hallwayMaxLenght, Vector3Int direction)
	{
		Room lastHallway = null;
		Room newHallway = null;

		newHallway = AddNextRoom(lastRoom, direction, RoomType.Hallway);

		for (int hallwayLenght = 1; hallwayLenght < hallwayMaxLenght; hallwayLenght++)
		{
			lastHallway = newHallway;
			newHallway = AddNextRoom(newHallway, direction, RoomType.Hallway);
		}

		return newHallway;
	}

	protected Room AddNewRoomNextToHallway(Room hallwayRoom, RoomType roomType)
	{
		bool isCorrectRemove;
		Vector3Int directionToSpawn;
		Room newRoom;

		if (hallwayRoom == null)
		{
			return null;
		}
		if (roomType == RoomType.Hallway)
		{
			throw new UnityException("Wrong room type");
		}

		directionToSpawn = GetRandomDirectionWithFreeArea(hallwayRoom.Position);

		if (directionToSpawn == Vector3Int.zero)
		{
			newRoom = AddNextRoom(hallwayRoom, directionToSpawn, roomType);

			isCorrectRemove = freeToSpawnRooms.Remove(newRoom);

			if (isCorrectRemove == false)
			{
				throw new UnityException("Invalid remove");
			}

			newRoom = GetRandomRoom(newRoom);
			return newRoom;
		}

		newRoom = AddNextRoom(hallwayRoom, directionToSpawn, roomType);
		return newRoom;
	}

	protected void AddSpecialRoom(RoomType roomType, int hallwayMaxLenght, bool isBigRoom = true)
	{
		if (isBigRoom)
		{
			bool isSpawned = false;
			foreach (var room in freeToSpawnRooms)
			{
				if (room.IsLast && room.RoomType == RoomType.EnemyRoom)
				{
					LevelMap.Map.Remove(room);

					if (CanSpawnBigRoom(room.Position, out Vector3Int lastPosition))
					{
						AddBigRoom(room.Position, lastPosition, roomType, room.DistanceFromStart + 1);
						isSpawned = true;
						break;
					}
				}
			}

			if (isSpawned == false)
			{
				LevelMap.CountMapSize();

				Room room = GetExtremeRoom(out Vector3Int direction);

				room = AddNewHallwayInTheDirection(room, hallwayMaxLenght, direction);

				if (CanSpawnBigRoom(room.Position + direction, out Vector3Int lastPosition))
				{
					AddBigRoom(room.Position + direction, lastPosition, roomType, room.DistanceFromStart + 1);
					isSpawned = true;
				}
			}
			if (isSpawned == false)
			{
				throw new UnityException("Spawn big room FAILED");
			}
		}
		else
		{
			Room room = GetRandomLastRoom();

			if (room == null)
			{
				room = AddNewHallway(room, hallwayMaxLenght);
				room = AddNewRoomNextToHallway(room, roomType);
			}
			else
			{
				LevelMap.Map.Remove(room);
				room = new Room(room.Position, roomType, room.DistanceFromStart);
			}

			room.IsLast = false;
		}
	}

	protected void AddBigRoom(Vector3Int firstPosition, Vector3Int lastPosition, RoomType roomType, int distance)
	{
		int left, right, up, down;

		if (firstPosition.x < lastPosition.x)
		{
			left = firstPosition.x;
			right = lastPosition.x;
		}
		else
		{
			left = lastPosition.x;
			right = firstPosition.x;
		}
		if (firstPosition.y < lastPosition.y)
		{
			down = firstPosition.y;
			up = lastPosition.y;
		}
		else
		{
			down = lastPosition.y;
			up = firstPosition.y;
		}

		for (int i = left; i <= right; i++)
		{
			for (int j = down; j <= up; j++)
			{
				Room room = new Room(new Vector3Int(i, j, 0), roomType, distance);
				room.IsLast = false;
			}
		}
	}

	protected void GenerateFloor()
	{
		LevelMap.CountMapSize();

		CreateTiles();

		foreach (var room in LevelMap.Map)
		{
			GenerateRoom(room);
		}

		SetSkirtingBoards();
		SetWallTiles();
		SetCollidWallTiles();
	}

	private void GenerateRoom(Room room)
	{
		switch (room.RoomType)
		{
			case RoomType.LibRoom:
				CopyRoomPrefab(room, GetRoomPrefab(room, _roomPrefabContainer.LibRooms));
				break;
			case RoomType.BossRoom:
				CopyRoomPrefab(room, GetRoomPrefab(room, _roomPrefabContainer.BossRooms));
				break;
			case RoomType.StartRoom:
				CopyRoomPrefab(room, GetRoomPrefab(room, _roomPrefabContainer.StartRooms));
				break;
			case RoomType.EnemyRoom:
				CopyRoomPrefab(room, GetRoomPrefab(room, _roomPrefabContainer.EnemyRooms));
				break;
			case RoomType.Hallway:
				CopyRoomPrefab(room, GetRoomPrefab(room, _roomPrefabContainer.Hallways));
				break;
			default:
				throw new UnityException("Invalid room type");
		}
	}

	private void CopyRoomPrefab(Room room, RoomPrefab prefab)
	{
		if (prefab == null)
		{
			throw new UnityException("Prefab not found");
		}

		Vector3Int tilePositionInPrefab;
		Vector3Int tilePositionInRoom;

		Tilemap tilemap = prefab.Room.GetComponent<Tilemap>();

		if (tilemap == null)
		{
			throw new UnityException("Tilemap is null");
		}

		for (int i = 0; i < LevelMap.ChunkSize.x; i++)
		{
			for (int j = 0; j < LevelMap.ChunkSize.y; j++)
			{
				tilePositionInPrefab = prefab.LeftDownCorner + new Vector3Int(i, j, 0);
				if (tilemap.HasTile(tilePositionInPrefab))
				{
					tilePositionInRoom = room.Position * LevelMap.ChunkSize + new Vector3Int(i, j, 0);
					_FloorTilemap.SetTile(tilePositionInRoom, GetFloorTile(i, j));
				}
			}
		}

		room.Light = _light;

		GameObject emptyObject = new GameObject($"{room.RoomType}:{room.Position}");
		emptyObject.transform.SetParent(_rooms.transform);

		Vector3 roomPosition;

		foreach (Transform child in prefab.Room.transform)
		{
			roomPosition = room.Position * LevelMap.ChunkSize + child.position - prefab.LeftDownCorner;

			GameObject gameObject = Instantiate(child.gameObject, roomPosition, Quaternion.identity, emptyObject.transform);
			gameObject.tag = child.gameObject.tag;

			if (gameObject.tag == LevelManager.TagsDictionary[Tag.Enemies] ||
				gameObject.tag == LevelManager.TagsDictionary[Tag.Lights])
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
			}

			room.Content.Add(gameObject);
		}
	}

	private void SetSkirtingBoards()
	{
		int left = LevelMap.LeftPosition;
		int right = LevelMap.RightPosition;
		int down = LevelMap.DownPosition;
		int up = LevelMap.UpPosition;

		for (int i = left; i <= right; i++)
		{
			for (int j = down; j <= up; j++)
			{
				int[] id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 1)
				{
					_FloorTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
	}

	private void SetWallTiles()
	{
		int left = LevelMap.LeftPosition;
		int right = LevelMap.RightPosition;
		int down = LevelMap.DownPosition;
		int up = LevelMap.UpPosition;

		for (int i = right; i >= left; i--)
		{
			for (int j = up; j >= down; j--)
			{
				if (Has(new Vector3Int(i, j, 0)) && HasUp(new Vector3Int(i, j, 0)) == false)
				{
					if (i % 2 == 0)
					{
						_WallTilemap.SetTile(new Vector3Int(i, j + 2, 0), _nonCollidWallTiles[0]);
						_WallTilemap.SetTile(new Vector3Int(i, j + 1, 0), _nonCollidWallTiles[2]);
					}
					else
					{
						_WallTilemap.SetTile(new Vector3Int(i, j + 2, 0), _nonCollidWallTiles[1]);
						_WallTilemap.SetTile(new Vector3Int(i, j + 1, 0), _nonCollidWallTiles[3]);
					}
				}
			}
		}
	}

	private void SetCollidWallTiles()
	{
		int[] id = new int[9];

		int left = LevelMap.LeftPosition;
		int right = LevelMap.RightPosition;
		int down = LevelMap.DownPosition;
		int up = LevelMap.UpPosition;

		for (int i = left; i <= right; i++)
		{
			for (int j = down; j <= up; j++)
			{
				id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 0)
				{
					_CollidWallTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
	}

	private Vector3Int GetRandomDirectionWithFreeArea(Vector3Int position)
	{
		Vector3Int[] directions = { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

		directions = directions.Where(direction => IsAreaFree(direction + position)).ToArray();

		if (directions.Length > 0)
		{
			return directions[Random.Range(0, directions.Length)];
		}

		return Vector3Int.zero;
	}

	private Vector3Int GetRandomDirection(Vector3Int position)
	{
		Vector3Int[] directions = { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

		directions = directions.Where(direction => IsChunkFree(direction + position)).ToArray();

		if (directions.Length > 0)
		{
			return directions[Random.Range(0, directions.Length)];
		}

		return Vector3Int.zero;
	}

	private Room GetRandomRoom(Room lastRoom, int chanceToChangeLastRoom = 100)
	{
		if (lastRoom != null && Random.Range(1, 100) >= chanceToChangeLastRoom)
		{
			foreach (var item in freeToSpawnRooms)
			{
				if (lastRoom.Position == item.Position)
				{
					return lastRoom;
				}
			}
		}

		if (freeToSpawnRooms.Count > 0)
		{
			return freeToSpawnRooms[Random.Range(0, freeToSpawnRooms.Count)];
		}

		return null;
	}

	private Room GetRandomLastRoom()
	{
		foreach (var room in LevelMap.Map)
		{
			if (room.IsLast)
			{
				return room;
			}
		}

		return null;
	}

	private RoomPrefab GetRoomPrefab(Room room, List<RoomPrefab> list)
	{
		List<RoomPrefab> newList = new List<RoomPrefab>();

		bool IsLeftMatch = true;
		bool IsRightMatch = true;
		bool IsUpMatch = true;
		bool IsDownMatch = true;

		foreach (var roomPrefab in list)
		{
			if (room.RoomType == RoomType.Hallway)
			{
				IsLeftMatch = IsChunkFree(room.Position + Vector3Int.left) != roomPrefab.IsLeftOpen;
				IsRightMatch = IsChunkFree(room.Position + Vector3Int.right) != roomPrefab.IsRightOpen;
				IsUpMatch = IsChunkFree(room.Position + Vector3Int.up) != roomPrefab.IsUpOpen;
				IsDownMatch = IsChunkFree(room.Position + Vector3Int.down) != roomPrefab.IsDownOpen;
			}

			if (IsLeftMatch && IsRightMatch && IsUpMatch && IsDownMatch)
			{
				newList.Add(roomPrefab);
			}
		}

		if (newList.Count > 0)
		{
			return newList[Random.Range(0, newList.Count)];
		}

		return null;
	}

	private Room GetExtremeRoom(out Vector3Int directionToSpawn)
	{
		Room room = null;

		int extremeChunk = 0;

		int start = LevelMap.DownChunk;
		int finish = LevelMap.UpChunk;

		Vector3Int edgeAxis = Vector3Int.right;
		Vector3Int direction = Vector3Int.up;

		switch (Random.Range(0, 4))
		{
			case 0:
				extremeChunk = LevelMap.LeftChunk;
				directionToSpawn = Vector3Int.left;

				start = LevelMap.DownChunk;
				finish = LevelMap.UpChunk;

				edgeAxis = Vector3Int.right;
				direction = Vector3Int.up;
				break;
			case 1:
				extremeChunk = LevelMap.UpChunk;
				directionToSpawn = Vector3Int.up;

				start = LevelMap.LeftChunk;
				finish = LevelMap.RightChunk;

				edgeAxis = Vector3Int.up;
				direction = Vector3Int.right;
				break;
			case 2:
				extremeChunk = LevelMap.RightChunk;
				directionToSpawn = Vector3Int.right;

				start = LevelMap.DownChunk;
				finish = LevelMap.UpChunk;

				edgeAxis = Vector3Int.right;
				direction = Vector3Int.up;
				break;
			case 3:
				extremeChunk = LevelMap.DownChunk;
				directionToSpawn = Vector3Int.down;

				start = LevelMap.LeftChunk;
				finish = LevelMap.RightChunk;

				edgeAxis = Vector3Int.up;
				direction = Vector3Int.right;
				break;
			default:
				throw new UnityException("Invalid Random value");
		}

		for (int i = start; i <= finish; i++)
		{
			room = LevelMap.GetRoom(edgeAxis * extremeChunk + direction * i);

			if (room != null)
			{
				return room;
			}
		}

		throw new UnityException("Room not found");
	}

	private Tile GetFloorTile(int x, int y)
	{
		if (x % 2 == 0 && y % 2 == 0)
		{
			return _floorTiles[0];
		}
		else if (x % 2 == 1 && y % 2 == 0)
		{
			return _floorTiles[1];
		}
		else if (x % 2 == 0 && y % 2 == 1)
		{
			return _floorTiles[2];
		}
		else
		{
			return _floorTiles[3];
		}
	}

	private TileWithID GetTileByID(int[] id)
	{
		if (id.Length != 9)
		{
			Debug.LogError("Incorrect ID");
		}

		foreach (var tile in _wallTiles)
		{
			bool correctTile = true;

			for (int i = 0; i < 9; i++)
			{
				if (tile.id[i] != 2 && tile.id[i] != id[i])
				{
					correctTile = false;
				}
			}

			if (correctTile)
			{
				return tile;
			}
		}

		return _wallTiles[0];
	}

	private int[] GetTileID(Vector3Int tilePosition)
	{
		int[] id = new int[9];

		id[0] = BoolToInt(HasLeftUp(tilePosition));
		id[1] = BoolToInt(HasUp(tilePosition));
		id[2] = BoolToInt(HasRightUp(tilePosition));
		id[3] = BoolToInt(HasLeft(tilePosition));
		id[4] = BoolToInt(Has(tilePosition));
		id[5] = BoolToInt(HasRight(tilePosition));
		id[6] = BoolToInt(HasLeftDown(tilePosition));
		id[7] = BoolToInt(HasDown(tilePosition));
		id[8] = BoolToInt(HasRightDown(tilePosition));

		return id;
	}
	#endregion

	#region Creation
	private void CreateCollidWallTile(int spriteIndex, int[] tileID)
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		tile.sprite = _roomPalette.Sprites[spriteIndex];
		TileWithID tileWithID = new TileWithID(tile, tileID);

		_wallTiles.Add(tileWithID);
	}

	private void CreateCollidWallTile(int[] tileID)
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		TileWithID tileWithID = new TileWithID(tile, tileID);

		_wallTiles.Add(tileWithID);
	}

	private void CreateNonCollidWallTile(int spriteIndex)
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		tile.sprite = _roomPalette.Sprites[spriteIndex];

		_nonCollidWallTiles.Add(tile);
	}

	private void CreateFloorTile(int spriteIndex)
	{
		Tile tile = ScriptableObject.CreateInstance<Tile>();

		tile.sprite = _roomPalette.Sprites[spriteIndex];

		_floorTiles.Add(tile);
	}

	private void CreateTiles()
	{
		//Void tile
		CreateCollidWallTile(8, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			0, 0, 0
		});

		//Black wall
		CreateCollidWallTile(new int[]
		{
			1, 1, 1,
			1, 1, 1,
			1, 1, 1
		});

		CreateNonCollidWallTile(6);
		CreateNonCollidWallTile(7);
		CreateNonCollidWallTile(15);
		CreateNonCollidWallTile(16);

		CreateFloorTile(23);
		CreateFloorTile(24);
		CreateFloorTile(31);
		CreateFloorTile(32);

		CreateCollidWallTile(0, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			1, 1, 0
		});

		CreateCollidWallTile(1, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			0, 1, 1
		});

		CreateCollidWallTile(2, new int[]
		{
			2, 1, 2,
			1, 1, 0,
			2, 0, 2
		});

		CreateCollidWallTile(3, new int[]
		{
			2, 1, 2,
			0, 1, 1,
			2, 0, 2
		});

		CreateCollidWallTile(4, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			2, 0, 2
		});

		CreateCollidWallTile(5, new int[]
		{
			2, 1, 1,
			0, 1, 1,
			2, 1, 1
		});

		CreateCollidWallTile(9, new int[]
		{
			1, 1, 0,
			1, 1, 1,
			1, 1, 1
		});

		CreateCollidWallTile(10, new int[]
		{
			0, 1, 1,
			1, 1, 1,
			1, 1, 1
		});

		CreateCollidWallTile(11, new int[]
		{
			2, 0, 2,
			1, 1, 0,
			2, 1, 2
		});

		CreateCollidWallTile(12, new int[]
		{
			2, 0, 2,
			0, 1, 1,
			2, 1, 2
		});

		CreateCollidWallTile(13, new int[]
		{
			2, 0, 2,
			1, 1, 1,
			1, 1, 1
		});

		CreateCollidWallTile(14, new int[]
		{
			1, 1, 2,
			1, 1, 0,
			1, 1, 2
		});

		CreateCollidWallTile(17, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			0, 0, 1
		});

		CreateCollidWallTile(18, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			1, 0, 0
		});

		CreateCollidWallTile(19, new int[]
		{
			0, 0, 2,
			0, 0, 1,
			2, 1, 1
		});

		CreateCollidWallTile(20, new int[]
		{
			2, 0, 0,
			1, 0, 0,
			1, 1, 2
		});

		CreateCollidWallTile(21, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			2, 1, 2
		});

		CreateCollidWallTile(22, new int[]
		{
			2, 0, 0,
			1, 0, 0,
			2, 0, 0
		});

		CreateCollidWallTile(25, new int[]
		{
			0, 0, 1,
			0, 0, 0,
			0, 0, 0
		});

		CreateCollidWallTile(26, new int[]
		{
			1, 0, 0,
			0, 0, 0,
			0, 0, 0
		});

		CreateCollidWallTile(27, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			0, 0, 2
		});

		CreateCollidWallTile(28, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			2, 0, 0
		});

		CreateCollidWallTile(29, new int[]
		{
			2, 1, 2,
			0, 0, 0,
			0, 0, 0
		});

		CreateCollidWallTile(30, new int[]
		{
			0, 0, 2,
			0, 0, 1,
			0, 0, 2
		});

		CreateCollidWallTile(33, new int[]
		{
			0, 0, 0,
			0, 1, 0,
			2, 1, 2
		});

		CreateCollidWallTile(34, new int[]
		{
			2, 1, 2,
			0, 1, 0,
			0, 0, 0
		});

		CreateCollidWallTile(35, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			0, 0, 2
		});

		CreateCollidWallTile(36, new int[]
		{
			2, 0, 0,
			1, 1, 0,
			2, 0, 0
		});

		CreateCollidWallTile(37, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			2, 1, 0
		});

		CreateCollidWallTile(38, new int[]
		{
			2, 0, 0,
			1, 1, 0,
			0, 1, 2
		});

		CreateCollidWallTile(39, new int[]
		{
			2, 1, 0,
			0, 1, 1,
			0, 0, 2
		});

		CreateCollidWallTile(40, new int[]
		{
			0, 1, 2,
			1, 1, 0,
			2, 0, 0
		});

		CreateCollidWallTile(41, new int[]
		{
			2, 0, 2,
			0, 1, 0,
			2, 0, 2
		});

		CreateCollidWallTile(42, new int[]
		{
			2, 0, 2,
			1, 1, 1,
			2, 0, 2
		});

		CreateCollidWallTile(43, new int[]
		{
			2, 1, 2,
			0, 1, 0,
			2, 1, 2
		});

		CreateCollidWallTile(44, new int[]
		{
			1, 1, 1,
			1, 0, 1,
			2, 0, 2
		});

		CreateCollidWallTile(45, new int[]
		{
			2, 0, 2,
			1, 0, 1,
			1, 1, 1
		});

		CreateCollidWallTile(46, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			1, 1, 2
		});

		CreateCollidWallTile(47, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			2, 1, 1
		});

		CreateCollidWallTile(48, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			2, 0, 1
		});

		CreateCollidWallTile(49, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			1, 0, 2
		});

		CreateCollidWallTile(50, new int[]
		{
			2, 0, 1,
			1, 0, 0,
			1, 1, 2
		});

		CreateCollidWallTile(51, new int[]
		{
			1, 0, 2,
			0, 0, 1,
			2, 1, 1
		});

		CreateCollidWallTile(52, new int[]
		{
			2, 1, 2,
			1, 0, 1,
			2, 1, 2
		});

		CreateCollidWallTile(53, new int[]
		{
			2, 1, 2,
			0, 0, 0,
			2, 1, 2
		});

		CreateCollidWallTile(54, new int[]
		{
			2, 0, 2,
			1, 0, 1,
			2, 0, 2
		});
	}
	#endregion

	#region HasFunctions
	private bool Has(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition) ||
			_WallTilemap.HasTile(tilePosition);
	}

	private bool HasRight(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right);
	}

	private bool HasRightDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.down);
	}

	private bool HasDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.down);
	}

	private bool HasLeftDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.down);
	}

	private bool HasLeft(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left);
	}

	private bool HasLeftUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.up);
	}

	private bool HasUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.up);
	}

	private bool HasRightUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.up);
	}
	#endregion

	private bool CanSpawnBigRoom(Vector3Int position, out Vector3Int lastPosition)
	{
		lastPosition = Vector3Int.zero;

		bool isUpFree = true;
		bool isDownFree = true;
		for (int i = 0; i < _bigRoomSize.y; i++)
		{
			Vector3Int newPositionUp = position + Vector3Int.up * i;
			Vector3Int newPositionDown = position + Vector3Int.down * i;
			if (IsAreaFree(newPositionUp) == false)
			{
				isUpFree = false;
			}
			if (IsAreaFree(newPositionDown) == false)
			{
				isDownFree = false;
			}
		}

		bool isLeftFree = true;
		bool isRightFree = true;
		for (int i = 0; i < _bigRoomSize.x; i++)
		{
			Vector3Int newPositionLeft = position + Vector3Int.left * i;
			Vector3Int newPositionRight = position + Vector3Int.right * i;
			if (IsAreaFree(newPositionLeft) == false)
			{
				isLeftFree = false;
			}
			if (IsAreaFree(newPositionRight) == false)
			{
				isRightFree = false;
			}
		}

		if (isUpFree)
		{
			lastPosition.y = position.y + _bigRoomSize.y - 1;
		}
		else if (isDownFree)
		{
			lastPosition.y = position.y - (_bigRoomSize.y - 1);
		}
		else
		{
			return false;
		}

		if (isRightFree)
		{
			lastPosition.x = position.x + _bigRoomSize.x - 1;
		}
		else if (isLeftFree)
		{
			lastPosition.x = position.x - (_bigRoomSize.x - 1);
		}
		else
		{
			return false;
		}

		return true;
	}

	private int BoolToInt(bool flag)
	{
		if (flag)
		{
			return 1;
		}

		return 0;
	}

	private int CountRoomsInArea(Vector3Int position)
	{
		int num = 0;
		foreach (var room in LevelMap.Map)
		{
			if (room.RoomType != RoomType.Hallway)
			{
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						if (room.Position == position + new Vector3Int(x, y, 0))
						{
							num++;
						}
					}
				}
			}
			else if (room.Position == position)
			{
				num++;
			}
		}

		return num;
	}

	private bool IsDirectionCorrect(Vector3Int direction)
	{
		Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.zero };

		return directions.Contains(direction);
	}

	private bool IsAreaFree(Vector3Int position)
	{
		foreach (var room in LevelMap.Map)
		{
			if (room.RoomType != RoomType.Hallway)
			{
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						if (room.Position == position + new Vector3Int(x, y, 0))
						{
							return false;
						}
					}
				}
			}
			if (room.Position == position)
			{
				return false;
			}
		}

		return true;
	}

	private bool IsChunkFree(Vector3Int position)
	{
		foreach (var room in LevelMap.Map)
		{
			if (room.Position == position)
			{
				return false;
			}
		}

		return true;
	}
}