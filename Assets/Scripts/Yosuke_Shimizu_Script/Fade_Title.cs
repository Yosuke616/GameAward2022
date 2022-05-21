using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fade_Title : MonoBehaviour
{
    public GameObject St_Button;
    public GameObject Ct_Button;
    public GameObject Op_Button;
    public GameObject En_Button;
    public GameObject Title_Log;
    
    //黒
    public GameObject black;

    //タイトルのフェード
    private byte nTitleCnt;

    //ボタンのフェード
    private byte nButtonCnt;

    //箱を透明にするカウント
    private byte nBlackCnt;

    //順番にフェードさせる
    private bool Jun;

    // Start is called before the first frame update
    void Start()
    {
        St_Button.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
        Ct_Button.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
        Op_Button.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
        En_Button.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
        Title_Log.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);

        black.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,255);

        nTitleCnt = 0;
        nButtonCnt = 0;
        nBlackCnt = 255;

        Jun = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Jun)
        {
            if (nTitleCnt < 255)
            {
                nTitleCnt++;
                Title_Log.GetComponent<SpriteRenderer>().color = new Color32(nTitleCnt, nTitleCnt, nTitleCnt, nTitleCnt);
            }

            //ボタンをフェードする
            if (nTitleCnt >= 60 && nButtonCnt < 255)
            {
                nButtonCnt++;
                St_Button.GetComponent<SpriteRenderer>().color = new Color32(nButtonCnt, nButtonCnt, nButtonCnt, nButtonCnt);
                Ct_Button.GetComponent<SpriteRenderer>().color = new Color32(nButtonCnt, nButtonCnt, nButtonCnt, nButtonCnt);
                Op_Button.GetComponent<SpriteRenderer>().color = new Color32(nButtonCnt, nButtonCnt, nButtonCnt, nButtonCnt);
                En_Button.GetComponent<SpriteRenderer>().color = new Color32(nButtonCnt, nButtonCnt, nButtonCnt, nButtonCnt);
            }

            if (nButtonCnt == 255)
            {
                Debug.Log("ロゼッタ様");
                GameObject camera = GameObject.Find("Main Camera");
                camera.GetComponent<Title_Button_Script>().SetStartFlg(true);
            }
        }
        //タイトルをフェード指せる
        else {
            if (nBlackCnt >= 0) {
                nBlackCnt--;
                black.GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, nBlackCnt);

                if (nBlackCnt == 0) {
                    Jun = true;
                }
            }
        }
    }
}
