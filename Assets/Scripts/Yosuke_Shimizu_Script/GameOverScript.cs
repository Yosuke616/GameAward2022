using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    //敵にぶつかってしまったかどうかのフラグ
    private bool g_bGameOverFlg;

    //点滅させます
    private bool g_bBlinking;

    //文字のやつ
    public Text GO_Tex;

    //ゲームオーバーの画像を出すためのやつ
    public Image _GameOverBG;

    //何かい点滅指せるかは個々の変数で決める
    private int MaxBlink;

    //点滅させるための変数
    private float alpha_Num;

    private bool Iine;

    int nCnt;

    // Start is called before the first frame update
    void Start()
    {
        //初めは入らないようにする
        g_bGameOverFlg = false;

        //最初は非アクティブ
        _GameOverBG.gameObject.SetActive(false);

        //点滅させるために最初はオンにする
        g_bBlinking = true;

        MaxBlink = 10;

        nCnt = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (g_bGameOverFlg) {

            nCnt--;

            //点滅させてから画面を暗くする
            if (g_bBlinking)
            {
                GameObject camera = GameObject.Find("MainCamera");

                if (Iine && nCnt < 0)
                {
                    camera.GetComponent<Blink_Script>().Blink(Iine);

                    Iine = false;
                    MaxBlink--;
                    nCnt = 10;
                   
                }
                else if (!Iine && nCnt < 0)
                {
                    camera.GetComponent<Blink_Script>().Blink(Iine);

                    Iine = true;
                    MaxBlink--;
                    nCnt = 10;
                }

                if (MaxBlink < 0)
                {
                    camera.GetComponent<Blink_Script>().LastBlink();
                    g_bBlinking = false;
                }
            }

            else {
                if (GO_Tex != null) GO_Tex.text = "　　　　失敗！";

                SoundManager.Instance.StopBgm();
                SoundManager.Instance.PlaySeByName("jingle37");

                _GameOverBG.gameObject.SetActive(true);
            }
           
        }
    }

    //敵にぶつかったかどうかのフラグを得る
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }

}
