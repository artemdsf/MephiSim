using UnityEngine;

[CreateAssetMenu(fileName = "HramStats", menuName = "ScriptableObjects/Characters/Stats/HramStats", order = 0)]
public class HramStats : Stats
{
	public float ShootingCooldown;
	public float TimeBeforeChangeDirection;
	public int ShotingDirections = 8;
	public float AngleOffset = 20;
}