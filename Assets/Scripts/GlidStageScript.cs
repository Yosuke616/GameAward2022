/*
 2022/3/18 ShimizuYosuke 
 画面いっぱいに当たり判定のある透明四角いオブジェクトを生成する(横400、縦300)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidStageScript : MonoBehaviour
{
    //オブジェクトの名前を取得するためのモノ
    public GameObject g_GlidColl;

    //==============================
    //色配置するマテリアルの設定
    public Material[] ColorSet = new Material[2];

    //色を変えるフラグ(trueだったら赤色に変える)
    private bool m_nSetColor =  true;
    //==============================

    //縦の数と横の数を設定する為の変数
    [SerializeField] private int GlidColl_Num_X = 400;
    [SerializeField] private int GlidColl_Num_Y = 300;

    [SerializeField] private float Position_Z = 100;

    // Start is called before the first frame update
    void Start()
    {
        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject Camera = GameObject.Find("MainCamera");

        //名前を変えるためにカウントを作る
        int nNameCnt = 0;

        for (int i = 0;i < GlidColl_Num_Y; i++) {
            for (int u = 0; u < GlidColl_Num_X; u++,nNameCnt++)
            {
                //スクリプトでオブジェクトを追加する
                GameObject GlidColl = GameObject.Instantiate(g_GlidColl) as GameObject;         //生成
                         
                //親と子の設定をする(ここではｶﾒﾗを親にする)
                GlidColl.transform.SetParent(Camera.transform);

                //オブジェクトの設定
                GlidColl.transform.position = new Vector3((Camera.transform.position.x - 211.0f)+(10.0f*u), (Camera.transform.position.y + 116.0f)- (10.0f * i), Position_Z);                      //座標
                GlidColl.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);                    //大きさ
                GlidColl.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);                 //回転

                //名前の設定(最後のiは何行目にあるか)
                GlidColl.name = "Glid_No." + nNameCnt + ":" + i;

                //色の設定(初期値は赤)
                GlidColl.GetComponent<MeshRenderer>().material = ColorSet[1];

                //マウスの当たり判定用のコンポーネントを追加する
                GlidColl.AddComponent<BoxCollider>();
                var Coll = GlidColl.GetComponent<BoxCollider>();
                Coll.enabled = true;

                //タグを付ける
                GlidColl.tag = "GlidObject";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //右クリックしてオブジェクトを消せるようにする(いったん)
        // マウス左ボタンをクリックした時
        if (Input.GetMouseButtonDown(0))
        {
            // スクリーン位置から3Dオブジェクトに対してRay（光線）を発射
            Ray rayOrigine = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // Rayがオブジェクトにヒットした場合
            if (Physics.Raycast(rayOrigine, out RaycastHit hitInfo))
            {
                //GameObjectとして扱うために変数を作って代入する
                GameObject HitObject = hitInfo.collider.gameObject;

                //ここで関数を呼び出す
                ClickGlid(HitObject);
            }
        }
    }

    //クリックしたら色々変わる関数(主に当たり判定の有無と色)
    private void ClickGlid(GameObject HitObject) {

        //破る用の当たり判定を取得する
        var Coll = HitObject.GetComponent<MeshCollider>();

        //マウスのRayを検出する用の当たり判定
        var MouseColl = HitObject.GetComponent<BoxCollider>();

        //色を変える
        if (Coll.enabled == false)
        {
            //赤色に変える
            HitObject.GetComponent<MeshRenderer>().material = ColorSet[1];
            //当たり判定をオンにする
            Coll = HitObject.GetComponent<MeshCollider>();
            Coll.enabled = true;
        }
        else if(Coll.enabled ==true){
            //青色に変える
            HitObject.GetComponent<MeshRenderer>().material = ColorSet[0];
            //当たり判定をオフにする
            Coll = HitObject.GetComponent<MeshCollider>();
            Coll.enabled = false;

        }

        //デバッグ用名前の検出
        Debug.Log(HitObject.name);
    } 
}
