/*
�쐬���F2022/5/07 Shimizu Shogo
���e  �F�؂���ꂽ����ό`������
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPaper : MonoBehaviour
{
    private Material _mat;          // �}�e���A��

    public float Add;               // �ω�������l
    public float valueChange;       // Shader�ɓn���l
    public float valueAngle;        // �p�x(�����E��)
    public float valueRadius;       // �Œ�l

    private bool bStart = false;    // ����t���O

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
            if (valueChange >= 220.0f)
            {
                valueChange = 110.0f;
                bStart = false;
            }
            else if (valueChange <= -220.0f)
            {
                valueChange = -220.0f;
                bStart = false;
            }
            _mat.SetFloat("_BendPivot", valueChange);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // �E���ɂ߂���
    public void SetRight()
    {
        bStart = true;
        Add = 1.0f;
        valueChange = 47.0f;
        valueAngle = 250.0f;
        valueRadius = 0.01f;
        _mat.SetFloat("_BendAngle", valueAngle);
        _mat.SetFloat("_BendRadius", valueRadius);
        Debug.Log("�E����");
    }

    // �����ɂ߂���
    public void SetLeft()
    {
        bStart = true;
        Add = 1.0f;
        valueChange = 34.0f;
        valueAngle = 140.0f;
        valueRadius = 0.01f;
        _mat.SetFloat("_BendAngle", valueAngle);
        _mat.SetFloat("_BendRadius", valueRadius);
        Debug.Log("������");
    }

    // �}�e���A���̐ݒ�
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }
}
