using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRLCur : MonoBehaviour
{
    private Vector3 m_pos;

    private Vector3 SendPos;

    // このオブジェクトのX,Y座標の移動限界
    private Vector2 limit;

    //ボタンを押したときのフラグ
    private bool g_bFirstFlg;

    void Start()
    {
        //初期化でオブジェクトの場所を決める
        m_pos = this.transform.position = new Vector3(0.0f,0.5f,0.0f);
        SendPos = new Vector3(0.0f,0.0f,0.0f);
        g_bFirstFlg = false;

        // 移動限界を決めておく
        limit.x = CreateTriangle.paperSizeX + 0.1f;
        limit.y = CreateTriangle.paperSizeY + 0.1f;
    }

    void Update()
    {
        // MainCameraがenableではない場合は何もしない
        if (Camera.main == null) { return; }

        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            // 接続されているコントローラの名前を調べる
            var controllerNames = Input.GetJoystickNames();
            // 一台もコントローラが接続されていなければマウスで
            if (controllerNames.Length == 0)
            {
                //--- マウス
                // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
                // マウス座標のzの値を0にする
                Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                // マウス座標をワールド座標に変換する
                m_pos = Camera.main.ScreenToWorldPoint(cursor);
            }
            else
            {
                //--- ゲームパッド

                //右方向に動かす
                if (Input.GetAxis("Horizontal2") < 0)
                {
                    m_pos.x += 0.25f;
                }
                //左方向に動かす
                if (Input.GetAxis("Horizontal2") > 0)
                {
                    m_pos.x += -0.25f;
                }
                //下方向に動かす
                if (Input.GetAxis("Vertical2") < 0)
                {
                    m_pos.y += -0.25f;
                }
                //上方向に動かす
                if (Input.GetAxis("Vertical2") > 0)
                {
                    m_pos.y += 0.25f;
                }
            }

        }
        else {
            this.gameObject.SetActive(false);
        }

        // 移動限界
        if (m_pos.x >  limit.x) m_pos.x =  limit.x;
        if (m_pos.x < -limit.x) m_pos.x = -limit.x;
        if (m_pos.y >  limit.y) m_pos.y =  limit.y;
        if (m_pos.y < -limit.y) m_pos.y = -limit.y;

        // 座標を反映
        this.transform.position = m_pos;

        //if (Input.GetAxis("LTrigger") == 1) {
        //    Debug.Log("mario");
        //}

        SendPos = this.transform.position;
    }

    //カーソルを動かすためにこいつの座標を送る
    public Vector3 GetCTRLPos() {
        return SendPos;
    }
}
