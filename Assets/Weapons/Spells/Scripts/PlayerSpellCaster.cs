using UnityEngine;

public class PlayerSpellCaster : SpellCaster
{

    private LayerMask _raycastLayer;
    [SerializeField] private uint _maxSpellsCount;
    public uint MaxSpellsCount => _maxSpellsCount;

	private void Awake()
	{
		_raycastLayer = LayerMask.GetMask("Floor");
	}

    public override bool AddSpell(Spell spell)
    {
        if (GetSpellsCount() >= _maxSpellsCount)
        {
            return false;
        }

        return base.AddSpell(spell);
    }

    public void AddSpell(Spell spell, uint instedOfIndex)
    {
        if (GetSpellsCount() >= _maxSpellsCount)
        {
            _spells[(int)instedOfIndex] = spell;
        }
        else
        {
            base.AddSpell(spell);
        }

    }

    private void Update()
    {
        for (int i = 0; i < GetSpellsCount(); i++)
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
                            CastAreaSpell(spell, hit.point);      
                    }
                }
            }
        }
    }
}
