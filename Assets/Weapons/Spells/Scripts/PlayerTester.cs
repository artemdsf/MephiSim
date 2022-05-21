using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTester : MonoBehaviour
{
    [SerializeField] private List<Spell> _spells = new List<Spell>();
    private PlayerSpellCaster _playerSpellCaster;
    private Player _player;
    private EffectsManager _effectsManager;

    private bool _godModeActive = false;

    void Start()
    {
        _playerSpellCaster = GetComponent<PlayerSpellCaster>();
        _player = GetComponent<Player>();
        _effectsManager = GetComponent<EffectsManager>();
    }

    void Update()
    {
        for (int i = 0; i < _spells.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "F" + (i + 1))))
            {
                _playerSpellCaster.AddSpell(_spells[i], _playerSpellCaster.MaxSpellsCount - 1);
                Debug.Log($"Giving spell {_spells[i].SpellName} to the player");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _player.RestoreMana(_player.MaxMana);
            Debug.Log("Mana has been restored");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _godModeActive = !_godModeActive;
            if (_godModeActive)
            {
                Debug.Log("God mode is activated!");
                _effectsManager.AddEffect(new GodMode());
            }
            else
            {
                Debug.Log("God mode is deactivated!");
                _effectsManager.RemoveEffect(new GodMode());
            }
        }
    }
}
