using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Key
{
	public KeyCode KeyCode;
	public Sprite KeySprite;
}

public enum KeyType
{
	Use
}

public class Player : Character
{
	[SerializeField] private SpriteRenderer _placeForHint;

	//-----------------------------Keys-------------------------------------
	[Header("Use key")]
	[SerializeField] private KeyCode _useKeyCode = KeyCode.E;
	[SerializeField] private Sprite _useKeySprite;
	private Key _useKey;

	private List<Key> _keys = new List<Key>();
	//----------------------------------------------------------------------
	[Header("Characteristics")]
	[SerializeField] private float _maxMana;
	private float _mana;

	public float Mana
	{
		get
		{
			return _mana;
		}

		private set
		{
			if (_mana >= 0)
			{
				_mana = value;
			}
			else
			{
				_mana = 0;
			}
		}
	}
	public float MaxMana
	{
		get
		{
			return _maxMana;
		}
	}

	public IUsable InteractableObject { private get; set; }

	private PlayerWeapon _weapon;
	internal Action<Room> ChangedRoom;

	protected override void Awake()
	{
		_useKey.KeyCode = _useKeyCode;
		_useKey.KeySprite = _useKeySprite;
		_keys.Add(_useKey);

		base.Awake();
	}

	protected override void Start()
	{
		_placeForHint.sprite = null;

		_weapon = GetComponent<PlayerWeapon>();
		if (_weapon == null)
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
		#endregion

		FindTeacher(movementDirection);
	}

	public void ShowKeyHint(KeyType keyType)
	{
		_placeForHint.sprite = _keys[(int)keyType].KeySprite;
	}

	public void HideKeyHint()
	{
		_placeForHint.sprite = null;
	}

	public override void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void FindTeacher(Vector2 direction)
	{
		Teacher nearestEnemy = CharactersManager.instance.FindNearestTeacher(transform.position);
		if (_weapon && nearestEnemy != null && direction == Vector2.zero)
		{
			_weapon.Shoot(nearestEnemy.transform.position - _weapon.WeaponEnd.position);
		}
	}

	private void TryUse()
	{
		if (Input.GetKeyDown(_useKeyCode))
		{
			InteractableObject?.Use();
		}
	}
}