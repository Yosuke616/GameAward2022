using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // シーン遷移用

public class StageSelect : MonoBehaviour
{
    // 他のスクリプトに出張する変数---------------------------------------------------
    public static int ProgressStages = 7;       // 現在の進捗
    public static bool bBackSelect = false;     // セレクトに帰ってきた時用フラグ
    public static bool bClearStage = false;     // クリアしてきたフラグ
    //--------------------------------------------------------------------------------

    
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
    //private SaveData saveData;          // ステージ毎の情報取得用

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
    private Text StageNo;               // パネルのステージNo用
    private Image[] star;               // ★表示用

    private float Base_Z = 850;         // パネル奥行調整用    
    private float LeftPanel = 0;        // 左パネル枚数
    private float RightPanel = 7;       // 右パネル枚数

    private bool CamZoom = false;       // カメラ移動フラグ
    private float zoomSpeed = 0.025f;    // カメラ移動速度   

    private int Select = 0;             // 現在選択       
    private int i;                      // ループ用変数    

    private bool bLoad = false;         // セーブデータロード用
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // ステージ選択パネル検索
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
        Stages = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };

        // ★部分を配列に
        star = new Image[] { Star1, Star2, Star3 };

        // ステージ情報パネル取得
        InfoPanel = GameObject.Find("StageInfoPanel");
        TimerData = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Timer").GetComponent<Text>();
        StageNo = GameObject.Find("Canvas").transform.Find("StageInfoPanel/StageNo").GetComponent<Text>();
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


        // テスト用
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoad.TestSaveLoad();
            ProgressStages = SaveLoad.saveData.Progress;
        }


        // ステージからセレクトに戻ってきたときの画面調整---------------------------------
        if (bBackSelect)
        {
            // 今クリアしたもの以外のパネルを右側に移動させる
            for (i = 0; i < ProgressStages - 1; i++)
            {
                Stages[i].transform.position = new Vector3(-Range_X,
                                                           Range_Y,
                                                           Base_Z - RightPanel - 1);

                LeftPanel++;
                RightPanel--;
                Select++;
            }

            // クリアしたステージを真ん中に
            Stages[i].transform.position = new Vector3(0, 0, 450);

            // クリアしてきた時に演出を出す・・・？
            if (bClearStage)
            {

                bClearStage = false;
            }

            bBackSelect = false;
        }
        //--------------------------------------------------------------------------------


        // ステージ選択-------------------------------------------------------------------
        if (PanelState == PANEL_STATE.NONE)
        {
            // ←画面移動
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
            {
                if (Select < ProgressStages)
                {
                    PanelState = PANEL_STATE.LEFT;

                    // 妖精さん左
                    FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.LEFT);

                    // ステージ情報見えないように
                    InfoPanel.SetActive(false);
                }
            }

            // →画面移動
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
            {
                if (Select > 0)
                {
                    PanelState = PANEL_STATE.RIGHT;

                    // 妖精さん右
                    FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.RIGHT);

                    // ステージ情報見えないように
                    InfoPanel.SetActive(false);
                }
            }
        }
        //--------------------------------------------------------------------------------


        // パネル移動---------------------------------------------------------------------
        switch (PanelState)
        {
            case PANEL_STATE.LEFT:
                // パネル左移動------------------------------------------------------------------
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
                if (Move_Z < Range_Z)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed);
                    Stages[Select + 1].transform.position -= new Vector3(0, 0, Speed);
                    Move_Z += Speed;
                }
                if (Move_X >= Range_X &&
                    Move_Y >= Range_Y &&
                    Move_Z >= Range_Z)
                {
                    Move_X = 0;
                    Move_Y = 0;
                    Move_Z = 0;
                    PanelState = PANEL_STATE.NONE;

                    Stages[Select].transform.position = new Vector3(Stages[Select].transform.position.x,
                                                                        Stages[Select].transform.position.y,
                                                                        Base_Z - LeftPanel - 1);
                    LeftPanel++;
                    RightPanel--;
                    Select++;
                }
                break;
                //--------------------------------------------------------------------------------


            case PANEL_STATE.RIGHT:
                // パネル右移動-------------------------------------------------------------------
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
                if (Move_Z < Range_Z)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed);
                    Stages[Select - 1].transform.position -= new Vector3(0, 0, Speed);
                    Move_Z += Speed;
                }
                if (Move_X >= Range_X &&
                    Move_Y >= Range_Y &&
                    Move_Z >= Range_Z)
                {
                    Move_X = 0;
                    Move_Y = 0;
                    Move_Z = 0;
                    PanelState = PANEL_STATE.NONE;

                    Stages[Select].transform.position = new Vector3(Stages[Select].transform.position.x,
                                                                        Stages[Select].transform.position.y,
                                                                        Base_Z - RightPanel - 1);

                    LeftPanel--;
                    RightPanel++;
                    Select--;
                }
                break;
                //--------------------------------------------------------------------------------


            case PANEL_STATE.NONE:
                // 通常時-------------------------------------------------------------------------
                // 妖精さん落ち着き
                if(!CamZoom)
                FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.NONE);

                // 背景変更
                ChangeBG.ChangeBg(Select);

                // ステージ情報表示
                InfoPanel.SetActive(true);
                TimerData.text = SaveLoad.saveData.Timer[Select];       // タイマー部分表示
                StageNo.text = "Stage　" + Select.ToString();           // ステージ名表示

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
                        //SceneManager.LoadScene("1-1");
                        FadeManager.Instance.FadeStart("1-1");
                        break;

                    case 1:
                        //SceneManager.LoadScene("1-2");
                        FadeManager.Instance.FadeStart("1-2");
                        break;

                    case 2:
                        //SceneManager.LoadScene("1-3");
                        FadeManager.Instance.FadeStart("1-3");
                        break;

                    case 3:
                        //SceneManager.LoadScene("1-4");
                        FadeManager.Instance.FadeStart("1-4");
                        break;

                    case 4:
                        //SceneManager.LoadScene("1-5");
                        FadeManager.Instance.FadeStart("1-5");
                        break;

                    case 5:
                        //SceneManager.LoadScene("1-6");
                        FadeManager.Instance.FadeStart("1-6");
                        break;

                    case 6:
                        //SceneManager.LoadScene("1-7");
                        FadeManager.Instance.FadeStart("1-7");
                        break;

                    case 7:
                        //SceneManager.LoadScene("1-8");
                        FadeManager.Instance.FadeStart("1-8");
                        break;

                }
            }
        }
        //--------------------------------------------------------------------------------
    }
}
