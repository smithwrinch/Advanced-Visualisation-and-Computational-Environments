using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OceanSceneManager : MonoBehaviour
{

    public static OceanSceneManager instance;

    public Camera mainCamera;
    public Camera loadCamera;
    public GameObject player;
    public GameObject rotateBeam;
    public GameObject beam;
    public GameObject ocean;
    public GameObject sun;
    public GameObject lighthouse;
    public WavesSettings wavesSettings1;
    public WavesSettings wavesSettings2;
    public WavesSettings wavesSettings3;
    public WavesSettings wavesSettings4;
    private bool endLevel;
    bool isLoaded = false;
    private float timeRemaining = 2;
    private bool onBoat;
    private WavesGenerator wg;  
    // Start is called before the first frame update
    void Start()
    {
        //mainCamera.enabled = false;
        //loadCamera.enabled = true;
        onBoat = true;
        endLevel = false;
        mainCamera.depthTextureMode = DepthTextureMode.Depth;
        loadCamera.depthTextureMode = DepthTextureMode.Depth;

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("instance already exists, destroying object");
            Destroy(this);
        }
        wg = ocean.GetComponent<WavesGenerator>();
        // wg.size = 100;
        // wg.Begin();
        sun.transform.eulerAngles = new Vector3(9.9f, Random.Range(0, 360), 0f);
        if(GameManager.instance.getOcean() == 0){
            initFirst();
        }
        else if (GameManager.instance.getOcean() == 1){
            initSecond();
        }
        else if (GameManager.instance.getOcean() == 2){
            initThird();
        }
        else{
            GameManager.instance.setOcean(0);
            initFirst();
        }
    }
    private void initFirst(){
        Debug.Log("ae");
        lighthouse.transform.position = new Vector3(200, -2.4f, 300);
        wg.wavesSettings = wavesSettings1;
    }
    
    private void initSecond(){
        Debug.Log("2");
        lighthouse.transform.position = new Vector3(-550, -2.4f, -400);
        wg.wavesSettings = wavesSettings2;
        
    }
    
    private void initThird(){
        lighthouse.transform.position = new Vector3(900, -2.4f, -550);   
        wg.wavesSettings = wavesSettings3;    
    }
    void Update()
    {
        beam.transform.RotateAround(rotateBeam.transform.position, Vector3.up, 5f * Time.deltaTime);
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if(!isLoaded)
        {
            isLoaded = true;
            loadCamera.enabled = false;
            mainCamera.enabled = true;
        }
        else if(endLevel){
            GameManager.instance.setOcean(GameManager.instance.getOcean()+1);
            if(GameManager.instance.getGameMode() == 0){
                GameManager.instance.setScene(2);
            }
            else{
                Debug.Log("restrating");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

    }

    public void setOnBoat(bool value)
    {
        onBoat = value;
    }
    public bool getOnBoat()
    {
        return onBoat;
    }

    public void endScene(){
        timeRemaining = 5f;
        endLevel = true;
    }
    
    public bool levelEnd(){
        return endLevel;
    }
}
