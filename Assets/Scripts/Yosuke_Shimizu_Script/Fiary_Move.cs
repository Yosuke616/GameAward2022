using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Move : MonoBehaviour
{

    //親オブジェクトの情報を格納する変数
    GameObject ParentObj_Move;

    //親の座標を格納する為の変数
    Vector3 PlayerPos_Move;

    bool qqq;

    // Start is called before the first frame update
    void Start()
    {
        //このオブジェクトの親の情報を取得する
        ParentObj_Move = this.transform.parent.gameObject;

        //親オブジェクトの座標を保存する
        PlayerPos_Move = ParentObj_Move.transform.position;

        //プレイヤーの少し横に移動させる
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        qqq = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!qqq) {
           　qqq = true;
            return;
        }

        Debug.Log("ロゼッタ様最高");

        GameObject ParentObj = transform.parent.gameObject;

        if (this.GetComponent<Fiary_Script>().GetMove()) {
            //親オブジェクトの座標を更新する
            ParentObj_Move = this.transform.parent.gameObject;

            //親オブジェクトの座標を保存する
            PlayerPos_Move = ParentObj_Move.transform.position;

            //プレイヤーの少し横に移動させる
            this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);
        }
    }
}
