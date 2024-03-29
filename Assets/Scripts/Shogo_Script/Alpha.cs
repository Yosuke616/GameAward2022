/*
作成日：2022/3/27 Shimizu Shogo
内容  ：切り取られた紙のα値を徐々に減らす
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    private Material _mat;

    public float valueAlpha;
    private bool bStart = false;

    // Start is called before the first frame update
    void Start()
    {
        //_mat.SetFloat("_Value", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bStart)
        {
            valueAlpha -= 0.01f;
            if (valueAlpha <= 0.0f)
            {
                valueAlpha = 0.0f;
                bStart = false;
            }
            _mat.SetFloat("_Value", valueAlpha);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // フェード開始
    public void SetAlpha()
    {
        //_mat = GetComponent<MeshRenderer>().material;
        bStart = true;
        valueAlpha = 1.0f;
        //_mat.SetFloat("_Value", valueAlpha);
    }

    // マテリアルの設定
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }
}
