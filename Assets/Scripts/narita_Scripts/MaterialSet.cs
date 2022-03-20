using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MaterialSet : MonoBehaviour
{
    public Material mat;
    //private Material[] mats;

    void Start()
    {
        //GetComponent<MeshRenderer>().material = mat;
    }

    void Update()
    {
        
    }

    public Material GetMaterial()
    {
        return mat;
    }
}
