using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomPrefabContainer", menuName = "ScriptableObjects/RoomPrefabs/RoomPrefabContainer", order = 0)]
public class RoomPrefabContainer : ScriptableObject
{
	public List<RoomPrefab> LibRooms;
	public List<RoomPrefab> BossRooms;
	public List<RoomPrefab> StartRooms;
	public List<RoomPrefab> EnemyRooms;
	public List<RoomPrefab> Hallways;
}