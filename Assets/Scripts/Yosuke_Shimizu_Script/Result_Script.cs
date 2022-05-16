using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Result_Script : MonoBehaviour
{
    //クリアしたかどうかのフラグ
    private bool g_bGoal;

    //よる場所
    private Vector3 targetPos = new Vector3(3.5f, -1.5f, -5.0f);

    //寄っていくスピード
    private float m_fSpeed = 0.5f;

    //ゆっくりさせる
    private float m_fYukkuri = 0.25f;

    //計算用変数
    private Vector3 m_fvelocity;

    //文字を出すためのやつ
    public Text tex;
    public Text timerTex;

    //使用する画像
    public Image _resultBG;

    //星の画像
    public Image Star_1;
    public Image Star_2;
    public Image Star_3;

    //タイムを記録する人
    private float Timer;


    //ボタンを追加しておく
    public Button Select;
    public Button Retry;
    public Button Title;

    //ボタンの数
    private int nMaxButton = 2;
    private int SelectButton = 0;

    //コントローラーの制御用
    private int nCnt;

    //選択できるかのフラグ
    private bool Optionflg;

    // Start is called before the first frame update
    void Start()
    {
        //初めは入ら無いようにする
        g_bGoal = false;

        //アクティブにしない
        _resultBG.gameObject.SetActive(false);

        //星はデフォルトではアクティブにしない
        Star_1.gameObject.SetActive(false);
        Star_2.gameObject.SetActive(false);
        Star_3.gameObject.SetActive(false);

        //初めは何にも動かせない
        Optionflg = false;

        nCnt = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //後でボタンを触れるようにする
        if (!Optionflg)
        {

            GameObject GO = GameObject.Find("ParentPlayer");

            if (!g_bGoal && GO.GetComponent<PlayerMove2>().GetGameOverFlg())
            {
                //時間を取得してある程度早かったら星の画像を出す
                GameObject Time = GameObject.Find("Timer");

                Timer = Time.GetComponent<TimerScript>().GetTime();
            }

            if (g_bGoal)
            {
                //カメラがよる
                GameObject Camera = GameObject.Find("MainCamera");


                m_fvelocity += ((targetPos - Camera.transform.position) * m_fSpeed);

                m_fvelocity *= m_fYukkuri;

                Camera.transform.position += m_fvelocity;

                if (Camera.transform.position == targetPos)
                {

                    if (tex && timerTex) tex.text = "クリアタイム:" + timerTex.text;

                    SoundManager.Instance.StopBgm();
                    SoundManager.Instance.PlaySeByName("clear");

                    _resultBG.gameObject.SetActive(true);

                    if (Timer < 30.0f)
                    {
                        Star_1.gameObject.SetActive(true);
                        Star_2.gameObject.SetActive(true);
                    }

                    Optionflg = true;

                }
                //Camera.transform.position = new Vector3(3.5f,-1.5f,-5.0f);
            }
        }
        else {
            nCnt--;

            if (nCnt < 0) {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) {
                    nCnt = 10;
                    SelectButton--;
                    if (SelectButton < 0) {
                        SelectButton = nMaxButton;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) {
                    nCnt = 10;
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

            switch (SelectButton) {
                case 0:
                    Select.Select();
                    Select.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 1:
                    Retry.Select();
                    Retry.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 2:
                    Title.Select();
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

    //クリアしたかどうかのフラグを得る
    public void SetGoalFlg(bool GoalFlg) {
        g_bGoal = GoalFlg;
    }

    public void OnSelect()
    {
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");
        
    }

    public void OnRetry() {
        
        // 同一シーンを読込
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitle() {
        // 同一シーンを読込
        SceneManager.LoadScene("StageSelect");   
    }

}
