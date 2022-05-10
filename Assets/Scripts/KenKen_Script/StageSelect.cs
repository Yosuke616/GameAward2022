using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // �V�[���J�ڗp

public class StageSelect : MonoBehaviour
{
    //================================================================================
    // ���̃X�N���v�g�ɏo������ϐ�
    //================================================================================
    // ���݂̐i��
    public static int ProgressStages = 7;

    // �Z���N�g�ɋA���Ă������p�t���O
    public static bool bBackSelect = false;

    // �N���A���Ă����t���O
    public static bool bClearStage = false;
    //================================================================================


    //================================================================================
    // �p�u���b�N�ł̃p�l���ړ��Ɋւ���ݒ�
    //================================================================================
    // �p�l���ړ����̃X�s�[�h
    public float Speed;

    // �p�l���ړ�����
    public float Range_X;
    public float Range_Y;
    public float Range_Z;

    // �ړ��ʕۑ�
    private float Move_X = 0;
    private float Move_Y = 0;
    private float Move_Z = 0;
    //================================================================================


    //================================================================================
    // �X�N���v�g���ł̕ϐ�
    //================================================================================
    // �p�l�����
    enum PANEL_STATE
    {
        LEFT,       // ���ړ�
        RIGHT,      // �E�ړ�
        NONE        // ����
    }
    private PANEL_STATE PanelState = PANEL_STATE.NONE;

    // ���s�����p
    private float Base_Z = 850;

    // ���p�l������
    private float LeftPanel = 0;

    // �E�p�l������
    private float RightPanel = 7;
   
    // �X�e�[�W�p�l���z��
    private GameObject[] Stages;

    // �X�e�[�W���\���p�l��
    private GameObject InfoPanel;

    // �p�l���̃X�e�[�WNo�p
    private Text StageNo;

    // ���ݑI��
    private int Select = 0;

    // ���[�v�p�ϐ�
    private int i;
    //================================================================================


    // Start is called before the first frame update
    void Start()
    {
        // �X�e�[�W�I���p�l������
        GameObject Stage1 = GameObject.Find("1-1");
        GameObject Stage2 = GameObject.Find("1-2");
        GameObject Stage3 = GameObject.Find("1-3");
        GameObject Stage4 = GameObject.Find("1-4");
        GameObject Stage5 = GameObject.Find("1-5");
        GameObject Stage6 = GameObject.Find("1-6");
        GameObject Stage7 = GameObject.Find("1-7");
        GameObject Stage8 = GameObject.Find("1-8");

        // �p�l����z���
        Stages = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };

        // �X�e�[�W���p�l���擾
        InfoPanel = GameObject.Find("StageInfoPanel");
        StageNo = GameObject.Find("Canvas").transform.Find("StageInfoPanel/StageNo").GetComponent<Text>(); ;
        InfoPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //================================================================================
        // �X�e�[�W����Z���N�g�ɖ߂��Ă����Ƃ��̉�ʒ���
        if (bBackSelect)
        {
            // �N���A�����X�e�[�W���̃p�l���������Ɉړ�������
            for (i = 0; i < ProgressStages; i++)
            {
                Stages[i].transform.position = new Vector3(-Range_X,
                                                           Range_Y,
                                                           Base_Z - RightPanel - 1);

                LeftPanel++;
                RightPanel--;
                Select++;
            }

            // �����킷��N���A���Ă��Ȃ��X�e�[�W��^�񒆂�
            Stages[i].transform.position = new Vector3(0, 0, 450);

            // �N���A���Ă������ɉ��o���o���E�E�E�H
            if(bClearStage)
            {

                bClearStage = false;
            }

            bBackSelect = false;
        }
        //================================================================================


        //================================================================================
        // �X�e�[�W�I��
        if (PanelState == PANEL_STATE.NONE)
        {
            // ����ʈړ�
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0) 
            {
                if (Select < ProgressStages)
                {
                    PanelState = PANEL_STATE.LEFT;
                    InfoPanel.SetActive(false);
                }
            }

            // ����ʈړ�
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
        // �p�l���ړ�
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
                // �X�e�[�W���\��
                InfoPanel.SetActive(true);
                StageNo.text = "Stage�@" + Select.ToString();
                break;
        }
        //================================================================================


        //================================================================================
        // �X�e�[�W�ڍs
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            // �Z���N�g�ɖ߂������p�̃t���OON
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
    // �X�e�[�W�N���A���ɐi���ۑ��֐�
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
