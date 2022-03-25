using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        Vector3 startPosition = new Vector3(
            (LevelMap.StartPosition.x + 0.5f) * LevelMap.ChunkSize.x,
            (LevelMap.StartPosition.y + 0.5f) * LevelMap.ChunkSize.y, 0);

        Instantiate(_player, startPosition, Quaternion.identity);
    }
}
