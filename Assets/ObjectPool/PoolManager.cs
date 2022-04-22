using UnityEngine;

public static class PoolManager
{
	public static PoolSetup PoolSetup;

	private static PoolPart[] _pools;
	

	[System.Serializable]
	public struct PoolPart
	{
		public string Name;
		public PoolObject PoolingPrefab;
		public int Count;
		public ObjectPooling Pool;
		public GameObject ObjectsParent;
	}

	public static void Initialize(PoolPart[] newPools)
	{
		_pools = newPools;

		for (int i = 0; i < _pools.Length; i++)
		{
			if (_pools[i].PoolingPrefab != null)
			{
				_pools[i].Pool.Initialize(_pools[i].Count, _pools[i].PoolingPrefab, _pools[i].ObjectsParent.transform);
			}
		}
	}

	public static GameObject GetObject(string name, Vector3 position, Quaternion rotation)
	{
		GameObject result = null;

		if (_pools != null)
		{
			for (int i = 0; i < _pools.Length; i++)
			{
				if (string.Compare(_pools[i].Name, name) == 0)
				{
					result = _pools[i].Pool.GetObject().gameObject;
					result.transform.position = position;
					result.transform.rotation = rotation;
					result.SetActive(true);
					return result;
				}
			}
		}

		return result;
	}
}
