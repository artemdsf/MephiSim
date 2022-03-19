using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _manaSlider;
    private Player _player;

    void Start()
    {
        _player = CharactersManager.instance.GetPlayer();
        _hpSlider.maxValue = _player.MaxHP;
    }

    void Update()
    {
        Debug.Log($"{_player.HP}");
        _hpSlider.value = _player.HP;
    
    }
}
