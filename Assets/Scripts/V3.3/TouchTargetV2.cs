using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTargetV2 : MonoBehaviour
{
    ControllerTouchTargets positionsController;
    AudioSource audioSource;
    TutorialProgress tp;
    [SerializeField] ParticleSystem ps;
    bool isTriggerEntered;

    private void Awake() {
        positionsController = FindObjectOfType<ControllerTouchTargets>();
        tp = FindObjectOfType<TutorialProgress>();
        audioSource = GetComponent<AudioSource>();
        isTriggerEntered = false;
    }
    
    public void moveTo(Vector3 desiredPos){
        gameObject.transform.position = desiredPos;
        isTriggerEntered = false;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("touched!");
        if(!isTriggerEntered && other.tag == "excavatorBucketControl"){
            isTriggerEntered = true;
            ps.Play();
            tp.targetCompleted();
            Debug.Log("toucche");
            playInterActionSFX();
            //play audio
            positionsController.moveTargetToNextPosition();
        }
    }

    void playInterActionSFX(){
        audioSource.PlayOneShot(positionsController.getInteractionClip());
    }
}
