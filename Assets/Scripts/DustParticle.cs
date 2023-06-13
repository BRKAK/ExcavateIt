using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticle : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
       //rb.velocity = Vector3.zero; 
    }

    
}
