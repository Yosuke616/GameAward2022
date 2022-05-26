using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRLCur : MonoBehaviour
{
    private Vector3 SendPos;

    private Vector3 pos;
    private Vector2 limit;

    //ボタンを押したときのフラグ
    private bool g_bFirstFlg;

    // Start is called before the first frame update
    void Start()
    {
        limit.x = CreateTriangle.paperSizeX + 0.05f;
        limit.y = CreateTriangle.paperSizeY + 0.05f;

        //初期化でオブジェクトの場所を決める
        pos = this.transform.position = new Vector3(0.0f,0.5f,0.0f);
        SendPos = new Vector3(0.0f,0.0f,0.0f);
        g_bFirstFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            //右方向に動かす
            if (Input.GetAxis("Horizontal3") < 0)
            {                            
                pos.x += 0.25f;          
            }                            
            //左方向に動かす             
            if (Input.GetAxis("Horizontal3") > 0)
            {
                pos.x += -0.25f;
            }
            //下方向に動かす
            if (Input.GetAxis("Vertical3") < 0)
            {
                pos.y += -0.25f;
            }
            //上方向に動かす
            if (Input.GetAxis("Vertical3") > 0)
            {
                pos.y += 0.25f;
            }
        }
        else {
            this.gameObject.SetActive(false);
        }

        // 移動限界
        if (pos.x >  limit.x) pos.x = limit.x;
        if (pos.x < -limit.x) pos.x = -limit.x;
        if (pos.y >  limit.y) pos.y =  limit.y;
        if (pos.y < -limit.y) pos.y = -limit.y;

        this.transform.position = pos;


        SendPos = this.transform.position;
    }

    //カーソルを動かすためにこいつの座標を送る
    public Vector3 GetCTRLPos() {
        return SendPos;
    }
}
