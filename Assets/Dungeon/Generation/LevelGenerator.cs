using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WallGenerator))]
public class LevelGenerator : MonoBehaviour
{
	private WallGenerator _wallGenerator;

	private void Start()
	{
		_wallGenerator = GetComponent<WallGenerator>();

		GenerateLevel();
	}

	private void GenerateLevel()
	{
		
	}
}