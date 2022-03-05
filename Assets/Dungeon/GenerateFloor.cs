using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum RoomType
{
	Room,
	hallwayHoriz,
	hallwayVertic
}

public class WallTile
{
	public WallTile(Tile tile, int[] arr)
	{
		this.tile = tile;
		id = arr;
	}

	public Tile tile;
	public int[] id = new int[9];
}

public class GenerateFloor : MonoBehaviour
{
	//1 - Generate floor
	//2 - Generate skirting board
	//3 - Generate wall in floor tilemap
	//4 - Generate collidable wall
	//Chunk start tile left down
	private readonly Vector3Int _chunkSize = new Vector3Int(18, 10, 0);

	[SerializeField] private List<Sprite> _sprites = new List<Sprite>();

	[SerializeField] private Tilemap _FloorTilemap;
	[SerializeField] private Tilemap _CollidWallTilemap;

	#region Tiles
	private Tile _baseFloorTile1;
	private Tile _baseFloorTile2;
	private Tile _baseFloorTile3;
	private Tile _baseFloorTile4;

	private Tile _floorPointRightDownTile;
	private Tile _floorPointLeftDownTile;
	private Tile _floorPointRightUpTile;
	private Tile _floorPointLeftUpTile;

	private Tile _floorAngleRightDownTile;
	private Tile _floorAngleLeftDownTile;
	private Tile _floorAngleRightUpTile;
	private Tile _floorAngleLeftUpTile;

	private Tile _floorStraightDownTile;
	private Tile _floorStraightLeftTile;
	private Tile _floorStraightUpTile;
	private Tile _floorStraightRightTile;

	private Tile _floorSquareDownTile;
	private Tile _floorSquareUpTile;
	private Tile _floorSquareRightTile;
	private Tile _floorSquareLeftTile;

	private Tile _floorAngleWithPointTile1;
	private Tile _floorAngleWithPointTile2;
	private Tile _floorAngleWithPointTile3;
	private Tile _floorAngleWithPointTile4;

	private Tile _floorSquareTile;
	private Tile _floorSquareHorizontalTile;
	private Tile _floorSquareVerticalTile;

	private Tile _baseWallTile1;
	private Tile _baseWallTile2;
	private Tile _baseWallTile3;
	private Tile _baseWallTile4;

	private Tile _upWallPointRightDownTile;
	private Tile _upWallPointLeftDownTile;
	private Tile _upWallPointRightUpTile;
	private Tile _upWallPointLeftUpTile;

	private Tile _upWallAngleRightDownTile;
	private Tile _upWallAngleLeftDownTile;
	private Tile _upWallAngleRightUpTile;
	private Tile _upWallAngleLeftUpTile;

	private Tile _upWallStraightDownTile;
	private Tile _upWallStraightLeftTile;
	private Tile _upWallStraightUpTile;
	private Tile _upWallStraightRightTile;

	private Tile _upWallSquareDownTile;
	private Tile _upWallSquareUpTile;
	private Tile _upWallSquareRightTile;
	private Tile _upWallSquareLeftTile;

	private Tile _upWallAngleWithPointTile1;
	private Tile _upWallAngleWithPointTile2;
	private Tile _upWallAngleWithPointTile3;
	private Tile _upWallAngleWithPointTile4;

	private Tile _upWallSquareTile;
	private Tile _upWallSquareHorizontalTile;
	private Tile _upWallSquareVerticalTile;

	private Tile _voidTile;
	private Tile _emptyTile;
	#endregion

	List<WallTile> wallTiles = new List<WallTile>();

	private Vector2Int _leftTopChunk = new Vector2Int(0, 0);
	private Vector2Int _rightDownChunk = new Vector2Int(0, 0);

	private void Start()
	{
		CreateTiles();

		GenerateMap();
		SetSkirtingBoards();
		SetWallTiles();
		SetCollidWallTiles();
	}

	private void CreateTiles()
	{
		_voidTile = ScriptableObject.CreateInstance<Tile>();
		_voidTile.sprite = _sprites[8];
		WallTile voidTile = new WallTile(_voidTile, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			0, 0, 0
		});
		wallTiles.Add(voidTile);

