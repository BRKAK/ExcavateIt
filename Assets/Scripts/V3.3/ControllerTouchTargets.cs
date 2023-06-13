using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerTouchTargets : MonoBehaviour
{
    [SerializeField] GameObject[] positions;
    [SerializeField] AudioClip interactionSound;
    [SerializeField] Text congratsTxt;
    TouchTargetV2 touchTargetObj;
    int currentPos;

    private void Awake() {
        touchTargetObj = FindObjectOfType<TouchTargetV2>();
        currentPos = 0;
        FindObjectOfType<TutorialProgress>().initTotalTarget(positions.Length);
    }

    private void Start() {
        touchTargetObj.moveTo(positions[0].transform.position);
        Debug.Log("tayfn" + positions.Length);
        congratsTxt.enabled = false;
    }

    private void Update() {
        if(currentPos == positions.Length){
            Debug.Log("win!");
            Destroy(touchTargetObj);
            congratsTxt.enabled = true;
            StartCoroutine(waiter());
            
        }
    }

    public void moveTargetToNextPosition(){ 
        if (currentPos != positions.Length ){
            currentPos++;
            touchTargetObj.moveTo(positions[currentPos].transform.position);
        }
    }


    public AudioClip getInteractionClip(){
        return interactionSound;
    } 

    IEnumerator waiter(){ 
        yield return new WaitForSeconds(4);
        PlayerPrefs.SetInt("currentLevel", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }

}
