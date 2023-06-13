using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAudioScript : MonoBehaviour
{
    AudioSource asource;
    private void Start() {
        asource = GetComponent<AudioSource>();
    }

    public void playIdleEngineSFX(){  
        asource.Play();  
    }

    public void stopIdleEngineSFX(){
        asource.Stop(); 
    }
}
