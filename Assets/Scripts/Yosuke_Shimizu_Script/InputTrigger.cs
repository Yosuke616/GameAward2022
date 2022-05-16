using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    // 前フレームに押されていたかのフラグ
    static private bool pressed = false;
    // トリガー機能が呼ばれた回数
    static private int numberCalled = 0;
    static private bool saveGetOneTimeDownFlag = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        numberCalled = 0;
        saveGetOneTimeDownFlag = false;
    }

    //一回だけ押して連続で押したことにしないための関数
    public bool GetOneTimeDown2()
    {
        // 前フレームに押されていなかったら
        //if (!g_bFirstFlg  && nCnt <= 0)
        //{
        //    // 現在押されていたら
        //    if (Input.GetAxis("LTrigger") == 1)
        //    {
        //        Debug.Log("1っかいだよー");
        //        g_bFirstFlg = false;
        //        nCnt = 15;
        //        return true;
        //    }
        //}

        // 押されているフラグの更新
        //if (Input.GetAxis("LTrigger") == 1)
        //{
        //    g_bFirstFlg = true;
        //}
        //else
        //{
        //    g_bFirstFlg = false;

        //}


        return false;
    }

    public bool GetOneTimeDown()
    {
        if (numberCalled != 0) return saveGetOneTimeDownFlag;
        numberCalled++;

        bool ret = false;

        float tri = Input.GetAxis("LTrigger");
        if (tri > 0)
        {
            //Debug.Log("L trigger:" + tri);

            // 前フレームまで押されていなかったかどうか
            if (pressed == false) ret = true;

            pressed = true;
        }
        else if (tri < 0)
        {
            //Debug.Log("R trigger:" + tri);
        }
        else
        {
            //Debug.Log("  trigger:none");

            pressed = false;
        }

        saveGetOneTimeDownFlag = ret;
        if (ret) Debug.LogWarning("成功");
       
        return ret;
    }
}
