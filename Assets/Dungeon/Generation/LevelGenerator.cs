using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : TileGenerator
{
	private void Start()
	{
		GenerateLevel();
	}

	/// <summary>
	/// Generate level
	/// </summary>
	private void GenerateLevel()
	{
		AddStartRoom();

		GenerateFloor();
	}
}