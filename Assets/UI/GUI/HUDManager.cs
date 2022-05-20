using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _manaSlider;

    [SerializeField] private Image _weaponIcon;
    [SerializeField] private List<Image> _spellIcons;

	[SerializeField] private Transform _description;

    private Player _player;
    private PlayerWeapon _weapon;

    private void Start()
    {
		_player = CharactersManager.Instance.GetPlayer();
		_weapon = _player.GetComponent<PlayerWeapon>();

		_player.Description = _description;
		_hpSlider.maxValue = _player.MaxHP;
        _manaSlider.maxValue = _player.MaxMana;

        _player.OnValuesChanging.AddListener(UpdateValues);
        _weapon.OnWeaponChanging += DisplayWeapon;
        _player.GetComponent<PlayerSpellCaster>().OnSpellsChanged += DisplaySpells;

        _weaponIcon.enabled = false;

		UpdateValues();
	}

	private void UpdateValues()
	{
		_hpSlider.value = _player.HP;
		_manaSlider.value = _player.Mana;
	}

	private void DisplayWeapon(WeaponStats stats) 
    {
        if (stats != null && stats.WeaponSprite != null)
        {
            _weaponIcon.enabled = true;
            _weaponIcon.sprite = stats.WeaponSprite;
        }
        else
        {
            _weaponIcon.enabled = false;
        }
    }

    private void DisplaySpells(PlayerSpellCaster playerSpellCaster)
    {
        for (int i = 0; i < _spellIcons.Count; i++)
        {
            Spell spell = playerSpellCaster.GetSpell(i);
            if (spell != null)
            {
                _spellIcons[i].enabled = true;
                _spellIcons[i].sprite = spell.Icon;
            }
            else
            {
                _spellIcons[i].enabled = false;
            }
        }
    }
}
