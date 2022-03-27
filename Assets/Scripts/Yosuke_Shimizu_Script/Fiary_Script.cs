/*
 2022/3/26 ShimizuYosuke　妖精の動きを作るためのスクリプト
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Script : MonoBehaviour
{
    //定数定義=================================================
    //妖精の状態を示す
    public enum FIARY_MOVE {
        FIARY_PLAYER_TRACKING = 0,
        FIARY_BREAK_PAPER,

        FIARY_MAX
    }
    //=========================================================

    //列挙隊を示す変数
    FIARY_MOVE g_FiaryMove;

    //カーソル状態を持っているスクリプトを持ってくる
    OutSide_Paper_Script_Second Paper_OutSide_Script;

    //カーソルのゲームオブジェクトを見つける為の変数
    GameObject OutSide_Cursor;

    //最初の一回だけ読み込めるようにする(trueで読み込めなくする)
    private bool g_bFirst_Load;


    //親オブジェクトの情報を格納する変数
    GameObject ParentObj;

    //親の座標を格納する為の変数
    Vector3 PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        //列挙隊を初期化する(最初はプレイヤーの周りを追従する)
        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;

        //このオブジェクトの親の情報を取得する
        ParentObj = this.transform.parent.gameObject;

        //親オブジェクトの座標を保存する
        PlayerPos = ParentObj.transform.position;

        //プレイヤーの少し横に移動させる
        this.transform.position = new Vector3(PlayerPos.x+1.0f, PlayerPos.y + 1.0f, PlayerPos.z) ;

        //最初は読み込めるようにする
        g_bFirst_Load = false;
    }

    // Update is called once per frame
    void Update()
    {
        //読み込みできるようにする(疑似初期化)
        if (!g_bFirst_Load) {
            //カーソルを見つける
            OutSide_Cursor = GameObject.Find("cursor");
            //カーソルの中のスクリプトをゲットする
            Paper_OutSide_Script = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>();

            //弐回目以降読み込まないようにする
            g_bFirst_Load = true;
        }

        //左マウスをクリックしているか(場合によって)
        //クリックしていた場合
        if (Input.GetMouseButton(0))
        {
            g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;   
        }
        //クリックしていなかった場合
        else
        {
            g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
        }

        //状態によって更新の内容を変える
        switch (g_FiaryMove) {
            case FIARY_MOVE.FIARY_PLAYER_TRACKING:

                //親オブジェクトの座標を更新する
                ParentObj = this.transform.parent.gameObject;

                //親オブジェクトの座標を保存する
                PlayerPos = ParentObj.transform.position;

                //プレイヤーの少し横に移動させる
                this.transform.position = new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z);

                break;
            case FIARY_MOVE.FIARY_BREAK_PAPER:
                //マウスのポジションに移動させる
                this.transform.position = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();
                break;
            default:break;
        }


    }
}
