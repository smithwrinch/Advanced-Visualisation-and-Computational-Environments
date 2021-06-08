using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoonSceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject prefab;
    public GameObject moonBase;
    public Light light;
    public Text OxygenText;
    public Text warningText;
    public Text warningText2;
    public Text distance;

    private MeshGenerator[] loadedChunks;
    private int baseX;
    private int baseZ;
    public static MoonSceneManager instance;
    private float oxygenLeft;
    private float startingOxygen;
    private bool endLevel;
    private float timeRemaining;    
    private int[] loadedChunkIndices;
    void Start()
    {
        Cursor.visible = false;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Moon scene manager instance already exists, destroying object");
            Destroy(this);
        }
        endLevel = false;
        loadedChunks = new MeshGenerator[25];
        if(GameManager.instance.getMoon() == 0){
            initFirst();
        }
        else if (GameManager.instance.getMoon() == 1){
            initSecond();
        }
        else if (GameManager.instance.getMoon() == 2){
            initThird();
        }
        else{
            GameManager.instance.setMoon(0);
            initFirst();
        }
        // initFirst();
        
        oxygenLeft = startingOxygen;
        timeRemaining = 0f;
        spawnBase();
        initChunks();
        warningText.text = "";
        warningText2.text = "";
    }

    private void initFirst(){
        baseX = 1;
        baseZ = 1;
        startingOxygen = 100000000000000f;
    }
    
    private void initSecond(){
        baseX = -6;
        baseZ = 1;
        startingOxygen = 185f;
    }
    
    private void initThird(){
        baseX = 18;
        baseZ = -5;
        startingOxygen = 365f;
    }
    private void spawnBase(){
        float x = baseX*MeshGenerator.WIDTH;
        float z = baseZ*MeshGenerator.WIDTH;
        moonBase.transform.position = new Vector3(x, MeshGenerator.yLevel, z);
    }

    public bool isBaseChunk(int x, int z){
        return baseX == x && baseZ == z;
    }

    private void initChunks(){

        Vector3[] positions = new Vector3[25];

        for(int xx =-3; xx < 2; xx++){
            for(int zz =-3; zz < 2; zz++){
              float x = xx*MeshGenerator.WIDTH - MeshGenerator.WIDTH/2f;
              float z = zz*MeshGenerator.WIDTH - MeshGenerator.WIDTH/2f;
              GameObject go = Instantiate(prefab, new Vector3(x,0,z), Quaternion.identity);
              MeshGenerator scr = go.GetComponent<MeshGenerator>();
              scr.setXOffset(xx);
              scr.setZOffset(zz);
              loadedChunks[(xx+3)*5 + zz+3] = scr;
            }            
        }

    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        for(int i =0; i < 25; i++){
            Vector3 target = loadedChunks[i].transform.position;
            target.x += MeshGenerator.WIDTH - MeshGenerator.WIDTH/2f;
            target.y += MeshGenerator.WIDTH - MeshGenerator.WIDTH/2f;
            bool needsUpdate = false;
            float thresh = 3f * MeshGenerator.WIDTH;
            if(target.x - playerPos.x > thresh ){
                loadedChunks[i].setXOffset(loadedChunks[i].getXOffset() - 5);
                needsUpdate = true;
            }
            
            else if(target.x - playerPos.x < -thresh ){
                loadedChunks[i].setXOffset(loadedChunks[i].getXOffset() + 5);
                needsUpdate = true;
            }

            if(target.z - playerPos.z > thresh ){
                loadedChunks[i].setZOffset(loadedChunks[i].getZOffset() - 5);
                needsUpdate = true;
            }
            
            else if(target.z - playerPos.z < -thresh ){
                loadedChunks[i].setZOffset(loadedChunks[i].getZOffset() + 5);
                needsUpdate = true;
            }
            
            if(needsUpdate){
                loadedChunks[i].UpdateVerts();
            }   
        }
        if(timeRemaining > 0){
                timeRemaining -= Time.deltaTime;
            }
        else if(endLevel && timeRemaining <= 0){
            GameManager.instance.setMoon(GameManager.instance.getMoon()+1);
            if(GameManager.instance.getGameMode() == 0){
                GameManager.instance.setScene(1);
            }
            else{
                Debug.Log("restrating");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }
    }

    public bool warning(){
        return GameManager.instance.getMoon() == 2 && oxygenLeft/ startingOxygen < 0.5f;
    }

    private void FixedUpdate() {
        if(!endLevel && GameManager.instance.getMoon() != 0){
            Vector3 baseVec = new Vector3(baseX*MeshGenerator.WIDTH, 0, baseZ*MeshGenerator.WIDTH);
            float dist = distanceXZ(player.transform.position, baseVec);
            distance.text = Mathf.Round(dist/10) +"m";

            oxygenLeft -= Time.deltaTime;
            OxygenText.text = "OXYGEN LEVELS: "+ ((Mathf.Round(oxygenLeft * 100)) / 100)+"s remaining";
            if(oxygenLeft/ startingOxygen < 0.5f){
                if(oxygenLeft/ startingOxygen < 0.25f){
                    light.intensity = 40*Mathf.Sin(10*oxygenLeft);
                    warningText2.text = "!!!!WARNING!!!!";
                    distance.text = "SOS";
                }else{
                    light.intensity = 40*Mathf.Sin(5*oxygenLeft);
                    warningText.text = "!!!!WARNING!!!!";
                }
            }
            if(oxygenLeft < 0){
                player.transform.position = new Vector3(0, 700, 9.7f);
                player.GetComponent<Rigidbody>().velocity=Vector3.zero;
                oxygenLeft = startingOxygen;
                warningText.text = "";
                warningText2.text = "";
            }
        }

    }

    private float distanceXZ(Vector3 v, Vector3 u){
        return Mathf.Sqrt(Mathf.Pow(v.x - u.x, 2f) + Mathf.Pow(v.z - u.z, 2f));
    }
    
    public void endScene(){
        timeRemaining = 5f;
        endLevel = true;
    }
    public bool levelEnd(){
        return endLevel;
    }
}
