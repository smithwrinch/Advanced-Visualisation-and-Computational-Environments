using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int currentScene; // 0 = ocean; 1 = moon
    private int gameMode; // 0 = both, 1 = depression, 2 = anxiety
    private int oceanLevel;
    private int moonLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("instance already exists, destroying object");
            Destroy(this);
        }
        gameMode = 1;
        oceanLevel = 0;
        moonLevel = 0;
        // currentScene = 0;
        // SceneManager.LoadScene(currentScene);
    }

    // Update is called once per frame
    void Update()
    {
        // Scene scene = SceneManager.GetActiveScene();
        // Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
    }

    public void setScene(int scene){
        Debug.Log("Loading Scene: " + scene);
        currentScene = scene;
        SceneManager.LoadScene(currentScene);
    }

    public void setGameMode(int gm){
        gameMode = gm;
    }
    public int getGameMode(){
        return gameMode;
    }
    
    public void setOcean(int l){
        oceanLevel = l;
    }
    public int getOcean(){
        return oceanLevel;
    }

    
    public void setMoon(int l){
        Debug.Log("Setting moon: " + l);
        moonLevel = l;
    }
    public int getMoon(){
        return moonLevel;
    }
    
}
