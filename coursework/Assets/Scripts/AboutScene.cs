using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutScene : MonoBehaviour
{
    public void toMenu(){
        GameManager.instance.setScene(0);
    } 
}
