using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLine : MonoBehaviour
{
    private Material _mat;

    public float Add;
    public float valueChange;
    public float valueAngle;
    public float valueRadius;
    public float valueStrength;

    private bool bStart = false;

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
            _mat.SetFloat("_BendStrength", valueStrength);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // 右側
    public void SetRightLine()
    {
        /*bStart = true;
        Add = 0.5f;
        valueChange = 50.0f;
        valueAngle = 250.0f;
        valueRadius = 0.01f;*/
        bStart = true;
        Add = 0.7f;
        valueChange = 47.0f;
        valueAngle = 220.0f;
        valueRadius = 0.01f;
        valueStrength = 1.0f;
    }

    // 左側
    public void SetLeftLine()
    {
        bStart = true;
        Add = 0.7f;
        valueChange = 37.0f;
        valueAngle = 140.0f;
        valueRadius = 0.01f;
        valueStrength = 1.0f;
    }

    // マテリアルの設定
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }
}
