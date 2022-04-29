using UnityEngine;

public class Chest
{
	public Chest(Transform transform, ChestInfo chestInfo)
	{
		foreach (Transform child in transform)
		{
			if (child.tag == "Reward")
			{
				_reward = child.gameObject;
				break;
			}
		}

		_closed = chestInfo.Closed;
		_opened = chestInfo.Opened;

		_spriteRenderer = transform.GetComponent<SpriteRenderer>();
	}

	public GameObject Reward => _reward;
	public Sprite Closed => _closed;
	public Sprite Opened => _opened;

	public bool IsClosed = true;

	private GameObject _reward;
	private Sprite _closed;
	private Sprite _opened;

	private SpriteRenderer _spriteRenderer;

	public void OpenChest()
	{
		IsClosed = false;
		_spriteRenderer.sprite = _opened;
		_reward.SetActive(true);
	}

	public void CloseChest()
	{
		IsClosed = true;
		_spriteRenderer.sprite = _closed;
		_reward.SetActive(false);
	}
}

public class ChestPrefab : MonoBehaviour
{
	[SerializeField] private ChestsContainer _chestsContainer;
	[Min(0)]
	[SerializeField] private int _prefabNum = 0;
	[SerializeField] private bool _randomSprite = false;

	private Chest _chest;

	private void OnValidate()
	{
		if (_randomSprite)
		{
			_chest = new Chest(transform, _chestsContainer.GetItem(_chestsContainer.Chests));
		}
		else
		{
			_chest = new Chest(transform, _chestsContainer.GetItem(_chestsContainer.Chests, _prefabNum));
		}
		_chest.OpenChest();
	}

	private void Awake()
	{
		if (_randomSprite)
		{
			_chest = new Chest(transform, _chestsContainer.GetItem(_chestsContainer.Chests));
		}
		else
		{
			_chest = new Chest(transform, _chestsContainer.GetItem(_chestsContainer.Chests, _prefabNum));
		}
		_chest.CloseChest();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_chest.IsClosed && collision.tag == "PlayerBottom")
		{
			_chest.OpenChest();
		}
	}
}