using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTime : MonoBehaviour
{
    // 時間
    private int nTime;
    private int nCurrentTime;

    // α値
    private float fAlpha;
    private float fAdd;

    private bool bAdd = true;

    // キラ更新
    public void UpdateKira()
    {
        nTime--;
        if (bAdd)
        {
            fAlpha += fAdd;
            if (fAlpha >= 1.0f)
            {
                bAdd = false;
            }
        }
        else
        {
            fAlpha -= fAdd;
            if (fAlpha <= -1.0f)
            {
                bAdd = true;
            }
        }

        if (nTime <= 0)
        {
            nTime = nCurrentTime;
        }
        gameObject.GetComponent<Renderer>().material.SetFloat("_Value", fAlpha);
    }

    // タイマー設定
    public void SetTime(int time)
    {
        nTime = nCurrentTime = time;
    }

    // α値設定
    public void SetAlpha(float alpha, bool add)
    {
        fAlpha = alpha;
        bAdd = add;
    }

    public void SetAdd(float add)
    {
        fAdd = add;
    }
}
