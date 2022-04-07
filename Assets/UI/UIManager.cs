using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _manaSlider;
    [SerializeField] private Image _weaponSlot;

    private Player _player;
    private PlayerWeapon _weapon;

    private void Start()
    {
        _player = CharactersManager.instance.GetPlayer();
        _hpSlider.maxValue = _player.MaxHP;

        _weapon = _player.GetComponent<PlayerWeapon>();
        _weapon.OnWeaponChanging += DisplayWeapon;
        _weaponSlot.enabled = false;
    }

    private void FixedUpdate()
    {
        _hpSlider.value = _player.HP;
    }

    void DisplayWeapon(WeaponStats stats) 
    {
        if (stats != null && stats.WeaponSprite != null)
        {
            _weaponSlot.enabled = true;
            _weaponSlot.sprite = stats.WeaponSprite;
        }
        else
        {
            _weaponSlot.enabled = false;
        }
    }

}
