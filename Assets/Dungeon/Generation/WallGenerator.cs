using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

public class WallGenerator : MonoBehaviour
{
	[SerializeField] private RoomPalette roomPalette;

	private Tile _tile;

	private List<WallTile> _wallTiles = new List<WallTile>();
	private List<Tile> _nonCollidWallTiles = new List<Tile>();
	private List<Tile> _floorTiles = new List<Tile>();

	private void CreateCollidWallTile(int spriteIndex, int[] tileID)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = roomPalette._sprites[spriteIndex];
		WallTile tile = new WallTile(_tile, tileID);

		_wallTiles.Add(tile);
	}

	private void CreateNonCollidWallTile(int spriteIndex)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = roomPalette._sprites[spriteIndex];

		_nonCollidWallTiles.Add(_tile);
	}

	private void CreateFloorTile(int spriteIndex)
	{
		_tile = ScriptableObject.CreateInstance<Tile>();

		_tile.sprite = roomPalette._sprites[spriteIndex];

		_floorTiles.Add(_tile);
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
}

