using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    [SerializeField] public Vector3Int _chunkSize = new Vector3Int(18, 10, 0);
	[SerializeField] protected int _mapSize = 15;
	[SerializeField] private int _hallwayWidth = 2;
}
