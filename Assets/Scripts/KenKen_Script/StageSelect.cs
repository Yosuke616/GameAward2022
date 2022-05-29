using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // シーン遷移用
using System;


public class StageSelect : MonoBehaviour
{
    // パブリックでのパネル移動に関する設定-------------------------------------------
    public Camera MainCam;       // メインカメラ取得
    public Vector3 TargetPos;    // カメラ移動位置
    public float Speed;          // パネル移動時のスピード

    // パネル移動制御
    public float Range_X;
    public float Range_Y;
    public float Range_Z;

    // 移動量保存
    private float Move_X = 0;
    private float Move_Y = 0;
    private float Move_Z = 0;
    //--------------------------------------------------------------------------------


    // スクリプト内での変数-----------------------------------------------------------
    private int ProgressStages = 0;       // 現在の進捗
    private bool bBackSelect = false;     // セレクトに帰ってきた時用フラグ

    // パネル状態
    enum PANEL_STATE    // パネル状態
    {
        LEFT,       // 左移動
        RIGHT,      // 右移動
        NONE        // 無し
    }
    private PANEL_STATE PanelState = PANEL_STATE.NONE;

    private GameObject[] Stages;        // ステージパネル配列 
    private GameObject InfoPanel;       // ステージ情報表示パネル
    private Text TimerData;             // クリアタイム
    private Image[] star;               // ★表示用

    // パネル調整用
    private float BaseFront_X = 0;
    private float BaseFront_Y = 0;
    private float BaseFront_Z = 450;

    // パネル奥行調整用
    private float BaseFar_X = 800;        
    private float BaseFar_Y = 250;
    private float BaseFar_Z = 850;   

    private float LeftPanel = 0;        // 左パネル枚数
    private float RightPanel = 8;       // 右パネル枚数

    private bool CamZoom = false;       // カメラ移動フラグ
    private float zoomSpeed = 0.075f;    // カメラ移動速度   

    private int Select = 0;             // 現在選択       
    private int i;                      // ループ用変数    

    private bool bLoad = false;         // セーブデータロード用
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // 60FPS設定
        Application.targetFrameRate = 60;

        // ステージ選択パネル検索
        GameObject Stage0 = GameObject.Find("Tutorial");
        GameObject Stage1 = GameObject.Find("1-1");
        GameObject Stage2 = GameObject.Find("1-2");
        GameObject Stage3 = GameObject.Find("1-3");
        GameObject Stage4 = GameObject.Find("1-4");
        GameObject Stage5 = GameObject.Find("1-5");
        GameObject Stage6 = GameObject.Find("1-6");
        GameObject Stage7 = GameObject.Find("1-7");
        GameObject Stage8 = GameObject.Find("1-8");

        // ★部分取得
        Image Star1 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star1").GetComponent<Image>();
        Image Star2 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star2").GetComponent<Image>();
        Image Star3 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star3").GetComponent<Image>();

        // パネルを配列に
        Stages = new GameObject[] { Stage0, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };

        // ★部分を配列に
        star = new Image[] { Star1, Star2, Star3 };

        // ステージ情報パネル取得
        InfoPanel = GameObject.Find("StageInfoPanel");
        TimerData = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Timer").GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        // セーブデータロード-------------------------------------------------------------
        if (bLoad == false)
        {
            SaveLoad.LoadData();
            ProgressStages = SaveLoad.saveData.Progress;
            bLoad = true;
        }
        //--------------------------------------------------------------------------------


