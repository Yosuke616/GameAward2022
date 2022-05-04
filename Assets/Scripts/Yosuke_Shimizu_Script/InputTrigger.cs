using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    private bool g_bFirstFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //一回だけ押して連続で押したことにしないための関数
    public bool GetOneTimeDown()
    {
        //ボタンを押したときかつあるフラグがオフだったらいいかんじにする
        //g_bFirstFlg = false;
        if (Input.GetAxis("LTrigger") == 1)
        {
            g_bFirstFlg = true;
        }

        if (g_bFirstFlg)
        {
            if (Input.GetAxis("LTrigger") < 1)
            {
                Debug.Log("1っかいだよー");
                g_bFirstFlg = false;
                return true;
            }
        }

        return false;
    }
}
