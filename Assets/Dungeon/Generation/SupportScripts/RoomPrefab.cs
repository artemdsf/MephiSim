using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPrefab : ScriptableObject
{
    public Tilemap Tilemap;

    public Vector3Int LeftDownCorner;

    public bool IsRightOpen;
    public bool IsLeftOpen;
    public bool IsUpOpen;
    public bool IsDownOpen;
}

[CreateAssetMenu(fileName = "StartRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/StartRoomPrefab", order = 1)]
public class StartRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.StartRoom;
}

[CreateAssetMenu(fileName = "EnemyRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/EnemyRoomPrefab", order = 1)]
public class EnemyRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.EnemyRoom;
}

[CreateAssetMenu(fileName = "SpecialRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/SpecialRoomPrefab", order = 1)]
public class SpecialRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.SpecialRoom;
}

[CreateAssetMenu(fileName = "DoubeRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/DoubeRoomPrefab", order = 1)]
public class DoubeRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.DoubeRoom;
}

[CreateAssetMenu(fileName = "HallwayPrefab", menuName = "ScriptableObjects/RoomPrefabs/HallwayPrefab", order = 1)]
public class HallwayPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.Hallway;
}