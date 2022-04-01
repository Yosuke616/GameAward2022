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
        FIARY_PLAYER_TRACKING = 0,          //プレイヤーに追従する
        FIARY_BREAK_PAPER,                  //外周をたどる
        FIARY_PAPER_IN,                     //紙が中に入っている

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

    //紙を破っているかどうかを確認するための変数
    CursorSystem CS_Script;

    //カーソルのシステムを得るための関数
    GameObject Cursor_System;

    //一回だけ外枠に行けるように
    private bool gFirst_Flg;

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

        //最初だけ外枠を見れるようにするためのフラグ
        gFirst_Flg = true;
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


            //破っているかどうかのフラグが分かるようなカーソルを得る
            Cursor_System = GameObject.Find("Cursor");

            //スクリプトを得る
            CS_Script = Cursor_System.GetComponent<CursorSystem>();

            //弐回目以降読み込まないようにする
            g_bFirst_Load = true;
        }

        //左マウスをクリックしているか(場合によって)
        //クリックしていた場合
        if (CS_Script.GetBreakFlg())
        {
            //破るモードで最初に一回だけは外周を回るモードに移行する
            if (gFirst_Flg)
            {
                g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;
            }
            else {
                g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
            }
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
                //個々の中に入っているときにクリックすると中にゆっくり移動していくモードに変わる
                if (Input.GetMouseButtonDown(0)) {
                    g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
                    gFirst_Flg = false;
                }
      
                break;
            case FIARY_MOVE.FIARY_PAPER_IN:
                //クリックした場所をここに保存する
                Vector3 vPos = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();
                //妖精の元の場所との差分を出して妖精を移動させる(仮でできた後で必ず手直しする)
                Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);
                velocity += (vPos - this.transform.position)* 0.05f;
                velocity *= 0.5f;
                this.transform.position += velocity;                
                break;
            
            default:break;
        }


    }
}
