using System.Collections.Generic;
using UnityEngine;

public class EnviromentContainer : ScriptableObject
{
	public T GetItem<T>(List<T> items)
	{
		return items[Random.Range(0, items.Count)];
	}

	public T GetItem<T>(List<T> items, int num)
	{
		if (num >= 0 && num < items.Count)
		{
			return items[num];
		}
		else
		{
			return items[0];
		}
	}
}
