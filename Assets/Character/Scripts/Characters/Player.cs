using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
	[SerializeField] private SpriteRenderer _placeForHint;

	//-----------------------------Keys-------------------------------------
	[Header("Use key")]
	[SerializeField] private Sprite _useKeySprite;
	//----------------------------------------------------------------------
	[Header("Characteristics")]
	[SerializeField] private float _maxMana;

	public IUsable InteractableObject { private get; set; }

	public float Mana
	{
		get => _mana;
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
	public float MaxMana =>_maxMana;

	private float _mana;

	private PlayerWeapon _weapon;

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
		TryChangeRoom();
		#endregion

		FindTeacher(movementDirection);
	}

	public void ShowUseHint()
	{
		_placeForHint.sprite = _useKeySprite;
	}

	public void HideKeyHint()
	{
		_placeForHint.sprite = null;
	}

	public override void Die()
	{
		LevelGenerator.Instance.Reset();
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
		if (Input.GetButtonDown("Use"))
		{
			InteractableObject?.Use();
		}
	}

	private void TryChangeRoom()
	{
		Room currentRoom = LevelMap.GetRoom(LevelMap.WorldCoordsToGrid(transform.position));
		if (currentRoom != null && currentRoom.IsVisited == false)
		{
			currentRoom.ActivateRoom();
		}
	}
}