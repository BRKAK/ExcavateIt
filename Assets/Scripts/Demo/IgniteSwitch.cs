using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteSwitch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("blip!");
    }

    private void OnMouseDown() {
        Debug.Log("blop!");
    }

    
}
