using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel : MonoBehaviour
{
    [SerializeField] Button igniteBtn;
    [SerializeField] Button swingBtn;
    [SerializeField] Button safetyBtn;
    [SerializeField] Color32 onColor, offColor, blackOffColor;

    ExcavatorMovement em;
    bool isEngineIgnited = false;
    bool isSwingLocked = true;
    bool isSafetyLockLocked = true;

    AudioManagerScript audioMan;
    IdleAudioScript idleAudio;

    private void Start() {
        em = FindObjectOfType<ExcavatorMovement>();
        audioMan = FindObjectOfType<AudioManagerScript>();
        idleAudio = FindObjectOfType<IdleAudioScript>();

    }

    private void Update() {
        
        
        
    }

    public void changeIgniteStatus(){
        isEngineIgnited = !isEngineIgnited;
        if(isEngineIgnited){
            igniteBtn.image.color = onColor;
            audioMan.playIgniteEngineSfx();
            idleAudio.playIdleEngineSFX();
        }else{
            igniteBtn.image.color = offColor;
            idleAudio.stopIdleEngineSFX();
        }
        
        em.changeStatus("ignite", isEngineIgnited);
        Debug.Log("ignite is: " + isEngineIgnited);
    }

    public void changeSwingLockStatus(){
        isSwingLocked = !isSwingLocked;
        if(isSwingLocked){
            swingBtn.image.color = offColor;
        }else{
            swingBtn.image.color = blackOffColor;
        }
        
        em.changeStatus("swingLock", isSwingLocked);
        Debug.Log("safetyLock is: " + isSwingLocked);
    }

    public void changeSafetyLockStatus(){
        isSafetyLockLocked = !isSafetyLockLocked;
        if(isSafetyLockLocked){
            safetyBtn.image.color = offColor;
        }else{
            safetyBtn.image.color = blackOffColor;
        }
        
        em.changeStatus("safetyLock", isSafetyLockLocked);
        Debug.Log("safetyLock is: " + isSafetyLockLocked);
    }
    
}
