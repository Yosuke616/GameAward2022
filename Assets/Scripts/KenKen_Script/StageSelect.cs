using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // シーン遷移用

public class StageSelect : MonoBehaviour
{
    //================================================================================
    // 他のスクリプトに出張する変数
    //================================================================================
    // 現在の進捗
    public static int ProgressStages = 7;

    // セレクトに帰ってきた時用フラグ
    public static bool bBackSelect = false;

    // クリアしてきたフラグ
    public static bool bClearStage = false;
    //================================================================================


    //================================================================================
    // パブリックでのパネル移動に関する設定
    //================================================================================
    // パネル移動時のスピード
    public float Speed;

    // パネル移動制御
    public float Range_X;
    public float Range_Y;
    public float Range_Z;

    // 移動量保存
    private float Move_X = 0;
    private float Move_Y = 0;
    private float Move_Z = 0;
    //================================================================================


    //================================================================================
    // スクリプト内での変数
    //================================================================================
    // パネル状態
    enum PANEL_STATE
    {
        LEFT,       // 左移動
        RIGHT,      // 右移動
        NONE        // 無し
    }
    private PANEL_STATE PanelState = PANEL_STATE.NONE;

    // 奥行調整用
    private float Base_Z = 850;

    // 左パネル枚数
    private float LeftPanel = 0;

    // 右パネル枚数
    private float RightPanel = 7;
   
    // ステージパネル配列
    private GameObject[] Stages;

    // ステージ情報表示パネル
    private GameObject InfoPanel;

    // パネルのステージNo用
    private Text StageNo;

    // 現在選択
    private int Select = 0;

    // ループ用変数
    private int i;
    //================================================================================


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

        // パネルを配列に
        Stages = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };

        // ステージ情報パネル取得
        InfoPanel = GameObject.Find("StageInfoPanel");
        StageNo = GameObject.Find("Canvas").transform.Find("StageInfoPanel/StageNo").GetComponent<Text>(); ;
        InfoPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //================================================================================
        // ステージからセレクトに戻ってきたときの画面調整
        if (bBackSelect)
        {
            // クリアしたステージ分のパネルを左側に移動させる
            for (i = 0; i < ProgressStages; i++)
            {
                Stages[i].transform.position = new Vector3(-Range_X,
                                                           Range_Y,
                                                           Base_Z - RightPanel - 1);

                LeftPanel++;
                RightPanel--;
                Select++;
            }

            // 次挑戦するクリアしていないステージを真ん中に
            Stages[i].transform.position = new Vector3(0, 0, 450);

            // クリアしてきた時に演出を出す・・・？
            if(bClearStage)
            {

                bClearStage = false;
            }

            bBackSelect = false;
        }
        //================================================================================


        //================================================================================
        // ステージ選択
        if (PanelState == PANEL_STATE.NONE)
        {
            // ←画面移動
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0) 
            {
                if (Select < ProgressStages)
                {
                    PanelState = PANEL_STATE.LEFT;
                    InfoPanel.SetActive(false);
                }
            }

            // →画面移動
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
            {
                if (Select > 0)
                {
                    PanelState = PANEL_STATE.RIGHT;
                    InfoPanel.SetActive(false);
                }
            }
        }
        //================================================================================


        //================================================================================
        // パネル移動
        switch (PanelState)
        {
            case PANEL_STATE.LEFT:
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

            case PANEL_STATE.RIGHT:
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

            case PANEL_STATE.NONE:
                // ステージ情報表示
                InfoPanel.SetActive(true);
                StageNo.text = "Stage　" + Select.ToString();
                break;
        }
        //================================================================================


        //================================================================================
        // ステージ移行
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            // セレクトに戻った時用のフラグON
            bBackSelect = true;

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
        //================================================================================
    }


    //================================================================================
    // ステージクリア時に進捗保存関数
    public static void UpdateProgress(string name)
    {
        if (name == "1-1") 
        {
            if (ProgressStages <= 1)
                ProgressStages = 1;
        }

        if (name == "1-2")
        {
            if (ProgressStages <= 2)
                ProgressStages = 2;
        }

        if (name == "1-3")
        {
            if (ProgressStages <= 3)
                ProgressStages = 3;
        }

        if (name == "1-4")
        {
            if (ProgressStages <= 4)
                ProgressStages = 4;
        }

        if (name == "1-5")
        {
            if (ProgressStages <= 5)
                ProgressStages = 5;
        }

        if (name == "1-6")
        {
            if (ProgressStages <= 6)
                ProgressStages = 6;
        }

        if (name == "1-6")
        {
            if (ProgressStages <= 6)
                ProgressStages = 6;
        }

        if (name == "1-7")
        {
            if (ProgressStages <= 7)
                ProgressStages = 7;
        }

        bClearStage = true;

        //if (name == "1-8")
        //{
        //    if (ProgressStages <= 8)
        //        ProgressStages = 8;
        //}
    }
    //================================================================================
}
