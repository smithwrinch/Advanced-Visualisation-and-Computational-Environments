using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moonCharacterController : MonoBehaviour
{
    public GameObject door;
    public GameObject arrow;
    private Rigidbody rb;
    Vector2 rotation = Vector2.zero;
    public float lookSpeed = 3f;
    public float jumpHeight = 4f;
    public float speed = 5f;
    private bool canJump;
    private bool opening;
    private float doorY;
    private bool hasCollided;
    private float time_elapsed;
    void Start()
    {
        time_elapsed = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0f, -8f, 0f); 
        canJump = false;
        opening = false;
        hasCollided = false;
        doorY = door.transform.position.y;
    }

    private void updateArrow(){
        float newY;
        if(MoonSceneManager.instance.warning()){
            time_elapsed += Time.deltaTime;
            newY = 180 * Mathf.Sin(time_elapsed * Mathf.Cos(time_elapsed/100f) + Mathf.Sin(time_elapsed));
       
        }
        else{
            //sets arrow towards door
            Quaternion lol = Quaternion.LookRotation(door.transform.position - transform.position);
            newY = lol.eulerAngles.y - transform.eulerAngles.y + 180;
        }
            arrow.transform.eulerAngles = new Vector3(
            arrow.transform.eulerAngles.x,
            newY,
            arrow.transform.eulerAngles.z
        );

    }

    // Update is called once per frame
    void Update()
    {
            updateArrow();
            Vector3 direction = new Vector3();
            if(canJump){

                if (Input.GetKey(KeyCode.W))
                {
                    direction += Camera.main.transform.forward;
                    
                }
                if (Input.GetKey(KeyCode.S))
                {
                    direction -= Camera.main.transform.forward;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    direction -= Camera.main.transform.right;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    direction += Camera.main.transform.right;
                }
                if (Input.GetKey(KeyCode.Space) && canJump)
                {
                    rb.AddForce(new Vector3(0f, jumpHeight, 0f), ForceMode.Impulse);
                    canJump = false;
                }
                if((direction.x != 0 || direction.y !=0) && canJump){
                    direction *= speed;
                    direction.y = jumpHeight/5f;

                    rb.AddForce(direction);

                }
                
                }
            if(opening){
                door.transform.position -= new Vector3(0.007f, .01f, 0);
                if(Mathf.Abs(doorY - door.transform.position.y) > 10f){
                    opening = false;
                    MoonSceneManager.instance.endScene();
            }
        }
        rotation.y += Input.GetAxis ("Mouse X");
        rotation.x += -Input.GetAxis ("Mouse Y");
        transform.eulerAngles = (Vector2)rotation * lookSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "door" && !hasCollided){
            hasCollided = true;
            opening = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "ground"){
            canJump = true;
            Debug.Log("yay");
        }
    }
       void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "ground"){
            canJump = false;
            Debug.Log("nay");
        }
    }
    
}
