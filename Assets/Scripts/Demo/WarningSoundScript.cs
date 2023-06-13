using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSoundScript : MonoBehaviour
{
    AudioSource asource;

    private void Start() {
        asource = GetComponent<AudioSource>();
    }

    public void playWarningSoundSFX(){  
        asource.Play();  
    }

    public void stopWarningSoundSFX(){
        asource.Stop(); 
    }
}
