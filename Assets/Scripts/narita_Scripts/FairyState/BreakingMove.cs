using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakingMove : FairyState
{
    // 親オブジェクト
    GameObject fairy;
    // 
    public static BreakingMove Instantiate(GameObject game)
    {
        BreakingMove bm = new BreakingMove();
        // 妖精さんを設定しておく
        bm.fairy = game;

        return bm;
    }


    // 妖精が移動する座標リスト
    static private List<Vector3> MovePoints;
    Vector3 m_vVel;
    Vector3 m_vAcc;
    const float FAIRY_ACC_VALUE = 0.002f;
    const float MAX_VELOCITY = 0.05f;

    // 振動フラグ
    private bool bFlg = false;



    // 初期化
    private void Start()
    {
        MovePoints = new List<Vector3>();
        m_vAcc = m_vVel = Vector3.zero;
    }

    // 更新
    public override void UpdateFairy()
    {
        if(MovePoints.Count <= 1) { Debug.LogWarning("妖精が移動する座標リストのサイズが1以下です"); return; }

        // --- 座標リストの通りに移動する
        // 考え方：座標リストの要素0から要素1に移動する
        //       要素1の座標に到着したら要素0を消す。
        //       リストの要素が1になるまで繰り返す。

        //--- 移動する方向を決める
        Vector3 direction = MovePoints[1] - MovePoints[0];
        //--- 移動速度を決める
        m_vAcc = direction.normalized * FAIRY_ACC_VALUE;
        m_vVel += m_vAcc;

        if (m_vVel.magnitude > 0.09f) m_vVel -= m_vAcc;

        // 現在座標-目的座標、最初の位置-目的座標 の比率が7割を超えたら
        // 速度を徐々に落とすようにする
        float t, t1, t2;
        t1 = Vector3.Distance(fairy.transform.position, MovePoints[1]);
        t2 = Vector3.Distance(MovePoints[0], MovePoints[1]);
        float rate = t1 / t2;
        if (rate < 0.3f)
        {
            m_vVel *= 0.95f;
        }

        //座標を更新
        fairy.transform.position += m_vVel;


        //--- 目的位置に到着したら出発地点である要素0のベクトルをリストから削除する
        //    →現在の要素1が出発地点となる
        Vector3 distance = MovePoints[1] - fairy.transform.position;
        Vector3 nDistance = distance.normalized;
        Vector3 nDirection = direction.normalized;
        float ab = Vector3.Distance(nDistance, nDirection);

        var game = Gamepad.current;
        // 振動
        if (!bFlg)
        {
            if (game != null)
            {
                var gamepad = Gamepad.current;
                //StartCoroutine("Vibration2");
                gamepad.SetMotorSpeeds(0.1f, 0.1f);
            }
            bFlg = true;
        }

        if (ab > 0.0001f)
        {

            MovePoints.RemoveAt(0);

            // このままだと誤差が広がるので修正しておく
            fairy.transform.position = MovePoints[0];
            fairy.transform.position += new Vector3(0, 0, 0.0f);
            m_vVel = m_vAcc = Vector3.zero;
        }
       
        // 妖精が移動し終わったら
        if (MovePoints.Count <= 1)
        {
            // 振動終了
            if(game != null)
            {
                game.SetMotorSpeeds(0.0f, 0.0f);
            }
            // 振動フラグON
            bFlg = false;

            //--- 破る処理
            DivideTriangle.Breaking();

            //--- プレイヤー追従に遷移
            var fs = fairy.GetComponent<Fiary_Script>();
            fs.SetState(Fiary_Script.eFairyState.STATE_FOLLOWING_PLAYER);
            fs.SmallStart();

            //--- サブカメラの妖精を大きくする
            List<GameObject> fairys = new List<GameObject>();
            fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
            foreach (var fairy in fairys)
            {
                // ---ここでスケールを小さくするフラグをONにする
                fairy.GetComponent<Fiary_Move>().BigStart();
            }

            //--- 妖精が移動する座標リストを空にする
            MovePoints.Clear();
        }
    }

    // 妖精が移動する座標リストをセット
    static public void SetFairyMovePoints(List<Vector3> points)
    {
        List<Vector3> newList = new List<Vector3>();
        newList.AddRange(points);
        MovePoints = newList;
    }
}
