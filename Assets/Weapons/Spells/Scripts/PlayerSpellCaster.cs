using UnityEngine;

public class PlayerSpellCaster : SpellCaster
{
	private LayerMask _raycastLayer;

	private void Awake()
	{
		_raycastLayer = LayerMask.GetMask("Floor");
	}

	private void Update()
	{
		for (int i = 0; i < _maxSpellsCount; i++)
		{
			if (Input.GetButtonDown("Spell" + (i + 1).ToString()))
			{
				Spell spell = GetSpell(i);
				if (spell != null)
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

					if (hit.collider != null)
					{
						if (spell.Type == Spell.SpellType.AOE)
						{
							CastAreaSpell(spell, hit.point);
						}
					}
				}
			}
		}
	}
}
