using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float velocity = 1;
    public GameManager gameManager;
    private Rigidbody2D rb;

    private bool spacePressed = false;
    private bool up = true;
    private bool canJump = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump 
            if (!spacePressed)
            {
                spacePressed = true;
                gameManager.Begin();
            }
            switch (gameManager.state)
            {
                case 1:
                    if (canJump) {

                        rb.gravityScale = 3f;
                        rb.velocity = Vector2.up * velocity;
                        canJump = false;

                    }
                    if(!up)
                    {
                        transform.position = new Vector3(-6.154f, -2.79f, 0);
                    }
                    break;
                case 2:
                    up = !up;
                    rb.gravityScale = 0.0f;
                    if (up)
                    {
                        transform.position = new Vector3(-6.154f, -2.79f, 0);
                    }
                    else
                    {
                        transform.position = new Vector3(-6.154f, -4.424f, 0);
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "ground" && !gameManager.spawnPause)
        {
            switch (gameManager.state)
            {
                case 1:
                    if(collision.gameObject.tag != "ob_rurso")
                    {
                        Debug.Log(collision.gameObject.tag);
                        gameManager.GameOver();
                    }
                    break;
                case 2:
                    if (collision.gameObject.tag != "ob_jumo")
                    {
                        Debug.Log(collision.gameObject.tag);
                        gameManager.GameOver();
                    }
                    break;
            }
        }
        else if (collision.gameObject.tag == "ground")
        {
            canJump = true;
        }
    }
}