        // ポーズ中
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }


        // テスト用
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveLoad.TestSaveLoad();
        //    ProgressStages = SaveLoad.saveData.Progress;
        //}


        // ステージからセレクトに戻ってきたときの画面調整---------------------------------
        if (bBackSelect == false)
        {
            // 今クリアしたもの以外のパネルを左側に移動させる
            for (i = 0; i < ProgressStages - 1; i++)
            {
                Stages[i].transform.position = new Vector3(-Range_X,
                                                           Range_Y,
                                                           BaseFar_Z - LeftPanel);

                LeftPanel++;
                RightPanel--;
                Select++;
            }

            // クリアしたステージを真ん中に
            Stages[i].transform.position = new Vector3(0, 0, 450);

            // フラグON
            bBackSelect = true;
        }
        //--------------------------------------------------------------------------------


        // ステージ選択-------------------------------------------------------------------
        if (PanelState == PANEL_STATE.NONE)
        {
            // カメラのズームしていない時のみ実行
            if (CamZoom == false)
            {
                // ←画面移動
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 5"))
                {
                    if (Select < ProgressStages)
                    {
                        SoundManager.Instance.PlaySeByName("SE_MenuOperation");

                        PanelState = PANEL_STATE.LEFT;

                        // 妖精さん左
                        FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.LEFT);

                        // ステージ情報見えないように
                        InfoPanel.SetActive(false);

                           
                    }
                }

                // →画面移動
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 4"))
                {
                    if (Select > 0)
                    {
                        SoundManager.Instance.PlaySeByName("SE_MenuOperation");

                        PanelState = PANEL_STATE.RIGHT;

                        // 妖精さん右
                        FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.RIGHT);

                        // ステージ情報見えないように
                        InfoPanel.SetActive(false);

                    }
                }
            }
        }
        //--------------------------------------------------------------------------------


        // パネル移動---------------------------------------------------------------------
        switch (PanelState)
        {
            case PANEL_STATE.LEFT:
                // パネル左移動------------------------------------------------------------------
                // 妖精さんが左まできたら
                if (FairyMoveSelect.LeftRight == false)
                {
                    break;
                }

                // 移動
                if (Move_X < Range_X)
                {
                    Stages[Select].transform.position -= new Vector3(Speed, 0, 0);
                    Stages[Select + 1].transform.position -= new Vector3(Speed, 0, 0);
                    Move_X += Speed;
                }
                if (Move_Y < Range_Y)
                {
                    Stages[Select].transform.position += new Vector3(0, Speed, 0);
                    Stages[Select + 1].transform.position -= new Vector3(0, Speed, 0);
                    Move_Y += Speed;
                }
                if (Move_Z < Range_Z - LeftPanel)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed);
                    Stages[Select + 1].transform.position -= new Vector3(0, 0, Speed);
                    Move_Z += Speed;     
                }

                // 奥行調整
                if (Stages[Select].transform.position.z > BaseFar_Z - LeftPanel)
                {
                    Stages[Select].transform.position = new Vector3(Stages[Select].transform.position.x,
                                                                    Stages[Select].transform.position.y,
                                                                    BaseFar_Z - LeftPanel);
                }

                if (Move_X >= Range_X &&
                    Move_Y >= Range_Y &&
                    Move_Z >= Range_Z - LeftPanel)
                {
                    Move_X = 0;
                    Move_Y = 0;
                    Move_Z = 0;
                    PanelState = PANEL_STATE.NONE;

                    // 位置調整
                    Stages[Select].transform.position = new Vector3(-BaseFar_X,
                                                                    BaseFar_Y,
                                                                    BaseFar_Z - LeftPanel);

                    Stages[Select + 1].transform.position = new Vector3(BaseFront_X,
                                                                           BaseFront_Y,
                                                                           BaseFront_Z);

                    LeftPanel++;
                    RightPanel--;
                    Select++;

                    // 背景変更
                    ChangeBG.ChangeBg(Select);
                }
                break;
                //--------------------------------------------------------------------------------


            case PANEL_STATE.RIGHT:
                // パネル右移動-------------------------------------------------------------------
                // 妖精さんが右まできたら
                if (FairyMoveSelect.LeftRight == false)
                {
                    break;
                }

                // 移動
                if (Move_X < Range_X)
                {
                    Stages[Select].transform.position += new Vector3(Speed, 0, 0);
                    Stages[Select - 1].transform.position += new Vector3(Speed, 0, 0);
                    Move_X += Speed;
                }
                if (Move_Y < Range_Y)
                {
                    Stages[Select].transform.position += new Vector3(0, Speed, 0);
                    Stages[Select - 1].transform.position -= new Vector3(0, Speed, 0);
                    Move_Y += Speed;
                }
                if (Move_Z < Range_Z - RightPanel)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed);
                    Stages[Select - 1].transform.position -= new Vector3(0, 0, Speed);
                    Move_Z += Speed;
                }

                // 奥行調整
                if (Stages[Select].transform.position.z > BaseFar_Z - RightPanel)
                {
                    Stages[Select].transform.position = new Vector3(Stages[Select].transform.position.x,
                                                                    Stages[Select].transform.position.y,
                                                                    BaseFar_Z - RightPanel);
                }

                if (Move_X >= Range_X &&
                    Move_Y >= Range_Y &&
                    Move_Z >= Range_Z - RightPanel)
                {
                    Move_X = 0;
                    Move_Y = 0;
                    Move_Z = 0;
                    PanelState = PANEL_STATE.NONE;

                    // 位置調整
                    Stages[Select].transform.position = new Vector3(BaseFar_X,
                                                                    BaseFar_Y,
                                                                    BaseFar_Z - RightPanel);

                    Stages[Select - 1].transform.position = new Vector3(BaseFront_X,
                                                                           BaseFront_Y,
                                                                           BaseFront_Z);

                    LeftPanel--;
                    RightPanel++;
                    Select--;

                    // 背景変更
                    ChangeBG.ChangeBg(Select);
                }
                break;
                //--------------------------------------------------------------------------------


            case PANEL_STATE.NONE:
                // 通常時-------------------------------------------------------------------------
                // 妖精さん落ち着き
                if(!CamZoom)
                FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.NONE);

                // ステージ情報表示
                InfoPanel.SetActive(true);
                TimerData.text = SaveLoad.saveData.Timer[Select];       // タイマー部分表示
                StageNameChange.ChangeStageName(Select);                // ステージ名変更


                for (i = 0; i < SaveLoad.saveData.Star[Select]; i++)    // ★部分表示
                {
                    star[i].enabled = true;
                }
                for (; i < 3; i++)
                {
                    star[i].enabled = false;
                }
                break;
                //--------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------


        // カメラ移動(ステージ移行)-------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            SoundManager.Instance.PlaySeByName("RipUpPaper07");

            // カメラ移動フラグON
            CamZoom = true;

            // 妖精さん真ん中に
            FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.CENTER);
        }

        // ステージ移動
        if (CamZoom)
        {
            // 距離の差を計算
            float diffX = TargetPos.x - MainCam.transform.position.x;
            float diffY = TargetPos.y - MainCam.transform.position.y;
            float diffZ = TargetPos.z - MainCam.transform.position.z;

            // 移動量計算
            Vector3 moveValue = new Vector3(diffX, diffY, diffZ) * zoomSpeed;

            // 移動
            MainCam.transform.Translate(moveValue);

            // ズームが終了したら
            if (diffX < 0.001f && diffY < 0.001f && diffZ < 0.001f)
            {
                CamZoom = false;

                // ステージ情報表示無し
                InfoPanel.SetActive(false);

                switch (Select)
                {
                    case 0:
                        FadeManager.Instance.FadeStart("Tutorial");
                        break;

                    case 1:
                        //SceneManager.LoadScene("1-1");
                        FadeManager.Instance.FadeStart("1-1");
                        break;

                    case 2:
                        //SceneManager.LoadScene("1-2");
                        FadeManager.Instance.FadeStart("1-2");
                        break;

                    case 3:
                        //SceneManager.LoadScene("1-3");
                        FadeManager.Instance.FadeStart("1-3");
                        break;

                    case 4:
                        //SceneManager.LoadScene("1-4");
                        FadeManager.Instance.FadeStart("1-4");
                        break;

                    case 5:
                        //SceneManager.LoadScene("1-5");
                        FadeManager.Instance.FadeStart("1-5");
                        break;

                    case 6:
                        //SceneManager.LoadScene("1-6");
                        FadeManager.Instance.FadeStart("1-6");
                        break;

                    case 7:
                        //SceneManager.LoadScene("1-7");
                        FadeManager.Instance.FadeStart("1-7");
                        break;

                    case 8:
                        //SceneManager.LoadScene("1-8");
                        FadeManager.Instance.FadeStart("1-8");
                        break;

                }
            }
        }
        //--------------------------------------------------------------------------------
    }
}
