using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    private float radius;

    void Start()
    {
        //radius = Mathf.Max(LevelMap.ChunkSize.x, LevelMap.ChunkSize.y);
    }

    void Update()
    {
        Room currentRoom = LevelMap.GetRoom(LevelMap.WorldCoordsToGrid(transform.position));

        if (currentRoom != null && currentRoom.IsVisited == false)
        {
            
            Light2D lightInstance = Instantiate(_light.gameObject, currentRoom.CenterPos, Quaternion.identity).GetComponent<Light2D>();
            lightInstance.pointLightOuterRadius = radius;

            currentRoom.IsVisited = true;
        }
    }
}
