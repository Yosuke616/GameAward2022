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

    //ボタンを押したら最初の演出をスキップできるようにする
    private bool IkkaiSkip;

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

        //初めはスキップできるぜ
        IkkaiSkip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Valume.First_Title == false)
        {
            if (Jun)
            {
                if (nTitleCnt < 255)
                {
                    if (nTitleCnt > 250)
                    {
                        nTitleCnt++;
                    }
                    else {
                        nTitleCnt += 4;
                    }

                    Title_Log.GetComponent<SpriteRenderer>().color = new Color32(nTitleCnt, nTitleCnt, nTitleCnt, nTitleCnt);
                }

                //ボタンをフェードする
                if (nTitleCnt >= 60 && nButtonCnt < 255)
                {
                    if (nButtonCnt > 250)
                    {
                        Debug.Log(nButtonCnt);
                        Debug.Log(nTitleCnt);
                        nButtonCnt++;
                    }
                    else {
                        nButtonCnt+=4;
                    }

                    St_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, nButtonCnt);
                    Ct_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, nButtonCnt);
                    Op_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, nButtonCnt);
                    En_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, nButtonCnt);
                }

                if (nButtonCnt >= 255)
                {
                    Debug.Log("ロゼッタ様");
                    GameObject camera = GameObject.Find("Main Camera");
                    camera.GetComponent<Title_Button_Script>().SetStartFlg(true);
                    Valume.First_Title = true;
                }
            }
            //タイトルをフェード指せる
            else
            {
                if (nBlackCnt >= 0)
                {
                    if (nBlackCnt < 5)
                    {
                        nBlackCnt--;
                    }
                    else {
                        nBlackCnt -= 4;
                    }
                    black.GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, nBlackCnt);

                    if (nBlackCnt == 0)
                    {
                        Jun = true;
                    }
                }
            }

            //ボタンを押したらスキップするようにする
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1"))
            {
                black.GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, 0);
                St_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                Ct_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                Op_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                En_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                Title_Log.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                GameObject camera = GameObject.Find("Main Camera");
                camera.GetComponent<Title_Button_Script>().SetStartFlg(true);
                Valume.First_Title = true;
            }

        }
        else {
            black.GetComponent<SpriteRenderer>().material.color = new Color32(0, 0, 0, 0);
            St_Button.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
            Ct_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            Op_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            En_Button.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            Title_Log.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<Title_Button_Script>().SetStartFlg(true);

        }       

    }
}
