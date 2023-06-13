using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialProgress : MonoBehaviour
{
    [SerializeField] Text progressText;
    [SerializeField] string templateStrForProgressTxt;
    int totalTarget;
    int currentTarget = 0;
    bool isLevelCompleted;

    public void initTotalTarget(int total){
        totalTarget = total;
        progressText.text = templateStrForProgressTxt + currentTarget + "/" + totalTarget;

    }

    public void targetCompleted(){
        if(!isLevelCompleted){
            currentTarget++;
            progressText.text = templateStrForProgressTxt + currentTarget + "/" + totalTarget;
            if(currentTarget == totalTarget && totalTarget != null){
                levelCompleted();
                isLevelCompleted = true;
            }
        }
        
    }

    void levelCompleted(){
        //blabla
    }
}
