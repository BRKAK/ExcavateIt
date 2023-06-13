using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [SerializeField] Light interiorLight, outerLight;
    [SerializeField] Button lightBtn;
    bool lightState=false;
    public void changeLightState(){
        lightState = !lightState;
        interiorLight.GetComponent<Light>().enabled = lightState;
        outerLight.GetComponent<Light>().enabled = lightState;
        if(lightState){
            lightBtn.image.color = Color.white;
        }else{
            lightBtn.image.color = Color.yellow;
        }
    }
}
