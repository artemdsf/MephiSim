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
