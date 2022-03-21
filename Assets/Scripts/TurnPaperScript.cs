/*
 2022/3/1 志水陽祐 
 紙をめくるためのスクリプト
 動的配列でページ番号を制御するようにする
 2022/3/5 志水陽祐
 色の変更だがマテリアルを事前に用意したやり方じゃなければ出来ない
 いずれテクスチャをランダムに設定できるようにしたい
 2022/3/6 志水陽祐
 ボタンで並び替えをしたい
 2022/3/8 志水陽祐
 並び替えができるようになった
 2022/3/16 志水陽祐
 一番手前に来ているものに当たり判定を付ける
 やりたいこと・・・複数のオブジェクトを配列に入れてそれをリストに入れて動じにめくれるようにする。
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaperScript : MonoBehaviour
{

    //オブジェクトの名前を取得する為のモノ
    public GameObject g_Page;

    //リストで制御するために動的配列を作る
    public List<GameObject> PageList = new List<GameObject>();

    //ページ数を決めるための変数(オブジェクトの数)
    [SerializeField] private int m_nPageMax = 100;

    //リストの最初か最後を保存するための変数
    private GameObject SaveObject;

    //配置するマテリアルの設定
    public Material[] ColorSet = new Material[1];

    //ページ番号を保存するための変数
    private int m_nPageNum;

   // Start is called before the first frame update
   void Start()
    {
        for (int i = 0; i < m_nPageMax; i++)
        {
            //スクリプトでオブジェクトを追加する
            GameObject Page = GameObject.Instantiate(g_Page) as GameObject;     //生成
            Page.transform.position = new Vector3(15.0f, 0.0f, 100.0f - i);       //座標
            Page.transform.localScale = new Vector3(100.0f, 0.5f, 100.0f);        //大きさ
            Page.transform.rotation = Quaternion.Euler(90, 0, 0);               //回転
            
            //名前の設定
            string szName = i.ToString();
            Page.name = szName;



            //一番最初に追加したものには色を付けて分かりやすくする
            if (i == 0)
            {
                //色の設定
                Page.GetComponent<MeshRenderer>().material = ColorSet[0];

                //一番最初のページの番号を保存しておく
                m_nPageNum = 0;
            }
            else {
                //当たり判定を一番手前以外消し去る
                var Coll = Page.GetComponent<BoxCollider>();
                Coll.enabled = false;
            }

            //リストに追加する
            PageList.Add(Page);
        }

        //リストに追加された物を表示
        foreach (GameObject i in PageList)
        {
            Debug.Log(i.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //ページの切り替え
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //当たり判定を削除する
            var Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = false;

            //ポインタ的な使い方(リストの先頭のオブジェクトを回避させる)
            SaveObject = PageList[0];
            //先頭のリストを削除する
            PageList.RemoveAt(0);

            //場所を変える
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f - (1* m_nPageMax));
            //リストの最後尾に追加する
            PageList.Add(SaveObject);

            //座標を変更できる用の変数を用意する
            Vector3 pos;

            //全体を少しずつ前に出す
            for (int i = 0;i < m_nPageMax; i++ ) {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position ;
                //少しずらす
                pos.z += 1.0f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }

            //ページの先頭が移動したのと同じだけ数値を変える
            //上を押すと後ろに行き少しずつ前に戻っていくのでマイナスしていく(先頭のやつを一番後ろに送る)
            m_nPageNum--;
            //0より小さくなったら最後尾に行くために数値を多きする
            if (m_nPageNum < 0) {
                //マイナス1するのは0から始めているため
                m_nPageNum = (m_nPageMax-1);
            }

            //一番手前のページに当たり判定をオンにする
            Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = true;

        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //先頭の当たり判定を消す
            var Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = false;

            //最後尾のリストの要素をポインタ的に当てはめる
            SaveObject = PageList[m_nPageMax - 1];
            //最後尾のリストのやつを削除する
            PageList.RemoveAt(m_nPageMax - 1);

            //リストの先頭に追加する
            PageList.Insert(0, SaveObject);
            
            //場所を変える
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f);


            //オブジェクトの場所を保存する為の変数
            Vector3 pos;

            //ページ全体を後ろに下げる
            for (int i = m_nPageMax - 1; i > 0;i--) {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position;
                //少しずらす
                pos.z -= 1.0f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }

            //ページの先頭が移動したのと同じだけ数値を変える
            //下を押すと前に行き少しずつ後ろに進んでいくのでプラスしていく（最後尾のやつを前に持ってくる）
            m_nPageNum++;
            if (m_nPageNum > (m_nPageMax-1)) {
                m_nPageNum = 0;
            }

            //先頭の当たり判定をオンにする
            Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = true;
        }

        //エンターを押したら並び順が元通りに戻る
        if (Input.GetKeyDown(KeyCode.Return) && m_nPageNum != 0) {

            //少しずつずらせるようなカウント
            int ShiftCount = 0; 

            //座標を変更できる用の変数を用意する
            Vector3 pos;

            //一番手前(初期)のページよりも後ろの人たちをまずは移動させる
            for (int i = m_nPageNum; i < m_nPageMax; i++, ShiftCount++)
            {
                //リストの情報を退避させる
                SaveObject = PageList[i];

                //そこのリストの情報を消す
                PageList.RemoveAt(i);

                //変数にそれぞれの座標を代入する
                pos = SaveObject.transform.position;

                //番号に応じた分の距離を移動させる
                pos.z += (1.0f * m_nPageNum);

                //計算した距離を適用させる
                SaveObject.transform.position = pos;


                //リストの並び順も変えなければならない
                //リストの先頭に追加する
                PageList.Insert(ShiftCount, SaveObject);

            }

            //一番手前(初期)のページよりも前にいる人たちを移動させる
            for (int i = (m_nPageMax-m_nPageNum); i < m_nPageMax; i++,ShiftCount++)
            {
                //リストの移動の為に代入する
                SaveObject = PageList[i];

                //リストにあったものを削除する
                PageList.RemoveAt(i);

                //変数にそれぞれの座標を代入する
                pos = SaveObject.transform.position;

                //番号に応じた分の距離を移動させる(初期ページのあった位置分足す)
                pos.z -= (1.0f * (m_nPageMax - m_nPageNum));

                //計算した距離を適用させる
                SaveObject.transform.position = pos;

                //リストの並び順も変えなければならない
                //リストの最後尾に追加する
                PageList.Insert(ShiftCount,SaveObject);
            }

            //ページはリセットされるため0にする
            m_nPageNum = 0;
        }



        //スペースボタンを押すと一番手前が消える
        if (Input.GetKeyDown(KeyCode.Space)){
            ////一ページ目から破いていきます
            //Destroy(PageList[0]);
            ////↑オブジェクト事態を消す↓リストから削除する
            //PageList.RemoveAt(0);   

            //リストに追加された物を表示
            //foreach (GameObject i in PageList)
            //{
            //    Debug.Log(i.name);
            //}

            //デバック用赤ページと数字の場所があっているか
            Debug.Log(m_nPageNum);
        }

        //5枚目より後ろはアクティブをオフにする
        //つまり見えなくする
        for (int i = 0;i < m_nPageMax ;i++) {
            if (i > 4)
            {
                PageList[i].SetActive(false);
            }
            else {
                PageList[i].SetActive(true);
            }
        }
    }
    
}
