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

    //ゲームオーバーの画像を出すためのやつ
    public Image _GameOverBG;

    //何かい点滅指せるかは個々の変数で決める
    private int MaxBlink;

    //点滅させるための変数
    private float alpha_Num;

    private bool Iine = true;

    int nCnt;

    //ボタンを追加しておく
    public Image Select;
    public Image Retry;
    public Image Title;

    //ボタンの数
    private int nMaxButton = 2;
    private int SelectButton = 0;

    //コントローラーの制御用
    private int nCnt2;

    //選択できるかのフラグ
    private bool Optionflg;

    private bool SE;

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

        //初めは何にも動かせない
        Optionflg = false;

        nCnt2 = 0;

        SE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Optionflg)
        {
            if (g_bGameOverFlg)
            {

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

                else
                {
                    //if (GO_Tex != null) GO_Tex.text = "　　　　失敗！";

                    if (!SE) {
                        SoundManager.Instance.StopBgm();
                        SoundManager.Instance.PlaySeByName("jingle37");
                        SE = true;
                    }

                    _GameOverBG.gameObject.SetActive(true);
                    Optionflg = true;
                }

            }
        }
        else
        {
            nCnt2--;

            if (nCnt2 < 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
                {
                    nCnt2 = 10;
                    SelectButton--;
                    if (SelectButton < 0)
                    {
                        SelectButton = nMaxButton;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
                {
                    nCnt2 = 10;
                    SelectButton++;
                    if (SelectButton > nMaxButton)
                    {
                        SelectButton = 0;
                    }
                }
            }

          
            //常に白に変えていく
            Select.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Retry.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Title.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            switch (SelectButton)
            {
                case 0:
                   
                    //Select.Select();
                    Select.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 1:

                    //Retry.Select();
                    Retry.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 2:

                    //Title.Select();
                    Title.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
            }

            //ボタンを押せるかどうかを判別する
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
            {
                Debug.Log("入ったよー");
                switch (SelectButton)
                {
                    case 0: OnSelect(); break;
                    case 1: OnRetry(); break;
                    case 2: OnTitle(); break;
                }
            }

        }
    }

    //敵にぶつかったかどうかのフラグを得る
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }


    public void OnSelect()
    {
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");
    }

    public void OnRetry()
    {
        // 同一シーンを読込
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitle()
    {
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");
    }


}
