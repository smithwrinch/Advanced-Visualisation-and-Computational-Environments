using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed;
    void Start()
    {
        transform.localScale += new Vector3(0, 0, Random.Range(5f, 17f));
    }
    void Update()
    {

        transform.position+= new Vector3(speed, 0f, 0f);
        if (transform.position.x > 35f)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
