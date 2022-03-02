using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    private float _timeLeft;
    private Character _character;

    protected void ApplyEffect(Character character, float time) {
        _timeLeft = time;
        _character = character;
    }

    private void Update()
    {
        if (_timeLeft > 0)
        {
            onFrame();
            _timeLeft -= Time.deltaTime;
        }
            
    }

    protected abstract void onFrame();
}
