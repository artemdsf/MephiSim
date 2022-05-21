using UnityEngine;

public class PlayerSpellCaster : SpellCaster
{
    private Player _player;

    private LayerMask _raycastLayer;
    [SerializeField] private uint _maxSpellsCount;
    public uint MaxSpellsCount => _maxSpellsCount;

    public delegate void SpellsChanched(PlayerSpellCaster playerSpellCaster);
    public event SpellsChanched OnSpellsChanged;

    private void Awake()
	{
		_raycastLayer = LayerMask.GetMask("Floor");
	}

    private void Start()
    {
        _player = GetComponent<Player>();
        OnSpellsChanged(this);
    }

    public override bool AddSpell(Spell spell)
    {
        bool suceed = false;

        if (GetSpellsCount() >= _maxSpellsCount)
        {
            return false;
        }

        suceed = base.AddSpell(spell);

        if (suceed)
        {
            OnSpellsChanged(this);
        }

        return suceed;
    }

    public Spell AddSpell(Spell spell, uint instedOfIndex)
    {
        Spell oldSpell = GetSpell(spell.SpellName);
        if (oldSpell)
        {
            return oldSpell;
        }

        if (GetSpellsCount() >= _maxSpellsCount)
        {
            oldSpell = GetSpell((int)instedOfIndex);
            SetSpell((int)instedOfIndex, spell);
            OnSpellsChanged(this);
            return oldSpell;
        }
        else
        {
            AddSpell(spell);
            return null;
        }

    }

    private void Update()
    {
        for (int i = 0; i < GetSpellsCount(); i++)
        {
            if (Input.GetButtonDown("Spell" + (i + 1).ToString()))
            {
                Spell spell = GetSpell(i);
                if (spell != null && _player.Mana >= spell.ManaCost)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null)
                    {
                        if (spell.Type == Spell.SpellType.AOE)
                            CastAreaSpell(spell, hit.point);
                        _player.SpendMana(spell.ManaCost);
                    }
                }
            }
        }
    }
}
