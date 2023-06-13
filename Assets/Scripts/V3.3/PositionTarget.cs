using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTarget : MonoBehaviour
{
    ControllerPositionTarget positionsController;
    AudioSource audioSource;
    TutorialProgress tp;
    [SerializeField] ParticleSystem ps;
    bool isTriggerEntered;

    private void Awake() {
        positionsController = FindObjectOfType<ControllerPositionTarget>();
        tp = FindObjectOfType<TutorialProgress>();
        audioSource = GetComponent<AudioSource>();
        isTriggerEntered = false;
    }
    
    public void moveTo(Vector3 desiredPos){
        gameObject.transform.position = desiredPos;
        isTriggerEntered = false;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("well done");
        if(!isTriggerEntered && other.tag == "excavatorPositionControl"){
            isTriggerEntered = true;
            ps.Play();
            tp.targetCompleted();
            Debug.Log("well done");
            playInterActionSFX();
            //play audio
            positionsController.moveTargetToNextPosition();
        }
    }

    void playInterActionSFX(){
        audioSource.PlayOneShot(positionsController.getInteractionClip());
    }
}
