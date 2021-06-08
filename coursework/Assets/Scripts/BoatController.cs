using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatController : MonoBehaviour
{

    public float speed = 0.1f;
    public float torqueSpeed = 100f;
    public Rigidbody rigidBody;
    public GameObject paddleRight;
    public GameObject paddleLeft;

    float t_r = 0;
    float t_l = 0;

    private void Start() {
        reset();
    }


    private void FixedUpdate()
    {
        if (OceanSceneManager.instance.getOnBoat())
        {

            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.AddRelativeForce(new Vector3(0f, 0f, speed));
                rotateRightPaddle(-100f);
                rotateLeftPaddle(100f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rigidBody.AddRelativeForce(new Vector3(0f, 0f, -speed), ForceMode.Acceleration);
                rotateRightPaddle(100f);
                rotateLeftPaddle(-100f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 tempVect = new Vector3(0, -torqueSpeed, 0);
                rigidBody.AddTorque(tempVect);
                rotateRightPaddle(100f);
                rotateLeftPaddle(100f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 tempVect = new Vector3(0, torqueSpeed, 0);
                rigidBody.AddTorque(tempVect);
                rotateRightPaddle(-100f);
                rotateLeftPaddle(-100f);
            }

            if (transform.up.y < 0f)
            {
                //player upside down
                // reset();
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void rotateRightPaddle(float factor){
        t_r -= (Mathf.Abs(factor)/ factor)*Time.deltaTime;
        float paddledir = Mathf.Sin(t_r);
        paddledir *= Mathf.Abs(factor);
        // paddleRight.transform.localRotation = Quaternion.Euler(new Vector3(paddledir, -180f, 30f + paddledir / 4f));
        
        float x = Mathf.Abs(Mathf.Sin(t_r));
        float y = Mathf.Cos(t_r);
        float z = -Mathf.Sin(t_r);

        paddleRight.transform.localRotation = Quaternion.Euler(new Vector3(x*50, y*50 - 180, z*30));
    }

    private void rotateLeftPaddle(float factor ){
        t_l -= (Mathf.Abs(factor)/ factor)*Time.deltaTime;
        float paddledir = Mathf.Sin(t_l);
        paddledir *= Mathf.Abs(factor);
        // paddleLeft.transform.localRotation = Quaternion.Euler(new Vector3(paddledir, 0f, 30f + paddledir / 4f));;

        float x = Mathf.Abs(Mathf.Sin(t_l));
        float y = Mathf.Cos(t_l);
        float z = -Mathf.Sin(t_l);

        paddleLeft.transform.localRotation = Quaternion.Euler(new Vector3(x*50, y*50, z*30));
    }


    private void reset()
    {
        rigidBody.transform.position = new Vector3(0f, 0.5f, 0f);
        rigidBody.transform.eulerAngles = new Vector3(0f, Random.Range(0, 360), 0f);
    }

}