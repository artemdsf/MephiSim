using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	/// <summary>
	/// Palette of tiles
	/// </summary>
	[SerializeField] private RoomPalette _roomPalette;
	/// <summary>
	/// Container of room prefabs
	/// </summary>
	[SerializeField] private RoomPrefabContainer _roomPrefabContainer;

	/// <summary>
	/// Floor tilemap 
	/// </summary>
	[SerializeField] private Tilemap _FloorTilemap;
	/// <summary>
	/// Wall tilemap
	/// </summary>
	[SerializeField] private Tilemap _WallTilemap;
	/// <summary>
	/// Collidable wall tilemap
	/// </summary>
	[SerializeField] private Tilemap _CollidWallTilemap;
	
	private Tile _tile;

	/// <summary>
	/// List of wall tiles
	/// </summary>
	private List<TileWithID> _wallTiles = new List<TileWithID>();
	/// <summary>
	/// List of non collidable wall tiles
	/// </summary>
	private List<Tile> _nonCollidWallTiles = new List<Tile>();
	/// <summary>
	/// List of floor tiles
	/// </summary>
	private List<Tile> _floorTiles = new List<Tile>();

	#region Generation
	/// <summary>
	/// Add start room to map
	/// </summary>
	/// <returns>Spawned room</returns>
	protected Room AddStartRoom()
	{
		return new Room(LevelMap.StartPosition, RoomType.StartRoom, 0);
	}

	/// <summary>
	/// Add new room next to room on the map
	/// </summary>
	/// <param name="lastRoom">Existing room</param>
	/// <param name="direction">Direction to spawn</param>
	/// <param name="roomType">Type of new room</param>
	/// <param name="distance">The distance of the new room from the start</param>
	/// <returns>Spawned room</returns>
	protected Room AddNextRoom(Room lastRoom, Vector2Int direction, RoomType roomType, int distance)
	{
		if (IsDirectionCorrect(direction) == false)
		{
			throw new UnityException("Invalid direction");
		}
		if (roomType == RoomType.StartRoom)
		{
			throw new UnityException("Can't spawn second start room");
		}

		Room room = new Room(lastRoom.Position + direction, roomType, distance);

		if (direction == Vector2Int.left)
		{
			lastRoom.IsLeftOpen = true;
			room.IsRightOpen = true;
		}
		else if (direction == Vector2Int.right)
		{
			lastRoom.IsRightOpen = true;
			room.IsLeftOpen = true;
		}
		else if (direction == Vector2Int.up)
		{
			lastRoom.IsUpOpen = true;
			room.IsDownOpen = true;
		}
		else if (direction == Vector2Int.down)
		{
			lastRoom.IsDownOpen = true;
			room.IsUpOpen = true;
		}

		return room;
	}

	/// <summary>
	/// Generate all tiles to tilemaps
	/// </summary>
	protected void GenerateFloor()
	{
		CreateTiles();

		foreach (var room in LevelMap.Map)
		{
			GenerateRoom(room);
		}

		SetSkirtingBoards();
		SetWallTiles();
		SetCollidWallTiles();
	}

	/// <summary>
	/// Generate one room to tilemap
	/// </summary>
	/// <param name="room">Room to spawn</param>
	private void GenerateRoom(Room room)
	{
		switch (room.RoomType)
		{
			case RoomType.StartRoom:
				CopyTilemap(room, _roomPrefabContainer.StartRooms[Random.Range(0, _roomPrefabContainer.StartRooms.Count)]);
				break;
			case RoomType.EnemyRoom:
				CopyTilemap(room, _roomPrefabContainer.EnemyRooms[Random.Range(0, _roomPrefabContainer.EnemyRooms.Count)]);
				break;
			case RoomType.SpecialRoom:
				CopyTilemap(room, _roomPrefabContainer.SpecialRooms[Random.Range(0, _roomPrefabContainer.SpecialRooms.Count)]);
				break;
			case RoomType.DoubeRoom:
				CopyTilemap(room, _roomPrefabContainer.DoubeRooms[Random.Range(0, _roomPrefabContainer.DoubeRooms.Count)]);
				break;
			case RoomType.Hallway:
				CopyTilemap(room, _roomPrefabContainer.Hallways[Random.Range(0, _roomPrefabContainer.Hallways.Count)]);
				break;
		}
	}

	/// <summary>
	/// Copy room from prefab to base tilemap
	/// </summary>
	/// <param name="room">Room</param>
	/// <param name="prefab">Room prefab</param>
	private void CopyTilemap(Room room, RoomPrefab prefab)
	{
		Vector3Int tilePosition;

		for (int i = 0; i < LevelMap.ChunkSize.x; i++)
		{
			for (int j = 0; j < LevelMap.ChunkSize.y; j++)
			{
				if (prefab.Tilemap.HasTile(new Vector3Int(i, j, 0)))
				{
					tilePosition = prefab.LeftDownCorner + new Vector3Int(i, j, 0);
					_FloorTilemap.SetTile(tilePosition, GetFloorTile(i, j));
				}
			}
		}
	}

	/// <summary>
	/// Get floor tile by position 
	/// </summary>
	/// <param name="x">X</param>
	/// <param name="y">Y</param>
	/// <returns>Floor tile</returns>
	private Tile GetFloorTile(int x, int y)
	{
		if (x % 2 == 0 && y % 2 == 0)
		{
			return _floorTiles[0];
		}
		else if(x % 2 == 1 && y % 2 == 0)
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

	/// <summary>
	/// Generate skirting boards
	/// </summary>
	private void SetSkirtingBoards()
	{
		int[] id = new int[9];

		for (int i = 0; i <= LevelMap.RightExtremeTilePosition; i++)
		{
			for (int j = 0; j <= LevelMap.UpExtremeTilePosition; j++)
			{
				id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 1)
				{
					_FloorTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
	}

	/// <summary>
	/// Generate back wall tiles
	/// </summary>
	private void SetWallTiles()
	{
		for (int i = LevelMap.RightExtremeTilePosition; i >= 0; i--)
		{
			for (int j = LevelMap.UpExtremeTilePosition; j >= 0; j--)
			{
				if (Has(new Vector3Int(i, j, 0)) && HasUp(new Vector3Int(i, j, 0)) == false)
				{
					if (i % 2 == 0)
					{
						_WallTilemap.SetTile(new Vector3Int(i, j + 2, 0), _nonCollidWallTiles[0]);
						_WallTilemap.SetTile(new Vector3Int(i, j + 1, 0), _nonCollidWallTiles[1]);
					}
					else
					{
						_WallTilemap.SetTile(new Vector3Int(i, j + 2, 0), _nonCollidWallTiles[2]);
						_WallTilemap.SetTile(new Vector3Int(i, j + 1, 0), _nonCollidWallTiles[3]);
					}
				}
			}
		}
	}

	/// <summary>
	/// Generate collidable wall tiles
	/// </summary>
	private void SetCollidWallTiles()
	{
		int[] id = new int[9];

		for (int i = 0; i <= LevelMap.RightExtremeTilePosition; i++)
		{
			for (int j = 0; j <= LevelMap.UpExtremeTilePosition; j++)
			{
				id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 0)
				{
					_CollidWallTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
	}

	/// <summary>
	/// Get tile ID
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>ID</returns>
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

	/// <summary>
	/// Get tile by ID
	/// </summary>
	/// <param name="id">Tile ID</param>
	/// <returns>Wall tile</returns>
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
	#endregion

	#region Creation
	/// <summary>
	/// Create collidable wall tile
	/// </summary>
	/// <param name="spriteIndex">Sprite index</param>
	/// <param name="tileID">Tile ID</param>
	private void CreateCollidWallTile(int spriteIndex, int[] tileID)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = _roomPalette.Sprites[spriteIndex];
		TileWithID tile = new TileWithID(_tile, tileID);

		_wallTiles.Add(tile);
	}

	/// <summary>
	/// Create wall tile
	/// </summary>
	/// <param name="spriteIndex">Sprite index</param>
	private void CreateNonCollidWallTile(int spriteIndex)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = _roomPalette.Sprites[spriteIndex];

		_nonCollidWallTiles.Add(_tile);
	}

	/// <summary>
	/// Create floor tile
	/// </summary>
	/// <param name="spriteIndex">Sprite index</param>
	private void CreateFloorTile(int spriteIndex)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = _roomPalette.Sprites[spriteIndex];

		_floorTiles.Add(_tile);
	}

	/// <summary>
	/// Create tile with ID
	/// </summary>
	private void CreateTiles()
	{
		//Void tile
		CreateCollidWallTile(8, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			0, 0, 0
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
			1, 1, 2,
			1, 1, 0,
			2, 0, 0
		});

		CreateCollidWallTile(3, new int[]
		{
			2, 1, 1,
			0, 1, 1,
			0, 0, 2
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
			2, 0, 0,
			1, 1, 0,
			1, 1, 2
		});

		CreateCollidWallTile(12, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			2, 1, 1
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
	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool Has(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition) ||
			_WallTilemap.HasTile(tilePosition);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasRight(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasRightDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.down);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.down);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasLeftDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.down) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.down);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasLeft(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasLeftUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.up);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.up);
	}

	/// <summary>
	/// Check for tile availability
	/// </summary>
	/// <param name="tilePosition">Tile position</param>
	/// <returns>Tile availability</returns>
	private bool HasRightUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.up) ||
			_WallTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.up);
	}
	#endregion

	/// <summary>
	/// Convert bool to int
	/// </summary>
	/// <param name="flag">Value</param>
	/// <returns>Int value</returns>
	private int BoolToInt(bool flag)
	{
		if (flag)
		{
			return 1;
		}
		return 0;
	}

	/// <summary>
	/// Check direction of correctness
	/// </summary>
	/// <param name="direction">Direction</param>
	/// <returns>Is correct</returns>
	private bool IsDirectionCorrect(Vector2Int direction)
	{
		if (direction == Vector2Int.left)
			return true;
		if (direction == Vector2Int.right)
			return true;
		if (direction == Vector2Int.up)
			return true;
		if (direction == Vector2Int.down)
			return true;
		return false;
	}
}