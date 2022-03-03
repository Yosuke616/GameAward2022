/*
 2022/3/1 志水陽祐 
 紙をめくるためのスクリプト
 動的配列でページ番号を制御するようにする
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaperScript : MonoBehaviour
{
    //ページの移動するスピード
    [SerializeField] private float TurnSpeed = 3.0f;

    //オブジェクトの名前を取得する為のモノ
    public GameObject g_Page;

    //リストで制御するために動的配列を作る
    public List<GameObject> PageList = new List<GameObject>();

    //ページ数を決めるための変数(オブジェクトの数)
    private int m_nPageNum = 100;

    //リストの最初か最後を保存するための変数
    private GameObject SaveObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;i < m_nPageNum ;i++) {

            //スクリプトでオブジェクトを追加する
            GameObject Page = GameObject.Instantiate(g_Page) as GameObject;   //生成
            Page.transform.position = new Vector3(15.0f,0.0f,100.0f - i);       //座標
            Page.transform.localScale = new Vector3(100.0f,0.5f,100.0f);        //大きさ
            Page.transform.rotation = Quaternion.Euler(90, 0, 0);               //回転

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
            //ポインタ的な使い方(リストの先頭のオブジェクトを回避させる)
            SaveObject = PageList[0];
            //先頭のリストを削除する
            PageList.RemoveAt(0);

            //場所を変える
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f - (1* m_nPageNum));
            //リストの最後尾に追加する
            PageList.Add(SaveObject);

            //座標を変更できる用の変数を用意する
            Vector3 pos;

            //全体を少しずつ前に出す
            for (int i = 0;i < m_nPageNum;i++ ) {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position ;
                //少しずらす
                pos.z += 1.0f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }

        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //最後尾のリストの要素をポインタ的に当てはめる
            SaveObject = PageList[m_nPageNum - 1];
            //最後尾のリストのやつを削除する
            PageList.RemoveAt(m_nPageNum -1);

            //リストの先頭に追加する
            PageList.Insert(0, SaveObject);
            
            //場所を変える
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f);


            //オブジェクトの場所を保存する為の変数
            Vector3 pos;

            //ページ全体を後ろに下げる
            for (int i = m_nPageNum-1; i > 0;i--) {
                //変数にそれぞれの座標を代入する
                pos = PageList[i].transform.position;
                //少しずらす
                pos.z -= 1.0f;
                //ずらした結果を代入する
                PageList[i].transform.position = pos;
            }
        }

        //スペースボタンを押すと一番手前が消える
        if (Input.GetKeyDown(KeyCode.Space)){            
                //一ページ目から破いていきます
                Destroy(PageList[0]);
                PageList.RemoveAt(0);   
        }
    }
}
