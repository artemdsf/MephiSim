using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Stats/PlayerStats", order = 0)]
public class PlayerStats : Stats
{
	[Min(0)]
	public float MaxMana = 100;
}
