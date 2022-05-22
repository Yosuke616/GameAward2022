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
    public enum FIARY_MOVE
    {
        FIARY_PLAYER_TRACKING = 0,          //プレイヤーに追従する
        FIARY_BREAK_PAPER,                  //外周をたどる
        FIARY_PAPER_IN,                     //紙を破っている情報を保存している状態
        FIARY_BREAK_MOVE,                   //紙を実際破るために移動している状態

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

    //クリックしたところを保存するためのリスト
    private List<Vector3> MousePos = new List<Vector3>();

    //前回の座標を保存するための変数
    private Vector3 Old_Mouse_Pos;

    //紙の外周の座標を保存するための変数
    private Vector3 Paper_Out;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        //列挙隊を初期化する(最初はプレイヤーの周りを追従する)
        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;

        //最初は読み込めるようにする
        g_bFirst_Load = false;

        //最初だけ外枠を見れるようにするためのフラグ
        gFirst_Flg = true;

        //リストの中身を初期化する
        MousePos.Clear();

        //前回の座標を保存するための変数
        Old_Mouse_Pos = new Vector3(0.0f, 0.0f, 0.0f);

        //外周をほぞんするっための変数の初期化
        Paper_Out = new Vector3(0.0f, 0.0f, 0.0f);

        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //読み込みできるようにする(疑似初期化)
        if (!g_bFirst_Load)
        {
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

        //プレイヤーの状況を得る
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            //左マウスをクリックしているか(場合によって)
            //クリックしていた場合
            //プレイヤーに追従しているときだけこの中に入る

            //コントローラーを押せるようのオブジェクトをゲットする
            GameObject obj = GameObject.Find("MainCamera");

            if (!CS_Script.GetBreakFlg() || Input.GetKeyDown(KeyCode.T))
            //クリックしていなかった場合
            {
                //中身が存在するのならばこの中に入る
                if (MousePos.Count > 0)
                {
                    g_FiaryMove = FIARY_MOVE.FIARY_BREAK_MOVE;
                }
            }

            Debug.Log(obj.GetComponent<InputTrigger>().GetOneTimeDown());

            //状態によって更新の内容を変える
            switch (g_FiaryMove)
            {
                case FIARY_MOVE.FIARY_PLAYER_TRACKING:                   

                    //ここで破るフラグが立っていたら変える
                    if (CS_Script.GetBreakFlg())
                    {
                        if (!gFirst_Flg)
                        {
                            g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
                            //ここで一番最初のポジションを得る
                            MousePos.Add(OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPoss_IN());
                        }

                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0) || obj.GetComponent<InputTrigger>().GetOneTimeDown())
                        {
                            g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;
                        }
                    }

                    Debug.Log("FIARY_PLAYER_TRACKING");

                    break;
                case FIARY_MOVE.FIARY_BREAK_PAPER:
                    //一番最初の紙の破る場所に行くやつ

                    //外周の急のポジションに移動させる
                    this.transform.position = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();

                    //個々の中に入っているときにクリックすると中にゆっくり移動していくモードに変わる
                    if (Input.GetMouseButtonDown(0) || obj.GetComponent<InputTrigger>().GetOneTimeDown())
                    {
                        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
                        gFirst_Flg = false;
                    }

                    //外周用に保存しておく
                    Paper_Out = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();

                    Debug.Log("FIARY_BREAK_PAPER");

                    break;
                case FIARY_MOVE.FIARY_PAPER_IN:
                    //妖精が破るための道順を決める

                    //妖精の位置は壱か所に固定しておく
                    this.transform.position = Paper_Out;

                    //↓後々使う
                    //クリックした場所をここに保存する
                    Vector3 vPos = Input.mousePosition;
                    vPos.z = 10.0f;

                    //ワールド座標に変える
                    Vector3 vWorldPos = Vector3.zero;
                    if (Camera.main != null) vWorldPos = Camera.main.ScreenToWorldPoint(vPos);

                    //クリックされた場所をリストに保存する(前回の座標と違ったときにリストに追加する)
                    if (!(vWorldPos == Old_Mouse_Pos))
                    {
                        if (Input.GetMouseButtonDown(0) || obj.GetComponent<InputTrigger>().GetOneTimeDown())
                        {
                            ////リストに追加する
                            MousePos.Add(vWorldPos);
                            ////過去の座標と比べるために今の座標を保存しておく
                            Old_Mouse_Pos = vWorldPos;

                        }
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        for (int i = 0; i < MousePos.Count; i++)
                        {
                            Debug.Log(MousePos[i]);
                        }
                    }

                    Debug.Log("FIARY_PAPER_IN");
                    break;

                case FIARY_MOVE.FIARY_BREAK_MOVE:
                    //リストに中身が入っている状態で破れるフラグがオフになっていたら破くモーションに移る
                    //リストをたどって妖精を移動させる

                    //現在の状況2022/4/12
                    //最後にカーソルのあった場所まで移動する

                    //このままだと一瞬で終わるのでこの中のリストのなかみが全て消えたらプレイヤー追従モードにする
                    Debug.Log(this.transform.position);

                    //===============================================================================================
                    //妖精の元の場所との差分を出して妖精を移動させる(仮でできた後で必ず手直しする)
                    //差分を出してそれを保存するための変数
                    Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    //ターゲットと自身とのポジションを引いて移動させたい速度をかける
                    velocity += (MousePos[0] - this.transform.position) * 0.1f;
                    //加速度的なものこれが無いと一定の速度になる
                    //velocity *= 0.5f;
                    //最後に移動しようとさせる
                    this.transform.position += velocity;
                    //===============================================================================================

                    count++;

                    //リストの最初を消す(プレイヤーとリストで指定した座標が一致したら消す)
                    if (count > 30)
                    {
                        MousePos.RemoveAt(0);
                        count = 0;
                    }

                    //リストの中身が無くなったらゆっくり消していく
                    if (MousePos.Count == 0)
                    {
                        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
                        gFirst_Flg = true;
                        Paper_Out = new Vector3(0.0f, 0.0f, 0.0f);
                        Debug.Log("いい感じ");
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        Debug.Log(MousePos[0]);
                        Debug.Log(this.transform.position);
                    }

                    Debug.Log("FIARY_BREAK_MOVE");

                    break;

                default: break;
            }
        }
        else {
            //this.gameObject.SetActive(false);
        }
    }

    public bool GetMove() {
        if (g_FiaryMove == FIARY_MOVE.FIARY_PLAYER_TRACKING) {
            return true;
        }

        return false;
    }
}
