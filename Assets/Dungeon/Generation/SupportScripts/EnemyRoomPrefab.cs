using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/EnemyRoomPrefab", order = 1)]
public class EnemyRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.EnemyRoom;
}
