using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public PlayerController life;
    public GameManager gm;
    public GameObject ob1; // score pickup

    private int count;
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm.SetCountText();
    }

    void Update()
    {
        if(transform.position.y < 0)
        {
            //Dead!
            gm.amountAlive--;
            Object.Destroy(this.gameObject);
        }
    }

    private void OnMove(InputValue movementValue)
    {
        if (!gm.gameStarted)
        {
            gm.GameBegin();
        }
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void NewLife()
    {
        PlayerController newLife = Instantiate(life);
        newLife.gm = gm;
        newLife.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        gm.amountAlive++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
           // other.gameObject.SetActive(false);
            gm.score = gm.score + 1;
            gm.SetCountText();
            other.transform.position = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f));
            // GameObject newOb1 = Instantiate(ob1);
            // newOb1.SetActive(true);
            //newOb1.transform.position = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f));
        }
        else if (other.gameObject.CompareTag("extralife"))
        {
            other.gameObject.SetActive(false);
            NewLife();
        }
    }
}