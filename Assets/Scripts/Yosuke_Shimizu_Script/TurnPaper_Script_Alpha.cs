/*
 2022/3/27 ShimizuYosuke 
 ｶﾒﾗ中央からレイを出して順番にリストに登録する
 矢印上下でめくれるようにする
 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaper_Script_Alpha : MonoBehaviour
{
    //一番最初のページを保存するための変数
    private int g_nFirstPage;

    //何個オブジェクトがあったかをカウントする変数
    private int g_nCountPage;

    //枚数分のオブジェクトを格納するためのリスト
    private List<GameObject> PageList = new List<GameObject>();

    //リストの最初か最後を保存するための変数
    private GameObject SaveObject;

    //一回だけ初期化するための変数
    private bool g_bFirst_Load;

    //Rayを使うための変数
    Ray ray;

    //レイが引っかかったら反応するようにする
    RaycastHit Hit;

    float i = 1;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        //カウントを0にする
        g_nCountPage = 0;
        //リストを初期化する
        PageList.Clear();
        //最初は何も指していない
        SaveObject = null;
        //最初だけ初期化できるようにする
        g_bFirst_Load = false;
        //一番最初のページを保存する
        g_nFirstPage = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //最初に初期化してあとは通らない
        if (!g_bFirst_Load) {
            //カメラを見つける
            GameObject Camera = GameObject.Find("MainCamera");
            //終点を見つける
            GameObject EndPos = GameObject.Find("EndPosition");

            //カメラからまっすぐにレイを飛ばす
            ray = new Ray(
                origin: Camera.transform.position,      //始点
                direction: EndPos.transform.position    //終点
            );

            //複数あるのでそれをリストに格納する
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                if (hit.collider.CompareTag("paper")) {
                    //リストに追加するよ
                    PageList.Add(hit.collider.gameObject);

                    if (PageList.Count == 1) {
                        //一番最初だけの処理をする
                        g_nFirstPage = 0;
                    }

                    //紙にめくるためのコンポーネントを持たせる
                    PageList[g_nCountPage].AddComponent<Turn_Shader>();

                    //カウントを増やしていく
                    g_nCountPage++;
                }
              
            }

            //ソートして並び変える
            PageList.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());

            g_bFirst_Load = true;
        }

        //ページ切り替え関数呼び出し
        SlidPaper();

        //Aボタン押したら数値を変える
        if (Input.GetKeyDown(KeyCode.C)) {
            PageList[0].GetComponent<Turn_Shader>().SetPaperSta(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PageList[0].GetComponent<Turn_Shader>().SetPaperSta(2);
        }
    }




    //ページ切り替え用の関数
    private void SlidPaper() {

        if (Input.GetKeyDown(KeyCode.UpArrow)){
            //ポインタ的な使い方(リストの先頭のオブジェクトを回避させる)
            SaveObject = PageList[0];
            //先頭のリストを削除する
            PageList.RemoveAt(0);

            //場所を変える
            SaveObject.transform.position = new Vector3(0.0f, 0.0f,(0.05f * (g_nCountPage)));
            //リストの最後尾に追加する
            PageList.Add(SaveObject);

            //座標を変更できる用の変数を用意する
            Vector3 pos;

            //全体を少しずつ前に出す
            for (int i = 0; i < g_nCountPage; i++)
            {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position;
                //少しずらす
                pos.z -= 0.05f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }

            //ページの先頭が移動したのと同じだけ数値を変える
            //下を押すと前に行き少しずつ後ろに進んでいくのでプラスしていく（最後尾のやつを前に持ってくる）
            g_nFirstPage++;
            if (g_nFirstPage > (g_nCountPage - 1))
            {
                g_nFirstPage = 0;
            }

            //最後にシェーダーの情報を元に戻す
            //SaveObject.GetComponent<Renderer>().material.SetFloat("_Flip", 1);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //最後尾のリストの要素をポインタ的に当てはめる
            SaveObject = PageList[g_nCountPage - 1];
            //最後尾のリストのやつを削除する
            PageList.RemoveAt(g_nCountPage - 1);

            //リストの先頭に追加する
            PageList.Insert(0, SaveObject);

            //場所を変える
            SaveObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

            //オブジェクトの場所を保存する為の変数
            Vector3 pos;

            //ページ全体を後ろに下げる
            for (int i = g_nCountPage - 1; i > 0; i--)
            {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position;
                //少しずらす
                pos.z += 0.05f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }

            //ページの先頭が移動したのと同じだけ数値を変える
            //上を押すと後ろに行き少しずつ前に戻っていくのでマイナスしていく(先頭のやつを一番後ろに送る)
            g_nFirstPage--;
            //0より小さくなったら最後尾に行くために数値を多きする
            if (g_nFirstPage < 0)
            {
                //マイナス1するのは0から始めているため
                g_nFirstPage = (g_nCountPage - 1);
            }
        }

        //エンターを押したら並び順が元通りに戻る
        if (Input.GetKeyDown(KeyCode.Space) && g_nFirstPage != 0)
        {

            //少しずつずらせるようなカウント
            int ShiftCount = 0;

            //座標を変更できる用の変数を用意する
            Vector3 pos;

            //一番手前(初期)のページよりも後ろの人たちをまずは移動させる
            for (int i = g_nFirstPage; i < g_nCountPage; i++, ShiftCount++)
            {
                //リストの情報を退避させる
                SaveObject = PageList[i];

                //そこのリストの情報を消す
                PageList.RemoveAt(i);

                //変数にそれぞれの座標を代入する
                pos = SaveObject.transform.position;

                //番号に応じた分の距離を移動させる
                pos.z += (0.05f * g_nFirstPage);

                //計算した距離を適用させる
                SaveObject.transform.position = pos;


                //リストの並び順も変えなければならない
                //リストの先頭に追加する
                PageList.Insert(ShiftCount, SaveObject);

            }

            //一番手前(初期)のページよりも前にいる人たちを移動させる
            for (int i = (g_nCountPage - g_nFirstPage); i < g_nCountPage; i++, ShiftCount++)
            {
                //リストの移動の為に代入する
                SaveObject = PageList[i];

                //リストにあったものを削除する
                PageList.RemoveAt(i);

                //変数にそれぞれの座標を代入する
                pos = SaveObject.transform.position;

                //番号に応じた分の距離を移動させる(初期ページのあった位置分足す)
                pos.z -= (0.05f * (g_nCountPage - g_nFirstPage));

                //計算した距離を適用させる
                SaveObject.transform.position = pos;

                //リストの並び順も変えなければならない
                //リストの最後尾に追加する
                PageList.Insert(ShiftCount, SaveObject);
            }

            //ページはリセットされるため0にする
            g_nFirstPage = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            for (int i = 0;i < g_nCountPage ;i++) {
                Debug.Log(i,PageList[i]);
                Debug.Log(99,SaveObject);
            }
        }
    }
}
