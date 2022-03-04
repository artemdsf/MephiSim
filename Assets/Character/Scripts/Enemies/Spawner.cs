using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Character> _characters;
    [SerializeField] Vector2 _sizesSpawnZone;
    [SerializeField] float _zCoord = -10;

    public void Spawn()
    { 
        foreach(Character character in _characters)
        {
            Vector3 relativePos = new Vector3(Random.Range(-_sizesSpawnZone.x / 2, _sizesSpawnZone.x / 2), Random.Range(-_sizesSpawnZone.y / 2, _sizesSpawnZone.y / 2), _zCoord);
            Instantiate(character, transform.position + relativePos, Quaternion.identity);
        }
    }
}
