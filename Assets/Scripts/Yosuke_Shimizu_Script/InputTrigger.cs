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


    public bool GetOneTimeDown()
    {
        // 1フレーム以内で既に呼ばれて結果が決まっていたらそれを返す
        if (numberCalled != 0) return saveGetOneTimeDownFlag;

        // この関数が呼ばれた回数を記憶
        numberCalled++;

        bool ret = false;

        float tri = Input.GetAxis("LTrigger");
        // Lトリガー
        if (tri > 0.95f)
        {
            // 前フレームまで押されていなかったかどうか
            if (pressed == false) ret = true;

            pressed = true;
            Debug.Log("L trigger:" + tri);
        }
        // Rトリガー
        else if (tri < 0) {}
        // 入力されていない状態
        else
        {
            pressed = false;
        }

        saveGetOneTimeDownFlag = ret;
        if (ret)
        {
            //SoundManager.Instance.PlaySeByName("SE_MenuOperation");
            Debug.LogWarning("成功");
        }
       
        return ret;
    }
}
