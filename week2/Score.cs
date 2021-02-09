using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public GameManager gm;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        GetComponent<UnityEngine.UI.Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.gameOver && gm.gameStarted)
        {
            score++;
            GetComponent<UnityEngine.UI.Text>().text = (score / 100).ToString();
        }
    }
}
