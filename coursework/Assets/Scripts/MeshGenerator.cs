using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public static float yLevel = 100;

    public int xSize = 250;
    public int zSize = 250;

    private int xOffset = 0;
    private int zOffset = 0;

    public static float WIDTH = 1200f;
    private int scale;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        scale = (int) WIDTH / xSize;
        CreateShape();
        UpdateMesh();
    }

    private float calculateHeight(float x, float z){
        x/=scale;
        z/=scale;        
        x += xSize * getXOffset();
        z += zSize * getZOffset();

        float low = Mathf.PerlinNoise(x*0.0051f, z*0.005f) * 60f * scale;
        float med = Mathf.PerlinNoise(x*0.1f, z*0.11f) * 3f * scale;
        float high = Mathf.PerlinNoise(0.3f*x, 0.31f*z) *scale;
        return low + med + high; 
    }


    void CreateShape(){
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        float median_y = calculateHeight(xSize*scale /2f, zSize*scale /2f);
        if(MoonSceneManager.instance.isBaseChunk(this.getXOffset(), this.getZOffset())){
            for(int i = 0, z = 0; z <= zSize*scale; z+=scale){
                for(int x = 0; x <= xSize*scale; x+=scale){
                    float y =calculateHeight(x,z);
                    float dx = x - xSize*scale /2f;
                    float dz = z - zSize*scale /2f;
                    float dist = Mathf.Sqrt(dx*dx + dz*dz);
                    
                    if(dist < 300){
                        y = yLevel;
                    }
                    vertices[i] = new Vector3(x,y,z);
                    i++;
                }
            }
        }
        else{
            for(int i = 0, z = 0; z <= zSize*scale; z+=scale){
                for(int x = 0; x <= xSize*scale; x+=scale){
                    float y =calculateHeight(x,z);
                    vertices[i] = new Vector3(x,y,z);
                    i++;
                }
            }
        }

        triangles = new int[xSize*zSize*6];

        int vert = 0;
        int tris = 0;
        for(int z = 0; z < zSize; z+=1){
            for(int x = 0; x < xSize; x+=1){
                triangles[0+tris] = vert+ 0;
                triangles[1+tris] = vert+ xSize+1;
                triangles[2+tris] = vert+ 1;
                triangles[3+tris] = vert + 1;
                triangles[4+tris] = vert + xSize+1;
                triangles[5+tris] = vert + xSize+2;
                vert++;
                tris += 6;
            }
            vert ++;
        }
    }

    public void UpdateVerts(){

        transform.position = new Vector3(xOffset * WIDTH - WIDTH/2f, 0, zOffset * WIDTH - WIDTH/2f);
        CreateShape();
        UpdateMesh();
    }

    public void setXOffset(int off){
        xOffset = off;
    }
    
    public void setZOffset(int off){
        zOffset = off;
    }

    public int getXOffset(){
        return xOffset;
    }
    
    public int getZOffset(){
        return zOffset;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
