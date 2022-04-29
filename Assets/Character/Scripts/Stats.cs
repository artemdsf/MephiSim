using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Characters/Stats/CharacterStats", order = 0)]
public class Stats : ScriptableObject
{
	[Min(0)]
	public float MaxHP = 100;
	[Range(0, 100)]
	public float DefaultDef = 0;
	[Min(0)]
	public float DefaultSpeed = 10;
}