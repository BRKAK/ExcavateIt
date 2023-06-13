using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSoundScript : MonoBehaviour
{
    AudioSource asource;

    private void Start() {
        asource = GetComponent<AudioSource>();
    }

    public void playGasSoundSFX(){  
        asource.Play();  
    }

    public void stopGasSoundSFX(){
        asource.Stop(); 
    }
}
