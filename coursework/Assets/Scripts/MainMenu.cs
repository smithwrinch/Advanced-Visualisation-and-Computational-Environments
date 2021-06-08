using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void playGame(){
        GameManager.instance.setGameMode(0); 
        GameManager.instance.setScene(1); 
    }
    
    public void playDepression(){
        Debug.Log("depressin");
        GameManager.instance.setGameMode(1); 
        Debug.Log(GameManager.instance.getGameMode());
        GameManager.instance.setScene(1); 
        Debug.Log(GameManager.instance.getGameMode());

    }
    public void playAnxiety(){
        GameManager.instance.setGameMode(2); 
        GameManager.instance.setScene(2); 

    }
     public void toAbout(){
        GameManager.instance.setScene(3);
    } 
}
