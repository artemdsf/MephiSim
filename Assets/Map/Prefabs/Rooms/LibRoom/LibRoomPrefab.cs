using UnityEngine;

[CreateAssetMenu(fileName = "LibRoomPrefab", menuName = "ScriptableObjects/RoomPrefabs/LibRoomPrefab", order = 1)]
public class LibRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.LibRoom;
}
