using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wireframe : MonoBehaviour
{
    public Material material;
    public bool simple;
    void Start()
    {
        
        

        

        if (simple) {
            MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh.SetIndices(mf.mesh.GetIndices(0),MeshTopology.LineStrip,0);
        } else {
            MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = material;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
