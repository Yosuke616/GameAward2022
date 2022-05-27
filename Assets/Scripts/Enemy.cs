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

    private bool m_bActive;
    public void SetActive(bool active)
    {
        m_bActive = active;
    }

    private void Start()
    {
        m_bActive = false;
        // 初期位置を保存しておく
        startPos = transform.position;
    }

    private void Update()
    {
        if (m_bActive)
        {
            Vector3 p;
            // 上下移動
            if (state == 0) p = new Vector3(move, 0, 0);
            // 左右移動
            else p = new Vector3(0, move, 0);

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
            if (state == 0)
            {
                diff = transform.position.y - startPos.y;
            }
            else
            {
                diff = startPos.x - transform.position.x;
            }
        }
    }

    public int getCounter()
    {
        return counter;
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

    // あたり判定オブジェクトと実際に見えているオブジェクトの座標を合わせる
    public void Synchronous(GameObject gameObject)
    {
        //Vector3 p;
        //if (state == 0) p = new Vector3(-diff, 0, 0); 
        //// 左右移動
        //else p = new Vector3(0, diff, 0);
        //
        //gameObject.transform.Translate(p);

        Vector3 distance;
        distance = transform.position - startPos;

        gameObject.transform.Translate(distance);
    }
}
