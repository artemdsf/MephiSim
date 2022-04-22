using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character
{
	[SerializeField] private SpriteRenderer _placeForHint;

	[Header("Use key")]
	[SerializeField] private Sprite _useKeySprite;

	public UnityEvent OnValuesChanging = new UnityEvent();

	public IUsable InteractableObject { private get; set; }

	public Transform Description;

	public float Mana
	{
		get => _mana;
		private set
		{
			if (_mana < 0)
			{
				_mana = 0;
			}
			else if (_mana > _playerStats.MaxMana)
			{
				_mana = _playerStats.MaxMana;
			}
			else
			{
				_mana = value;
			}
		}
	}

	private float _mana;

	private PlayerStats _playerStats;

	private PlayerWeapon _weapon;

	protected override void Awake()
	{
		_playerStats = (PlayerStats)Stats;

		Mana = _playerStats.MaxMana;

		base.Awake();
	}

	protected override void Start()
	{
		_placeForHint.sprite = null;

		if (TryGetComponent(out _weapon) == false)
		{
			Debug.LogWarning("There is no weapon script assigned to player", this);
		}

		base.Start();
	}

	private void Update()
	{
		#region Movement
		Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Move(movementDirection);
		#endregion

		#region Interactions
		TryUse();
		TryChangeRoom();
		#endregion

		FindEnemy(movementDirection);
	}

	public void SpendMana(float value)
	{
		if (value < 0)
		{
			value = 0;
		}

		Mana -= value;

		OnValuesChanging.Invoke();
	}

	public void RestoreMana(float value)
	{
		if (value < 0)
		{
			value = 0;
		}

		Mana += value;

		OnValuesChanging.Invoke();
	}

	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);

		OnValuesChanging.Invoke();
	}

	public override void Heal(float hp)
	{
		base.Heal(hp);

		OnValuesChanging.Invoke();
	}

	public void ShowUseHint()
	{
		_placeForHint.sprite = _useKeySprite;
	}

	public void HideKeyHint()
	{
		_placeForHint.sprite = null;
	}

	public void ShowDescription(string text)
	{
		foreach (Transform child in Description)
		{
			if (child.tag == "Text")
			{
				child.gameObject.GetComponent<Text>().text = text;
				break;
			}
		}

		Description.gameObject.SetActive(true);
	}

	public void HideDescription()
	{
		Description.gameObject.SetActive(false);
	}

	public override void Die()
	{
		LevelGenerator.Instance.Reset();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void FindEnemy(Vector2 direction)
	{
		Enemy nearestEnemy = CharactersManager.instance.FindNearestEnemy(transform.position);
		if (_weapon && nearestEnemy != null && direction == Vector2.zero)
		{
			_weapon.Shoot(nearestEnemy.transform.position - _weapon.WeaponEnd.position);
		}
	}

	private void TryUse()
	{
		if (Input.GetButtonDown("Use"))
		{
			InteractableObject?.Use();
		}
	}

	private void TryChangeRoom()
	{
		Room currentRoom = LevelInfo.GetRoom(LevelInfo.WorldCoordsToGrid(transform.position));
		if (currentRoom != null && currentRoom.IsVisited == false)
		{
			if (currentRoom.RoomType == RoomType.EnemyRoom)
			{
				TasksManager.Instance.NewTask();
			}
			
			currentRoom.ActivateRoom();
		}
	}
}