using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public int state = 0;      // 0:上下移動  0以外:左右移動
    public float move = 0.01f;
    private int counter = 0;
    private float diff = 0.0f;
    private Vector3 startPos;

    private void Start()
    {
        // 初期位置を保存しておく
        startPos = transform.position;
    }

    private void Update()
    {
        Vector3 p;
        // 上下移動
        if (state == 0) p = new Vector3(0, move, 0);
        // 左右移動
        else p = new Vector3(move, 0, 0);

        // 座標に反映
        transform.Translate(p);

        // フレームカウンタ更新
        counter++;

        //countが500になれば-1を掛けて逆方向に動かす
        if (counter == 500)
        {
            counter = 0;
            move *= -1;
        }

        // 初期位置からの距離
        if(state == 0)
        {
            diff = transform.position.y - startPos.y;
        }
        else
        {
            diff = startPos.x - transform.position.x;
        }
    }

    public int getCounter()
    {
        return counter;
    }

    // シンクにてぃんくる
    public void Synchronous(Enemy enemyFunc)
    {
        counter = enemyFunc.counter;
        move = enemyFunc.move;
        state = enemyFunc.state;

        // カウンターをもとに移動量を計算
        //float moveValue = Mathf.Abs(move) * counter;
        //float moveValue = Mathf.Abs(move) * counter;


        Vector3 p;
        // 上下移動
        if (state == 0) p = new Vector3(0, enemyFunc.diff, 0);
        // 左右移動
        else p = new Vector3(-enemyFunc.diff, 0, 0);

        transform.Translate(p);
    }

    // 左右移動の状態にする
    void HorizontalMovement()
    {
        state = 1;
    }

    // 上下移動にする
    void VerticalMovement()
    {
        state = 0;
    }


    // エネミーの機能を与える
    // 第一引数:エネミー機能をつける対象
    // 第二引数:カメラに写っているオリジナルの動き
    static public void AddEnemyFunction(GameObject obj, Enemy originalEnemy)
    {
        if (obj == null || originalEnemy == null) return;

        var enemyFunc = obj.AddComponent<Enemy>();

        enemyFunc.Synchronous(originalEnemy);
    }
}
