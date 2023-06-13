using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] MeshRenderer[] mrs;
    [SerializeField] Material wellDoneMaterial;
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "dustParticle"){
            Debug.Log("well done dust");
            foreach (MeshRenderer mr in mrs)
            {
                mr.material = wellDoneMaterial;
            }

        }
    }
}
