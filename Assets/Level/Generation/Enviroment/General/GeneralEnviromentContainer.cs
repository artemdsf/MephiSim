using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralEnviromentContainer", menuName = "ScriptableObjects/Enviroment/General/GeneralEnviromentContainer", order = 0)]
public class GeneralEnviromentContainer : EnviromentContainer
{
	public List<GeneralEnviromentInfo> Items = new List<GeneralEnviromentInfo>();
}