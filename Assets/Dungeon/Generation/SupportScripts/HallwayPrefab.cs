using UnityEngine;

[CreateAssetMenu(fileName = "HallwayPrefab", menuName = "ScriptableObjects/RoomPrefabs/HallwayPrefab", order = 1)]
public class HallwayPrefab : RoomPrefab
{
    public readonly RoomType RoomType = RoomType.Hallway;
}
