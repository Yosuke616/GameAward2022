/*
作成日：2022/5/07 Shimizu Shogo
内容  ：切り取られた紙を変形させる
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPaper : MonoBehaviour
{
    private Material _mat;          // マテリアル

    public float Add;               // 変化させる値
    public float valueChange;       // Shaderに渡す値
    public float valueAngle;        // 角度(左か右か)
    public float valueRadius;       // 固定値

    private bool bStart = false;    // 動作フラグ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bStart)
        {
            valueChange += Add;
            if (valueChange >= 300.0f)
            {
                valueChange = 300.0f;
                bStart = false;
            }
            else if (valueChange <= -300.0f)
            {
                valueChange = -300.0f;
                bStart = false;
            }
            _mat.SetFloat("_BendPivot", valueChange);
            _mat.SetFloat("_BendAngle", valueAngle);
            _mat.SetFloat("_BendRadius", valueRadius);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // 右側にめくる
    public void SetRight()
    {
        bStart = true;
        Add = 0.7f;
        valueChange = 47.0f;
        valueAngle = 220.0f;
        valueRadius = 0.01f;
        Debug.Log("右だよ");
    }

    // 左側にめくる
    public void SetLeft()
    {
        bStart = true;
        Add = 0.7f;
        valueChange = 37.0f;
        valueAngle = 140.0f;
        valueRadius = 0.01f;
        Debug.Log("左だよ");
    }

    // マテリアルの設定
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }
}
