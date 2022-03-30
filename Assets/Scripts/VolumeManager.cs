using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    AudioSource sound;
    public static float volume = 0.3f;

    void Start() {
        sound =  GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) ) && volume < 1) {
            volume += 0.1f;
            AudioListener.volume = volume;
            sound.volume = volume;
        }
        if ((Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) ) && volume > 0) {
            volume -= 0.1f;
            AudioListener.volume = volume;
            sound.volume = volume;
        }
    }
}
