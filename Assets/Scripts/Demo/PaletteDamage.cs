using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteDamage : MonoBehaviour
{
    [SerializeField] Text guiText;
    WarningSoundScript wSound;

    private void Start() {
        //wSound = FindObjectOfType<WarningSoundScript>();
    }
    /*
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag + "palette dmg!");
        //wSound.playWarningSoundSFX();
        guiText.GetComponent<Text>().enabled = true;
        guiText.text = "Palette Damaged!";
        //StartCoroutine(ShowMessage("Palette Damaged!", 2));
      
    }
    */

    IEnumerator ShowMessage (string message, float delay) {
     guiText.text = message;
     guiText.enabled = true;
     yield return new WaitForSeconds(delay);
     guiText.enabled = false;
    }
    
}
