using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EffectsManager : MonoBehaviour
{
    private EffectsManager _instance;

    EffectsManager Instance => _instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
    }




}
