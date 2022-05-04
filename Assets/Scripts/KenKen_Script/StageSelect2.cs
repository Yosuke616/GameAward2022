using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // �V�[���J�ڗp

public class StageSelect2 : MonoBehaviour
{
    // 現在の進捗
    public int ProgressStages = 4;

    // ステージ選択パネル
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject Stage4;
    public GameObject Stage5;
    public GameObject Stage6;
    public GameObject Stage7;
    public GameObject Stage8;

    private GameObject[] Stages;

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

    // 奥行調整用
    private float Base_Z = 850;

    // 左パネル枚数
    private float LeftPanel = 7;

    // 右パネル枚数
    private float RightPanel = 0;

    // パネル状態
    enum PANEL_STATE
    {
        LEFT,
        RIGHT,
        NONE
    }
    private PANEL_STATE PanelState = PANEL_STATE.NONE;

    // 現在選択
    private int Select = 0;



    // Start is called before the first frame update
    void Start()
    {
        // ステージパネルを配列に
        Stages = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };
    }

    // Update is called once per frame
    void Update()
    {
        // ステージ選択
        if (PanelState == PANEL_STATE.NONE)
        {
            // ←画面移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Select > 0)
                {
                    PanelState = PANEL_STATE.LEFT;
                }
            }

            // →画面移動
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Select < ProgressStages)
                {
                    PanelState = PANEL_STATE.RIGHT;
                }
            }
        }

        // パネル移動
        switch (PanelState)
        {
            case PANEL_STATE.LEFT:
                if (Move_X < Range_X)
                {
                    Stages[Select].transform.position -= new Vector3(Speed, 0, 0);
                    Stages[Select - 1].transform.position -= new Vector3(Speed, 0, 0);
                    Move_X += Speed;
                }
                if (Move_Y < Range_Y)
                {
                    Stages[Select].transform.position += new Vector3(0, Speed / 2, 0);
                    Stages[Select - 1].transform.position -= new Vector3(0, Speed / 2, 0);
                    Move_Y += Speed / 2;
                }
                if (Move_Z < Range_Z)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed / 2);
                    Stages[Select - 1].transform.position -= new Vector3(0, 0, Speed / 2);
                    Move_Z += Speed / 2;
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
                                                                        Base_Z - LeftPanel);
                    LeftPanel++;
                    RightPanel--;
                    Select--;
                }
                break;

            case PANEL_STATE.RIGHT:
                if (Move_X < Range_X)
                {
                    Stages[Select].transform.position += new Vector3(Speed, 0, 0);
                    Stages[Select + 1].transform.position += new Vector3(Speed, 0, 0);
                    Move_X += Speed;
                }
                if (Move_Y < Range_Y)
                {
                    Stages[Select].transform.position += new Vector3(0, Speed / 2, 0);
                    Stages[Select + 1].transform.position -= new Vector3(0, Speed / 2, 0);
                    Move_Y += Speed / 2;
                }
                if (Move_Z < Range_Z)
                {
                    Stages[Select].transform.position += new Vector3(0, 0, Speed / 2);
                    Stages[Select + 1].transform.position -= new Vector3(0, 0, Speed / 2);
                    Move_Z += Speed / 2;
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
                                                                        Base_Z - RightPanel);

                    LeftPanel--;
                    RightPanel++;
                    Select++;
                }
                break;

            case PANEL_STATE.NONE:
                break;
        }


        // ステージ移行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (Select)
            {
                case 0:
                    SceneManager.LoadScene("1-1");
                    break;

                case 1:
                    SceneManager.LoadScene("1-2");
                    break;

                case 2:
                    SceneManager.LoadScene("1-3");
                    break;

                case 3:
                    SceneManager.LoadScene("1-4");
                    break;
            }
        }
    }


    // �X�e�[�W�N���A���ɐi���ۑ��֐�
    //public static void UpdateProgress2(string name)
    //{
    //    if (name == "1-1")
    //    {
    //        if (ProgressStage <= 1)
    //            ProgressStage = 1;
    //    }

    //    if (name == "1-2")
    //    {
    //        if (ProgressStage <= 2)
    //            ProgressStage = 2;
    //    }

    //    if (name == "1-3")
    //    {
    //        if (ProgressStage <= 3)
    //            ProgressStage = 3;
    //    }

    //    if (name == "1-4")
    //    {
    //        if (ProgressStage <= 4)
    //            ProgressStage = 4;
    //    }
    //}
}
