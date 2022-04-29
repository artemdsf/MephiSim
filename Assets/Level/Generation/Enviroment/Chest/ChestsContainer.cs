using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestsContainer", menuName = "ScriptableObjects/Enviroment/Chest/ChestContainer", order = 0)]
public class ChestsContainer : EnviromentContainer
{
	public List<ChestInfo> Chests = new List<ChestInfo>();
}