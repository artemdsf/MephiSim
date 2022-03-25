using UnityEngine;

public class RoomPrefab : ScriptableObject
{
    public GameObject Room;

    [SerializeField] private Vector3Int _leftDownCorner;
    public Vector3Int LeftDownCorner => _leftDownCorner;

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

[CreateAssetMenu(fileName = "HallwayPrefab", menuName = "ScriptableObjects/RoomPrefabs/HallwayPrefab", order = 1)]
public class HallwayPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.Hallway;
}