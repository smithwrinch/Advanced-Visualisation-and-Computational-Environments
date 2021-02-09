using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool gameStarted;
    public int score;
    public int amountAlive;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject startText;
    float timeLeft; // time to restart
    // Start is called before the first frame update
    void Start()
    {

        gameOver = false;
        gameStarted = false;
        score = 0;
        amountAlive = 1;
        timeLeft = 5f;
        winTextObject.SetActive(false);
    }

    public void GameBegin()
    {
        gameStarted = true;
        startText.SetActive(false);
        SetCountText();
    }



    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            SetCountText();

        }
    }
    public void SetCountText()
    {
        countText.text = score.ToString();
        if (amountAlive <= 0 && !gameOver)
        {
            winTextObject.SetActive(true);
            gameOver = true;
           
        }
    }
}
