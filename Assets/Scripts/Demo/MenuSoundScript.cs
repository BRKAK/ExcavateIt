using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundScript : MonoBehaviour
{
    AudioSource asource;

    private void Start() {
        asource = GetComponent<AudioSource>();
    }

    public void playMenuSoundSFX(){  
        asource.Play();  
    }

}
