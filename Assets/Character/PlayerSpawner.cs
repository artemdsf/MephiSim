using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        Vector3 startPosition = (Vector3)LevelMap.ChunkSize * 0.5f;

        Instantiate(_player, startPosition, Quaternion.identity);
    }
}
