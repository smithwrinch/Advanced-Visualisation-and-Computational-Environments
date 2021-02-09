using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public float maxTime = 1; //time for each spawn
    public float timer = 0;
    public GameObject ob1;
    public GameObject ob2;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
      //  GameObject newOb1 = Instantiate(ob1);
        //newOb1.transform.position = transform.position + new Vector3(Random.Range(10, 15), 0 ,0);

    }

    // Update is called once per frame
    void Update()
    {
        if(timer > maxTime && gm.gameStarted)
        {
            switch (gm.state)
            {
                case 1:
                    GameObject newOb1 = Instantiate(ob1);
                    newOb1.transform.position = transform.position + new Vector3(Random.Range(10, 15), 0, 0);
                    maxTime = 1.5f;
                    Destroy(newOb1, 15);
                    break;
                case 2:
                    maxTime = 0.9f;
                    GameObject newOb2 = Instantiate(ob2);
                    newOb2.transform.position = transform.position + new Vector3(15, 0, 0);
                    Destroy(newOb2, 15);
                    break;

            }
            timer = 0;
        }

        timer += Time.deltaTime;
        
    }
}
