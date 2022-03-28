/*
 2022/3/26 ShimizuYosuke
 中心から外側に向かってレイを飛ばし、さらに中心に向かって飛ばす
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script : MonoBehaviour
{
    //値を返すためのレイ
    RaycastHit hits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの座標を取得する(ビューボート座標)
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mousePos.z = 0.1f;

        //レイを出し始める座標
        Vector3 StartPos = new Vector3(0.0f,0.0f,0.1f);

        //計算用に少しだけ減らす
        mousePos.x -= 0.5f;
        mousePos.y -= 0.5f;

        //レイにぶつかったオブジェクトの情報を取得する
        RaycastHit hit;

        //マウス座標用の計算
        mousePos *= 10000;
        mousePos.z = 0.1f;


        //中央からマウスの座標に向けてレイを飛ばす
        if (Physics.Raycast(StartPos, mousePos, out hit)) {
            if (hit.collider.CompareTag("OutSideHitBox")) {
                //レイを可視化する
                Debug.DrawRay(StartPos,mousePos,Color.red,1.0f);

                //ヒットした座標を保存しておく
                Vector3 HitPos = hit.point;
                HitPos.z = 0.1f;

                //Debug.Log(HitPos);

                //レイを飛ばす計算をする
                mousePos *= -1;
                mousePos.z = 0.1f;


                //ヒットしたところからさらにレイを飛ばす
                if (Physics.SphereCast(HitPos, 5.0f, mousePos, out hits))
                {
                    Debug.DrawRay(HitPos, mousePos, Color.blue, 1.0f);
                    if (hits.collider.CompareTag("paper")) {
                        Debug.Log(hits.point);
                    }
                }

            }
        }
    }

    //二回目のレイでぶつかった外側の座標を送るためのやつ
    public Vector3 GetPaperHit() {
        return hits.point;
    }
}
