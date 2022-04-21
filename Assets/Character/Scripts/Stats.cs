using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats/CharacteStats", order = 0)]
public class Stats : ScriptableObject
{
	public float MaxHP = 100;
	public float DefaultDef = 0;
	public float DefaultSpeed = 10;
}