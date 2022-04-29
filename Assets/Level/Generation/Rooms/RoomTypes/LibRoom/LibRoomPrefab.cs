using UnityEngine;

[CreateAssetMenu(fileName = "LibRoomPrefab", menuName = "ScriptableObjects/Generation/RoomPrefabs/LibRoomPrefab", order = 1)]
public class LibRoomPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.LibRoom;
}
