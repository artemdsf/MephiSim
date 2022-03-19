using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _manaSlider;
    private Player _player;

    private void Start()
    {
        _player = CharactersManager.instance.GetPlayer();
        _hpSlider.maxValue = _player.MaxHP;
    }

    private void FixedUpdate()
    {
        _hpSlider.value = _player.HP;
    }
}
