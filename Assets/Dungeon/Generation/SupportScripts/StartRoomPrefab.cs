using UnityEngine;

[CreateAssetMenu(fileName = "StartRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/StartRoomPrefab", order = 1)]
public class StartRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.StartRoom;
}