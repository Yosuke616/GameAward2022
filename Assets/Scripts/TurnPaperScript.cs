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
    private GameObject m_Obj;

    // Start is called before the first frame update
    void Start()
    {
        //リストで制御するために動的配列を作る
        List<GameObject> PageList = new List<GameObject>();

        //初期化で始まりの並びを決める
        for (int  i  = 0;i < 3 ;i++) {
            switch (i) {
                case 0:
                    m_Obj = GameObject.Find("Paper1Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper1Page").transform.position = new Vector3(15.0f, 0.0f, 100.0f);
                    //ページ番号を設定する(リストで追加していく)
                    PageList.Add(m_Obj);

                    break;
                case 1:
                    m_Obj = GameObject.Find("Paper2Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper2Page").transform.position = new Vector3(15.0f, 0.0f, 99.0f);
                    //ページ番号を設定する
                    PageList.Add(m_Obj);
                    break;
                case 2:
                    m_Obj = GameObject.Find("Paper3Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper3Page").transform.position = new Vector3(15.0f, 0.0f, 98.0f);
                    //ページ番号を設定する
                    PageList.Add(m_Obj);
                    break;
                default:break;
            }
        }

        foreach (GameObject i in PageList) {
            Debug.Log(i.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
