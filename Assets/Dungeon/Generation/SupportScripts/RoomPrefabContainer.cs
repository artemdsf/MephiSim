using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RoomPrefabContainer", menuName = "ScriptableObjects/RoomPrefabs/RoomPrefabContainer", order = 0)]
public class RoomPrefabContainer : ScriptableObject
{
    public List<RoomPrefab> StartRooms;
    public List<RoomPrefab> EnemyRooms;
    public List<RoomPrefab> SpecialRooms;
    public List<RoomPrefab> DoubeRooms;
    public List<RoomPrefab> Hallways;
}