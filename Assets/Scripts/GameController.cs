using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] Text scoreText;
    int score = 0;

    void Start()
    {
        scoreText.text = "Score: 0";
    }

    public void increaseScore(){
        score++;
        scoreText.text = "Score: " + score;
    }
}
