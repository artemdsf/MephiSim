using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GenerateLevel _generationScript;

    private void Start()
    {
        Vector3 startPosition = new Vector3(
            (_generationScript._startPosition.x + 0.5f) * _generationScript._chunkSize.x,
            (_generationScript._startPosition.y + 0.5f) * _generationScript._chunkSize.y, 0);

        Instantiate(_player, startPosition, Quaternion.identity);
    }
}
