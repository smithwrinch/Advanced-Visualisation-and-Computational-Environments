using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startCanvas;
    public Image img;
    public bool gameOver = false;
    public bool gameStarted = false;
    public bool spawnPause = false;

    public int state = 1; // state of type of obstacle
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        startCanvas.SetActive(true);
    }

    public void Begin()
    {
        gameStarted = true;
        startCanvas.SetActive(false);
    }

    // Update is called once per frame
    public void GameOver()
    {
        //startCanvas.SetActive(false);
        gameOver = true;
        Time.timeScale = 0;
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void FadeOutStart()
    {
        StartCoroutine(FadeImage(true));

    }

    public void ChangeState()
    {
        state++;
        if(state > 2)
        {
            state = 1;
        }

    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Replay();
            }
        }
        else if(Score.score % 1100 == 0)
        {
            if(Random.Range(0, 3) == 0)
            {
                ChangeState();
            }
        }
    }
    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
