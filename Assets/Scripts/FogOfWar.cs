using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    //private float radius;

    void Start()
    {
        CharactersManager.instance.GetPlayer().ChangedRoom += ChangedRoom;
    }

    void ChangedRoom(Room room)
    {
        Light2D lightInstance = Instantiate(_light.gameObject, room.CenterPos + Vector3.up * 1.8F, Quaternion.identity).GetComponent<Light2D>();
        lightInstance.transform.localScale = new Vector3(LevelMap.ChunkSize.x + 0.7F, LevelMap.ChunkSize.y + 3F, 1);
    }

}
