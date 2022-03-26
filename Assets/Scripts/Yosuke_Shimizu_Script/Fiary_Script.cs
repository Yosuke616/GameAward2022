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

    //列挙隊を示す変数
    FIARY_MOVE g_FiaryMove;
    
    //=========================================================

    // Start is called before the first frame update
    void Start()
    {
        //列挙隊を初期化する(最初はプレイヤーの周りを追従する)
        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;

        //このオブジェクトの親の情報を取得する
        GameObject ParentObj = this.transform.parent.gameObject;

        Vector3 PlayerPos = ParentObj.transform.position;

        //プレイヤーの少し横に移動させる
        this.transform.position = new Vector3(PlayerPos.x+1.0f, PlayerPos.y + 1.0f, PlayerPos.z) ;
    }

    // Update is called once per frame
    void Update()
    {
        //状態によって更新の内容を変える
        switch (g_FiaryMove) {
            case FIARY_MOVE.FIARY_PLAYER_TRACKING:
                break;
            case FIARY_MOVE.FIARY_BREAK_PAPER:
                break;
            default:break;
        }


    }
}
