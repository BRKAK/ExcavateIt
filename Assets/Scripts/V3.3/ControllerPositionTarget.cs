using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerPositionTarget : MonoBehaviour
{
    [SerializeField] GameObject[] positions;
    [SerializeField] AudioClip interactionSound;
    [SerializeField] Text congratsTxt;
    PositionTarget positionTargetObj;
    int currentPos;

    private void Awake() {
        positionTargetObj = FindObjectOfType<PositionTarget>();
        currentPos = 0;
        FindObjectOfType<TutorialProgress>().initTotalTarget(positions.Length );
    }

    private void Start() {
        positionTargetObj.moveTo(positions[0].transform.position);
    }

    private void Update() {
        if(currentPos == positions.Length){
            Debug.Log("win!");
            positionTargetObj.enabled = false;
            congratsTxt.enabled = true;
            StartCoroutine(waiter());
            
        }
    }

    public void moveTargetToNextPosition(){
        if (currentPos != positions.Length ){
            currentPos++;
            positionTargetObj.moveTo(positions[currentPos].transform.position);
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
