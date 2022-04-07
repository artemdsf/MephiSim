using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomPalette", menuName = "ScriptableObjects/RoomPalette", order = 1)]
public class RoomPalette : ScriptableObject
{
	[SerializeField] public List<Sprite> Sprites = new List<Sprite>();
}
