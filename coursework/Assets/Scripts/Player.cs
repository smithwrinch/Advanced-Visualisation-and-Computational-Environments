using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

    private bool unparented;
    private bool moved;
    private bool rotating;
    private Vector3 temp; // world pos

    public GameObject startPlank;
    public GameObject door;
    public GameObject hinge;

    void Start(){
        unparented = false;
        moved = false;
        rotating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish" && !unparented)
        {
            // temp = transform.parent.position;
            Debug.Log("Collision");
            // Vector3 pos = transform.parent.transform.position
            //  controller.onBoat = false;
            OceanSceneManager.instance.setOnBoat(false);
            // playerCam.transform.position = transform.position;
            // transform.parent = null; // *should* not move, but you say...
            transform.SetParent(null);
            // transform.position = temp; // restore world position
            // transform.localPosition = temp;
            // Debug.Log(transform.position);
            // Vector3 position = new Vector3(-100f,0,0);
            // transform.position = position;
            unparented = true;
            other.gameObject.tag="Finished!";

        }   
        else if(other.gameObject.tag == "door" && unparented){
            rotating = true;
            other.gameObject.tag = "Finished!";
        }
        // Debug.Log(other.gameObject.tag);
    }

    private void FixedUpdate() {
        if(unparented){
            if(!moved){
                transform.position = startPlank.transform.position;
                moved = true;
            }
            transform.position = new Vector3(transform.position.x, getY(distanceXZ(transform.position, door.transform.position)), transform.position.z);
            if(rotating){
                // Debug.Log(door.transform.rotation.z);
                door.transform.RotateAround(hinge.transform.position, Vector3.up, 20 * Time.deltaTime);
                if(door.transform.rotation.z >= 0.3){
                    rotating = false;
                    OceanSceneManager.instance.endScene();
                }
            }
        }
        // Debug.Log(distanceXZ(transform.position, door.transform.position));
    }

    private float distanceXZ(Vector3 v, Vector3 u){
        return Mathf.Sqrt(Mathf.Pow(v.x - u.x, 2f) + Mathf.Pow(v.z - u.z, 2f));
    }


    private float getY(float dist){
        if(dist >= 24){
            return 6.5f;
        }
        else if(dist >= 13){
            return (-3.5f/11f) * dist + 14.13636f;
        }
        else return 10f;
    }
}
