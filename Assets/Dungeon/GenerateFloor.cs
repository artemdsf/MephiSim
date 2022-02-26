using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateFloor : MonoBehaviour
{
    [SerializeField] List<Tilemap> _floorMaps = new List<Tilemap>();
    [SerializeField] Tilemap _collisionMap;


    // Start is called before the first frame update
    private void Start()
    {
        _collisionMap.InsertCells(Vector3Int.zero, Vector3Int.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
