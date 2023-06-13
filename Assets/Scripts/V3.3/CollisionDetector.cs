using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "GameController")
        {
            Debug.Log("Controller");
            return;
        }
        if (collision.gameObject.name=="Bucket" && collision.collider.gameObject.tag == "Gorund")
        {
            Debug.Log("Collision!!");
        }
        //Debug.Log("Geldi");
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("Digging");
        }
    }
}
