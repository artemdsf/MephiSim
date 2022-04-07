using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private WeaponStats _weaponStats;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = _weaponStats.WeaponSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBottom")
        {
            PlayerWeapon _playerWeapon = collision.GetComponentInParent<PlayerWeapon>();

            WeaponStats temp = _playerWeapon.Stats;
            _playerWeapon.GiveWeapon(_weaponStats);

            if (temp != null)
            {
                _weaponStats = temp;
                _renderer.sprite = temp.WeaponSprite;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
