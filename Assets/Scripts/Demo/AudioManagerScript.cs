using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioClip igniteEngineSfx;
    [SerializeField] AudioClip gasEngineSfx;

    AudioSource audioS;

    private void Start() {
        audioS = GetComponent<AudioSource>();

    }

    public void playIgniteEngineSfx(){
        audioS.PlayOneShot(igniteEngineSfx);
    }

    
}
