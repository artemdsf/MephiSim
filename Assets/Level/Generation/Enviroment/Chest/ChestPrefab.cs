using UnityEngine;

public class ChestPrefab : MonoBehaviour
{
	[SerializeField] private GameObject _reward;
	[SerializeField] private Sprite _closed;
	[SerializeField] private Sprite _opened;

	private SpriteRenderer _spriteRenderer;
	private bool _isClosed = true;

	private void Awake()
	{
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = _closed;
		_reward.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_isClosed && collision.tag == "PlayerBottom")
		{
			OpenChest();
		}
	}

	private void OpenChest()
	{
		_isClosed = false;

		_spriteRenderer.sprite = _opened;
		_reward.SetActive(true);
	}
}
