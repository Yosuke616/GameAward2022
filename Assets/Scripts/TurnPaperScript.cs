/*
 2022/3/1 志水陽祐 
 紙をめくるためのスクリプト
 
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
        //初期化で始まりの並びを決める
        for (int  i  = 0;i < 3 ;i++) {
            switch (i) {
                case 0:
                    m_Obj = GameObject.Find("Paper1Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper1Page").transform.position = new Vector3(15.0f, 0.0f, 100.0f);
                    //ページ番号を設定する

                    break;
                case 1:
                    m_Obj = GameObject.Find("Paper2Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper2Page").transform.position = new Vector3(15.0f, 0.0f, 99.0f);
                    //ページ番号を設定する

                    break;
                case 2:
                    m_Obj = GameObject.Find("Paper3Page");
                    //ここで場所を決定する
                    GameObject.Find("Paper3Page").transform.position = new Vector3(15.0f, 0.0f, 98.0f);
                    //ページ番号を設定する

                    break;
                default:break;
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
