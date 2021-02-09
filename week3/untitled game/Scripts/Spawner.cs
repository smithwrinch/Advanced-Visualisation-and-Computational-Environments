using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float maxTime = 1; //time for each spawn
    public float timer = 0;
    public GameObject ob1; // this is the pickup score entities
    public GameObject ob2; // this is the pickup duplicate entities
    public Obstacle ob; // this is the pickup duplicate entities
    public GameManager gm;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > maxTime && gm.gameStarted)
        {
            if(!gm.gameOver)
            {
                //if(count % 2 == 0)
                //{
                GameObject newOb2 = Instantiate(ob2);
                newOb2.transform.position = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
                 newOb2.SetActive(true);
                //}

               

                Obstacle newOb = Instantiate(ob);
                ob.speed = 0.02f ;
                newOb.transform.position = new Vector3(-35f, 0.5f, Random.Range(-10f, 10f));

                maxTime = 3;
            }
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
