using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class rpmHeatFuel : MonoBehaviour
{
    [SerializeField] Slider heatSlider;
    [SerializeField] Slider fuelSlider;
    [SerializeField] Slider powerSlider;
    [SerializeField] Text guiText;

    WarningSoundScript wSound;

    float rpm = 0f;
    float fuel = 1000f;
    float power = 1;
    bool firstCallHeat = true, firstCallFuel = true;

    private void Start() {
        wSound = FindObjectOfType<WarningSoundScript>();
    }
    public void changeHeat(float val){
        if(rpm >= 0 ){
            rpm += val * Time.deltaTime;
        }else{
            rpm = 0;
        }
        heatSlider.value = rpm/1000;
        if(firstCallHeat && rpm > 800f){
            firstCallHeat=false;
            guiText.text = "Engine Overheat!";
            guiText.enabled = true;
            wSound.playWarningSoundSFX();
        }
        if(rpm < 400){
            guiText.enabled = false;
            firstCallHeat = true;
        }
    }


    public void changeFuel(float val){
        fuel += val * Time.deltaTime;
        fuelSlider.value = fuel/1000f;
        Debug.Log("fuel: " + fuel);
        if(fuel < 100f){
            firstCallFuel=false;
            guiText.text = "Fuel Low!";
            guiText.enabled = true;
            wSound.playWarningSoundSFX();
        }
        if(fuel <= 1){
            guiText.text = "GAME OVER";
            guiText.enabled = true;
            wSound.playWarningSoundSFX();
            StartCoroutine(waiter());
        }
    }

    public void changePower(float val){
        power = val;
        powerSlider.value = power;
    }

    IEnumerator waiter(){ 
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }

    }
