using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2 : MonoBehaviour
{
    //2.9 4.29
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            transform.position += new Vector3(Random.Range(10, 15), 0, 0);
        }
        else
        {
            transform.position += new Vector3(Random.Range(10, 15), -1.39f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
