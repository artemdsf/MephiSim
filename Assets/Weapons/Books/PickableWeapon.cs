using UnityEngine;

public class PickableWeapon : MonoBehaviour, IUsable
{
	[SerializeField] private WeaponStats _weaponStats;
	private SpriteRenderer _renderer;
	private Player _player;

	private void Awake()
	{
		_renderer = GetComponentInChildren<SpriteRenderer>();
		_renderer.sprite = _weaponStats.WeaponSprite;
	}

	private void Start()
	{
		_player = CharactersManager.Instance.GetPlayer();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "PlayerBottom")
		{
			_player.InteractableObject = this;
			_player.ShowUseHint();
			_player.ShowDescription(_weaponStats.Description);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "PlayerBottom")
		{
			_player.InteractableObject = null;
			_player.HideKeyHint();
			_player.HideDescription();
		}
	}

	public void Use()
	{
		PlayerWeapon _playerWeapon = _player.GetComponentInParent<PlayerWeapon>();

		WeaponStats temp = _playerWeapon.Stats;
		_playerWeapon.GiveWeapon(_weaponStats);

		if (temp != null)
		{
			_weaponStats = temp;
			_renderer.sprite = temp.WeaponSprite;

			_player.ShowDescription(_weaponStats.Description);
		}
		else
		{
			_player.HideDescription();
			Destroy(gameObject);
		}
	}
}
