using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Move : MonoBehaviour
{

    //親オブジェクトの情報を格納する変数
    GameObject ParentObj_Move;
    //親の座標を格納する為の変数
    Vector3 PlayerPos_Move;


    // 小さくするフラグ
    private bool small, big;
    public void SmallStart()
    {
        small = true;
        frameCount = 30;
    }
    public void BigStart()
    {
        big = true;
        frameCount = 30;
    }
    Vector3 normalScale;
    int frameCount;


    void Start()
    {
        //このオブジェクトの親の情報を取得する
        ParentObj_Move = this.transform.parent.gameObject;
        //親オブジェクトの座標を保存する
        PlayerPos_Move = ParentObj_Move.transform.position;
        //プレイヤーの少し横に移動させる
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        big = small = false;
        normalScale = new Vector3(0.8f, 0.8f, 0.8f);
        frameCount = 30;
    }

    void Update()
    {
        GameObject ParentObj = transform.parent.gameObject;

        //親オブジェクトの座標を更新する
        ParentObj_Move = this.transform.parent.gameObject;

        //親オブジェクトの座標を保存する
        PlayerPos_Move = ParentObj_Move.transform.position;

        //プレイヤーの少し横に移動させる
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        if (small)
        {
            // 0.5秒でスケールを0にする
            frameCount--;

            Vector3 scale = normalScale * (frameCount / 30.0f);
            this.transform.localScale = scale;

            if (frameCount <= 0)
            {
                small = false;
                frameCount = 30;
            }
        }
        else if (big)
        {
            // 0.5秒でスケールを0にする
            frameCount--;

            Vector3 scale = normalScale * (Mathf.Abs((float)(30 - frameCount)) / 30.0f);

            this.transform.localScale = scale;

            if (frameCount <= 0)
            {
                big = false;
                frameCount = 30;
            }
        }

    }
}
