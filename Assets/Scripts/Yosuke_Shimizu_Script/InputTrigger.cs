using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    private bool g_bFirstFlg = false;

    //時間で連続で押せるかどうかを制御する
    private int nCnt;

    // Start is called before the first frame update
    void Start()
    {
        nCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        nCnt--;
        if (nCnt < -1) {
            nCnt = -1;
        }
    }

    //一回だけ押して連続で押したことにしないための関数
    public bool GetOneTimeDown()
    {
        // 前フレームに押されていなかったら
        if (!g_bFirstFlg  && nCnt <= 0)
        {
            // 現在押されていたら
            if (Input.GetAxis("LTrigger") == 1)
            {
                Debug.Log("1っかいだよー");
                g_bFirstFlg = false;
                nCnt = 15;
                return true;
            }
        }

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
}
