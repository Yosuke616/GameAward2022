using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_Scrupt : MonoBehaviour
{
    //ボタンの種類を追加しておく
    public Button First;
    public Button Countinue;
    public Button Option;
    public Button End;

    //ボタンの数
    private int nMaxButton = 3;
    private int nSelectButton = 0;

    //コントローラーの使用制限
    private int nCnt;

    //動かせるタイミングを決める
    private bool Titleflg;

    // Start is called before the first frame update
    void Start()
    {
        //コントローラーを動かす為の時間
        nCnt = 0;

        //タイトルを動かすためのフラグ
        Titleflg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Titleflg) {
            nCnt--;

            if (nCnt < 0) {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
                {
                    nCnt = 90;
                    nSelectButton--;
                    if (nSelectButton < 0)
                    {
                        nSelectButton = nMaxButton;
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
                {
                    nCnt = 90;
                    nSelectButton++;
                    if (nSelectButton > nMaxButton)
                    {
                        nSelectButton = 0;
                    }
                }
            }

            //常に白に変えていく
            First.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Countinue.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Option.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            End.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            switch (nSelectButton) {
                case 0:
                    First.Select();
                    First.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 1:
                    Countinue.Select();
                    Countinue.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 2:
                    Option.Select();
                    Option.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 3:
                    End.Select();
                    End.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                default:break;

            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1")) {
                switch (nSelectButton) {
                    case 0:OnFirst(); break;
                    case 1:OnCountinue(); break;
                    case 2:OnOption(); break;
                    case 3:OnEnd(); break;

                    default:break;
                }
            }

        }
    }

    //初めから
    public void OnFirst() {
        Debug.Log("ロゼッター");
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");
    }

    //続きから
    public void OnCountinue() {
        Debug.Log("ロゼッタかわいいー");
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");
    }

    //オプション
    public void OnOption() {
        Debug.Log("ロゼッタ様抱いてー！");

        Titleflg = true;
        GameObject obj = GameObject.Find("OptionTitle");
        obj.GetComponent<TitleOption_Script>().SetTitleOption(false);
    }

    //終わる
    public void OnEnd() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void SetTitleFlg(bool TF) {
        Titleflg = TF;
    }

}
