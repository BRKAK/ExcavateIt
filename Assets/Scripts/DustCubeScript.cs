using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCubeScript : MonoBehaviour
{
    [SerializeField] Transform bucketPivotOne;
    [SerializeField] GameObject dustParticlePrefab;

    bool isFirstInteraction = true;

    float aX, aY, bX, bY;
    float randX, randZ;

    Vector3 randPos;

    private void Start() {
        /*aX = bucketPivotOne.transform.position.x;
        aY = bucketPivotOne.transform.position.z;
        bX = bucketPivotTwo.transform.position.x;
        bY = bucketPivotTwo.transform.position.z;*/
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "excavatorDigControl" && isFirstInteraction){
            isFirstInteraction = false;
            Vector3 n = new Vector3(bucketPivotOne.position.y, bucketPivotOne.position.x, bucketPivotOne.position.z);
            Debug.Log("Pozisyonn " + bucketPivotOne.position);
            Instantiate(dustParticlePrefab, transform.position, transform.rotation);
            Debug.Log("Dust cube interacted!");
            Destroy(this.gameObject);

        }
    }

    Vector3 getRandomPos(){
        randX = Random.Range(aX,bX);
        randZ = Random.Range(aY, bY);

        return new Vector3(randX, bucketPivotOne.position.z, randZ);
    }
}
