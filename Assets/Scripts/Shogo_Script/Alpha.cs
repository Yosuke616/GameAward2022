/*
�쐬���F2022/3/27 Shimizu Shogo
���e  �F�؂���ꂽ���̃��l�����X�Ɍ��炷
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    private Material _mat;

    private float valueAlpha;
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
            valueAlpha -= 0.01f;
            if (valueAlpha <= 0.0f)
            {
                valueAlpha = 0.0f;
                bStart = false;
            }
            _mat.SetFloat("_Value", valueAlpha);
        }
    }

    public void SetAlpha(int num)
    {
        switch (num)
        {
            case 1:
                _mat = (Material)Resources.Load("Effects/Alpha");
                break;
            case 2:
                _mat = (Material)Resources.Load("Effects/Alpha_002");
                break;
            case 3:
                _mat = (Material)Resources.Load("Effects/Alpha_003");
                break;
        }
        bStart = true;
        valueAlpha = 1.0f;
    }
}
