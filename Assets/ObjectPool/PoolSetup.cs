using UnityEngine;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : MonoBehaviour
{
	[SerializeField] private PoolManager.PoolPart[] _pools;

	private GameObject objectParent;

	private void OnValidate()
	{
		for (int i = 0; i < _pools.Length; i++)
		{
			_pools[i].Name = _pools[i].PoolingPrefab.name;
		}
	}

	private void Awake()
	{
		if (PoolManager.PoolSetup != null)
		{
			Destroy(this);
		}

		PoolManager.PoolSetup = this;

		Initialize();
	}

	private void Initialize()
	{
		PoolManager.Initialize(_pools);
	}
}