using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RoomPrefabContainer", menuName = "ScriptableObjects/RoomPrefabs/RoomPrefabContainer", order = 0)]
public class RoomPrefabContainer : ScriptableObject
{
    public List<StartRoomPrefab> StartRooms;
    public List<EnemyRoomPrefab> EnemyRooms;
    public List<SpecialRoomPrefab> SpecialRooms;
    public List<DoubeRoomPrefab> DoubeRooms;
    public List<HallwayPrefab> Hallways;
}