		_emptyTile = ScriptableObject.CreateInstance<Tile>();
		WallTile emptyTile = new WallTile(_emptyTile, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			1, 1, 1
		});
		wallTiles.Add(emptyTile);

		_baseFloorTile1 = ScriptableObject.CreateInstance<Tile>();
		_baseFloorTile1.sprite = _sprites[23];
		_baseFloorTile2 = ScriptableObject.CreateInstance<Tile>();
		_baseFloorTile2.sprite = _sprites[24];
		_baseFloorTile3 = ScriptableObject.CreateInstance<Tile>();
		_baseFloorTile3.sprite = _sprites[31];
		_baseFloorTile4 = ScriptableObject.CreateInstance<Tile>();
		_baseFloorTile4.sprite = _sprites[32];

		_floorPointRightDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorPointRightDownTile.sprite = _sprites[0];
		WallTile floorPointRightDownTile = new WallTile(_floorPointRightDownTile, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			1, 1, 0
		});
		wallTiles.Add(floorPointRightDownTile);

		_floorPointLeftDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorPointLeftDownTile.sprite = _sprites[1];
		WallTile floorPointLeftDownTile = new WallTile(_floorPointLeftDownTile, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			0, 1, 1
		});
		wallTiles.Add(floorPointLeftDownTile);

		_floorPointRightUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorPointRightUpTile.sprite = _sprites[9];
		WallTile floorPointRightUpTile = new WallTile(_floorPointRightUpTile, new int[]
		{
			1, 1, 0,
			1, 1, 1,
			1, 1, 1
		});
		wallTiles.Add(floorPointRightUpTile);

		_floorPointLeftUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorPointLeftUpTile.sprite = _sprites[10];
		WallTile floorPointLeftUpTile = new WallTile(_floorPointLeftUpTile, new int[]
		{
			0, 1, 1,
			1, 1, 1,
			1, 1, 1
		});
		wallTiles.Add(floorPointLeftUpTile);

		_floorAngleRightDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorAngleRightDownTile.sprite = _sprites[2];
		WallTile floorAngleRightDownTile = new WallTile(_floorAngleRightDownTile, new int[]
		{
			1, 1, 2,
			1, 1, 0,
			2, 0, 0
		});
		wallTiles.Add(floorAngleRightDownTile);

		_floorAngleLeftDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorAngleLeftDownTile.sprite = _sprites[3];
		WallTile floorAngleLeftDownTile = new WallTile(_floorAngleLeftDownTile, new int[]
		{
			2, 1, 1,
			0, 1, 1,
			0, 0, 2
		});
		wallTiles.Add(floorAngleLeftDownTile);

		_floorAngleRightUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorAngleRightUpTile.sprite = _sprites[11];
		WallTile floorAngleRightUpTile = new WallTile(_floorAngleRightUpTile, new int[]
		{
			2, 0, 0,
			1, 1, 0,
			1, 1, 2
		});
		wallTiles.Add(floorAngleRightUpTile);

		_floorAngleLeftUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorAngleLeftUpTile.sprite = _sprites[12];
		WallTile floorAngleLeftUpTile = new WallTile(_floorAngleLeftUpTile, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			2, 1, 1
		});
		wallTiles.Add(floorAngleLeftUpTile);

		_floorStraightDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorStraightDownTile.sprite = _sprites[4];
		WallTile floorStraightDownTile = new WallTile(_floorStraightDownTile, new int[]
		{
			1, 1, 1,
			1, 1, 1,
			2, 0, 2
		});
		wallTiles.Add(floorStraightDownTile);

		_floorStraightLeftTile = ScriptableObject.CreateInstance<Tile>();
		_floorStraightLeftTile.sprite = _sprites[5];
		WallTile floorStraightLeftTile = new WallTile(_floorStraightLeftTile, new int[]
		{
			2, 1, 1,
			0, 1, 1,
			2, 1, 1
		});
		wallTiles.Add(floorStraightLeftTile);

		_floorStraightUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorStraightUpTile.sprite = _sprites[13];
		WallTile floorStraightUpTile = new WallTile(_floorStraightUpTile, new int[]
		{
			2, 0, 2,
			1, 1, 1,
			1, 1, 1
		});
		wallTiles.Add(floorStraightUpTile);

		_floorStraightRightTile = ScriptableObject.CreateInstance<Tile>();
		_floorStraightRightTile.sprite = _sprites[14];
		WallTile floorStraightRightTile = new WallTile(_floorStraightRightTile, new int[]
		{
			1, 1, 2,
			1, 1, 0,
			1, 1, 2
		});
		wallTiles.Add(floorStraightRightTile);

		_baseWallTile1 = ScriptableObject.CreateInstance<Tile>();
		_baseWallTile1.sprite = _sprites[6];
		_baseWallTile2 = ScriptableObject.CreateInstance<Tile>();
		_baseWallTile2.sprite = _sprites[7];
		_baseWallTile3 = ScriptableObject.CreateInstance<Tile>();
		_baseWallTile3.sprite = _sprites[15];
		_baseWallTile4 = ScriptableObject.CreateInstance<Tile>();
		_baseWallTile4.sprite = _sprites[16];

		_upWallPointRightDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallPointRightDownTile.sprite = _sprites[17];
		WallTile upWallPointRightDownTile = new WallTile(_upWallPointRightDownTile, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			0, 0, 1
		});
		wallTiles.Add(upWallPointRightDownTile);

		_upWallPointLeftDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallPointLeftDownTile.sprite = _sprites[18];
		WallTile upWallPointLeftDownTile = new WallTile(_upWallPointLeftDownTile, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			1, 0, 0
		});
		wallTiles.Add(upWallPointLeftDownTile);

		_upWallPointRightUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallPointRightUpTile.sprite = _sprites[25];
		WallTile upWallPointRightUpTile = new WallTile(_upWallPointRightUpTile, new int[]
		{
			0, 0, 1,
			0, 0, 0,
			0, 0, 0
		});
		wallTiles.Add(upWallPointRightUpTile);

		_upWallPointLeftUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallPointLeftUpTile.sprite = _sprites[26];
		WallTile upWallPointLeftUpTile = new WallTile(_upWallPointLeftUpTile, new int[]
		{
			1, 0, 0,
			0, 0, 0,
			0, 0, 0
		});
		wallTiles.Add(upWallPointLeftUpTile);

		_upWallAngleRightDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleRightDownTile.sprite = _sprites[19];
		WallTile upWallAngleRightDownTile = new WallTile(_upWallAngleRightDownTile, new int[]
		{
			0, 0, 2,
			0, 0, 1,
			2, 1, 1
		});
		wallTiles.Add(upWallAngleRightDownTile);

		_upWallAngleLeftDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleLeftDownTile.sprite = _sprites[20];
		WallTile upWallAngleLeftDownTile = new WallTile(_upWallAngleLeftDownTile, new int[]
		{
			2, 0, 0,
			1, 0, 0,
			1, 1, 2
		});
		wallTiles.Add(upWallAngleLeftDownTile);

		_upWallAngleRightUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleRightUpTile.sprite = _sprites[27];
		WallTile upWallAngleRightUpTile = new WallTile(_upWallAngleRightUpTile, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			0, 0, 2
		});
		wallTiles.Add(upWallAngleRightUpTile);

		_upWallAngleLeftUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleLeftUpTile.sprite = _sprites[28];
		WallTile upWallAngleLeftUpTile = new WallTile(_upWallAngleLeftUpTile, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			2, 0, 0
		});
		wallTiles.Add(upWallAngleLeftUpTile);

		_upWallStraightDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallStraightDownTile.sprite = _sprites[21];
		WallTile upWallStraightDownTile = new WallTile(_upWallStraightDownTile, new int[]
		{
			0, 0, 0,
			0, 0, 0,
			2, 1, 2
		});
		wallTiles.Add(upWallStraightDownTile);

		_upWallStraightLeftTile = ScriptableObject.CreateInstance<Tile>();
		_upWallStraightLeftTile.sprite = _sprites[22];
		WallTile upWallStraightLeftTile = new WallTile(_upWallStraightLeftTile, new int[]
		{
			2, 0, 0,
			1, 0, 0,
			2, 0, 0
		});
		wallTiles.Add(upWallStraightLeftTile);

		_upWallStraightUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallStraightUpTile.sprite = _sprites[29];
		WallTile upWallStraightUpTile = new WallTile(_upWallStraightUpTile, new int[]
		{
			2, 1, 2,
			0, 0, 0,
			0, 0, 0
		});
		wallTiles.Add(upWallStraightUpTile);

		_upWallStraightRightTile = ScriptableObject.CreateInstance<Tile>();
		_upWallStraightRightTile.sprite = _sprites[30];
		WallTile upWallStraightRightTile = new WallTile(_upWallStraightRightTile, new int[]
		{
			0, 0, 2,
			0, 0, 1,
			0, 0, 2
		});
		wallTiles.Add(upWallStraightRightTile);

		_floorSquareDownTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareDownTile.sprite = _sprites[33];
		WallTile floorSquareDownTile = new WallTile(_floorSquareDownTile, new int[]
		{
			0, 0, 0,
			0, 1, 0,
			2, 1, 2
		});
		wallTiles.Add(floorSquareDownTile);

		_floorSquareUpTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareUpTile.sprite = _sprites[34];
		WallTile floorSquareUpTile = new WallTile(_floorSquareUpTile, new int[]
		{
			2, 1, 2,
			0, 1, 0,
			0, 0, 0
		});
		wallTiles.Add(floorSquareUpTile);

		_floorSquareRightTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareRightTile.sprite = _sprites[35];
		WallTile floorSquareRightTile = new WallTile(_floorSquareRightTile, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			0, 0, 2
		});
		wallTiles.Add(floorSquareRightTile);

		_floorSquareLeftTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareLeftTile.sprite = _sprites[36];
		WallTile floorSquareLeftTile = new WallTile(_floorSquareLeftTile, new int[]
		{
			2, 0, 0,
			1, 1, 0,
			2, 0, 0
		});
		wallTiles.Add(floorSquareLeftTile);

		_floorAngleWithPointTile1 = ScriptableObject.CreateInstance<Tile>();
		_floorAngleWithPointTile1.sprite = _sprites[37];
		WallTile floorAngleWithPointTile1 = new WallTile(_floorAngleWithPointTile1, new int[]
		{
			0, 0, 2,
			0, 1, 1,
			2, 1, 0
		});
		wallTiles.Add(floorAngleWithPointTile1);

		_floorAngleWithPointTile2 = ScriptableObject.CreateInstance<Tile>();
		_floorAngleWithPointTile2.sprite = _sprites[38];
		WallTile floorAngleWithPointTile2 = new WallTile(_floorAngleWithPointTile2, new int[]
		{
			2, 0, 0,
			1, 1, 0,
			0, 1, 2
		});
		wallTiles.Add(floorAngleWithPointTile2);

		_floorAngleWithPointTile3 = ScriptableObject.CreateInstance<Tile>();
		_floorAngleWithPointTile3.sprite = _sprites[39];
		WallTile floorAngleWithPointTile3 = new WallTile(_floorAngleWithPointTile3, new int[]
		{
			2, 1, 0,
			0, 1, 1,
			0, 0, 2
		});
		wallTiles.Add(floorAngleWithPointTile3);

		_floorAngleWithPointTile4 = ScriptableObject.CreateInstance<Tile>();
		_floorAngleWithPointTile4.sprite = _sprites[40];
		WallTile floorAngleWithPointTile4 = new WallTile(_floorAngleWithPointTile4, new int[]
		{
			0, 1, 2,
			1, 1, 0,
			2, 0, 0
		});
		wallTiles.Add(floorAngleWithPointTile4);

		_floorSquareTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareTile.sprite = _sprites[41];
		WallTile floorSquareTile = new WallTile(_floorSquareTile, new int[]
		{
			2, 0, 2,
			0, 1, 0,
			2, 0, 2
		});
		wallTiles.Add(floorSquareTile);

		_floorSquareHorizontalTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareHorizontalTile.sprite = _sprites[42];
		WallTile floorSquareHorizontalTile = new WallTile(_floorSquareHorizontalTile, new int[]
		{
			2, 0, 2,
			1, 1, 1,
			2, 0, 2
		});
		wallTiles.Add(floorSquareHorizontalTile);

		_floorSquareVerticalTile = ScriptableObject.CreateInstance<Tile>();
		_floorSquareVerticalTile.sprite = _sprites[43];
		WallTile floorSquareVerticalTile = new WallTile(_floorSquareVerticalTile, new int[]
		{
			2, 1, 2,
			0, 1, 0,
			2, 1, 2
		});
		wallTiles.Add(floorSquareVerticalTile);

		_upWallSquareDownTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareDownTile.sprite = _sprites[44];
		WallTile upWallSquareDownTile = new WallTile(_upWallSquareDownTile, new int[]
		{
			1, 1, 1,
			1, 0, 1,
			2, 0, 2
		});
		wallTiles.Add(upWallSquareDownTile);

		_upWallSquareUpTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareUpTile.sprite = _sprites[45];
		WallTile upWallSquareUpTile = new WallTile(_upWallSquareUpTile, new int[]
		{
			2, 0, 2,
			1, 0, 1,
			1, 1, 1
		});
		wallTiles.Add(upWallSquareUpTile);

		_upWallSquareRightTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareRightTile.sprite = _sprites[46];
		WallTile upWallSquareRightTile = new WallTile(_upWallSquareRightTile, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			1, 1, 2
		});
		wallTiles.Add(upWallSquareRightTile);

		_upWallSquareLeftTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareLeftTile.sprite = _sprites[47];
		WallTile upWallSquareLeftTile = new WallTile(_upWallSquareLeftTile, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			2, 1, 1
		});
		wallTiles.Add(upWallSquareLeftTile);

		_upWallAngleWithPointTile1 = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleWithPointTile1.sprite = _sprites[48];
		WallTile upWallAngleWithPointTile1 = new WallTile(_upWallAngleWithPointTile1, new int[]
		{
			1, 1, 2,
			1, 0, 0,
			2, 0, 1
		});
		wallTiles.Add(upWallAngleWithPointTile1);

		_upWallAngleWithPointTile2 = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleWithPointTile2.sprite = _sprites[49];
		WallTile upWallAngleWithPointTile2 = new WallTile(_upWallAngleWithPointTile2, new int[]
		{
			2, 1, 1,
			0, 0, 1,
			1, 0, 2
		});
		wallTiles.Add(upWallAngleWithPointTile2);

		_upWallAngleWithPointTile3 = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleWithPointTile3.sprite = _sprites[50];
		WallTile upWallAngleWithPointTile3 = new WallTile(_upWallAngleWithPointTile3, new int[]
		{
			2, 0, 1,
			1, 0, 0,
			1, 1, 2
		});
		wallTiles.Add(upWallAngleWithPointTile3);

		_upWallAngleWithPointTile4 = ScriptableObject.CreateInstance<Tile>();
		_upWallAngleWithPointTile4.sprite = _sprites[51];
		WallTile upWallAngleWithPointTile4 = new WallTile(_upWallAngleWithPointTile4, new int[]
		{
			1, 0, 2,
			0, 0, 1,
			2, 1, 1
		});
		wallTiles.Add(upWallAngleWithPointTile4);

		_upWallSquareTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareTile.sprite = _sprites[52];
		WallTile upWallSquareTile = new WallTile(_upWallSquareTile, new int[]
		{
			2, 1, 2,
			1, 0, 1,
			2, 1, 2
		});
		wallTiles.Add(upWallSquareTile);

		_upWallSquareHorizontalTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareHorizontalTile.sprite = _sprites[53];
		WallTile upWallSquareHorizontalTile = new WallTile(_upWallSquareHorizontalTile, new int[]
		{
			2, 1, 2,
			0, 0, 0,
			2, 1, 2
		});
		wallTiles.Add(upWallSquareHorizontalTile);

		_upWallSquareVerticalTile = ScriptableObject.CreateInstance<Tile>();
		_upWallSquareVerticalTile.sprite = _sprites[54];
		WallTile upWallSquareVerticalTile = new WallTile(_upWallSquareVerticalTile, new int[]
		{
			2, 0, 2,
			1, 0, 1,
			2, 0, 2
		});
		wallTiles.Add(upWallSquareVerticalTile);
	}

	private void SetSkirtingBoards()
	{
		int[] id = new int[9];

		for (int i = _leftTopChunk.x * _chunkSize.x; i <= (_rightDownChunk.x + 1) * _chunkSize.x; i++)
		{
			for (int j = _rightDownChunk.y * _chunkSize.y; j <= (_leftTopChunk.y + 1) * _chunkSize.y; j++)
			{
				id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 1)
				{
					_FloorTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
	}

	//Place around floor tile
	private void SetWallTiles()
	{
		for (int i = (_rightDownChunk.x + 1) * _chunkSize.x; i >= _leftTopChunk.x * _chunkSize.x; i--)
		{
			for (int j = (_leftTopChunk.y + 1) * _chunkSize.y; j >= _rightDownChunk.y * _chunkSize.y; j--)
			{
				if (Has(new Vector3Int(i, j, 0)) && HasUp(new Vector3Int(i, j, 0)) == false)
				{
					if (i % 2 == 0)
					{
						_FloorTilemap.SetTile(new Vector3Int(i, j + 2, 0), _baseWallTile1);
						_FloorTilemap.SetTile(new Vector3Int(i, j + 1, 0), _baseWallTile3);
					}
					else
					{
						_FloorTilemap.SetTile(new Vector3Int(i, j + 2, 0), _baseWallTile2);
						_FloorTilemap.SetTile(new Vector3Int(i, j + 1, 0), _baseWallTile4);
					}
				}
			}
		}
	}

	private void SetCollidWallTiles()
	{
		int[] id = new int[9];

		for (int i = (_leftTopChunk.x - 2) * _chunkSize.x; i <= (_rightDownChunk.x + 3) * _chunkSize.x; i++)
		{
			for (int j = (_rightDownChunk.y - 2) * _chunkSize.y; j <= (_leftTopChunk.y + 3) * _chunkSize.y; j++)
			{
				id = GetTileID(new Vector3Int(i, j, 0));

				if (id[4] == 0)
				{
					_CollidWallTilemap.SetTile(new Vector3Int(i, j, 1), GetTileByID(id).tile);
				}
			}
		}
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

	private WallTile GetTileByID(int[] id)
	{
		if (id.Length != 9)
		{
			Debug.LogError("Incorrect ID");
		}

		foreach (var tile in wallTiles)
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

		return wallTiles[0];
	}

	private int BoolToInt(bool flag)
	{
		if (flag)
		{
			return 1;
		}
		return 0;
	}

	#region HasFunctions
	private bool Has(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition);
	}

	private bool HasRight(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right);
	}

	private bool HasRightDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.down);
	}

	private bool HasDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.down);
	}

	private bool HasLeftDown(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.down);
	}

	private bool HasLeft(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left);
	}

	private bool HasLeftUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.left + Vector3Int.up);
	}

	private bool HasUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.up);
	}

	private bool HasRightUp(Vector3Int tilePosition)
	{
		return _FloorTilemap.HasTile(tilePosition + Vector3Int.right + Vector3Int.up);
	}
	#endregion

	private void GenerateMap()
	{
		GenerateChunk(Vector3Int.zero, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up, RoomType.hallwayVertic);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 2, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 3, RoomType.hallwayVertic);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 4, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 2 + Vector3Int.right, RoomType.hallwayHoriz);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 2 + Vector3Int.right * 2, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 2 + Vector3Int.right * 3, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 3 + Vector3Int.right * 2, RoomType.Room);
		GenerateChunk(Vector3Int.zero + Vector3Int.up * 3 + Vector3Int.right * 3, RoomType.Room);
	}

	private void GenerateChunk(Vector3Int chunkPosition, RoomType roomType)
	{
		Vector3Int startPosition = chunkPosition * _chunkSize;

		if (roomType == RoomType.Room)
		{
			for (int x = 0; x < _chunkSize.x; x++)
			{
				for (int y = 0; y < _chunkSize.y; y++)
				{
					if (x % 2 == 0)
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile1);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile2);
						}
					}
					else
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile3);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile4);
						}
					}
				}
			}
		}
		else if (roomType == RoomType.hallwayHoriz)
		{
			for (int x = 0; x < _chunkSize.x; x++)
			{
				for (int y = _chunkSize.y / 2 - 1; y < _chunkSize.y / 2 + 1; y++)
				{
					if (x % 2 == 0)
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile1);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile2);
						}
					}
					else
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile3);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile4);
						}
					}
				}
			}
		}
		else if (roomType == RoomType.hallwayVertic)
		{
			for (int x = _chunkSize.x / 2 - 1; x < _chunkSize.x / 2 + 1; x++)
			{
				for (int y = 0; y < _chunkSize.y; y++)
				{
					if (x % 2 == 0)
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile1);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile2);
						}
					}
					else
					{
						if (y % 2 == 0)
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile3);
						}
						else
						{
							_FloorTilemap.SetTile(startPosition + Vector3Int.one * new Vector3Int(x, y, 0), _baseFloorTile4);
						}
					}
				}
			}
		}

		if (_leftTopChunk.x > chunkPosition.x)
		{
			_leftTopChunk.x = chunkPosition.x;
		}
		if (_leftTopChunk.y < chunkPosition.y)
		{
			_leftTopChunk.y = chunkPosition.y;
		}
		if (_rightDownChunk.x < chunkPosition.x)
		{
			_rightDownChunk.x = chunkPosition.x;
		}
		if (_rightDownChunk.y > chunkPosition.y)
		{
			_rightDownChunk.y = chunkPosition.y;
		}
	}
}