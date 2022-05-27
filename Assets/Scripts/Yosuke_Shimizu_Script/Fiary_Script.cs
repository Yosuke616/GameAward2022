/*
 2022/3/26 ShimizuYosuke　妖精の動きを作るためのスクリプト
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Script : MonoBehaviour
{
    public enum eFairyState
    {
        STATE_FOLLOWING_PLAYER = 0,     // サブカメラプレイヤーに追従
        STATE_DICISION_BREAKING_POINT,  // 破り開始位置決定
        STATE_BREAKING_MOVE,            // 破り中

        MAX_FAIRY_STATE
    }

    //紙を破っているかどうかを確認するための変数
    CursorSystem CS_Script;
    //カーソルのシステムを得るための関数
    GameObject Cursor_System;

    // ステートの数だけ要素を確保する
    private FairyState[] m_FairyState = new FairyState[(int)eFairyState.MAX_FAIRY_STATE];
    // 現在のステート
    [SerializeField] private eFairyState m_CurrentState;
    public eFairyState GetState { get { return m_CurrentState; } }
    public void SetState(eFairyState state)
    {
        m_CurrentState = state;
    }


    // 小さくするフラグ
    private bool small, big;
    Vector3 normalScale;
    int frameCount;
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
    public void Back()
    {
        this.transform.Rotate(new Vector3(0, 90, 0));
    }
    public void Front()
    {
        this.transform.Rotate(new Vector3(0, 90, 0));
    }


    void Start()
    {
        big = small = false;
        normalScale = new Vector3(15.0f, 15.0f, 15.0f);
        frameCount = 30;

        //破っているかどうかのフラグが分かるようなカーソルを得る
        Cursor_System = GameObject.Find("Cursor");
        //スクリプトを得る
        CS_Script = Cursor_System.GetComponent<CursorSystem>();

        // 最初はプレイヤー追従
        m_CurrentState = eFairyState.STATE_FOLLOWING_PLAYER;

        // 各ステートを配列に入れておく(AddComponentのようなもの)
        // プレイヤー追従
        m_FairyState[(int)eFairyState.STATE_FOLLOWING_PLAYER] = FollowingPlayer.Instantiate(this.gameObject);
        // 破り座標の決定
        m_FairyState[(int)eFairyState.STATE_DICISION_BREAKING_POINT] = DicisionBreakingPoint.Instantiate(this.gameObject);
        // 破り中
        m_FairyState[(int)eFairyState.STATE_BREAKING_MOVE] = BreakingMove.Instantiate(this.gameObject);


    }

    void Update()
    {
        //プレイヤーの状況を得る
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            // ステートに応じて更新内容を変える
            m_FairyState[(int)m_CurrentState].UpdateFairy();

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
        else
        {
            //this.gameObject.SetActive(false);
        }
    }

}
