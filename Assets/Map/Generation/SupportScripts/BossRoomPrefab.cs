using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/BossRoomPrefab", order = 1)]
public class BossRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.BossRoom;
}
