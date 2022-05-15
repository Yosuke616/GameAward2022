using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // �V�[���J�ڗp

public class StageSelect : MonoBehaviour
{
    // ���̃X�N���v�g�ɏo������ϐ�---------------------------------------------------
    public static int ProgressStages = 7;       // ���݂̐i��
    public static bool bBackSelect = false;     // �Z���N�g�ɋA���Ă������p�t���O
    public static bool bClearStage = false;     // �N���A���Ă����t���O
    //--------------------------------------------------------------------------------

    
    // �p�u���b�N�ł̃p�l���ړ��Ɋւ���ݒ�-------------------------------------------
    public Camera MainCam;       // ���C���J�����擾
    public Vector3 TargetPos;    // �J�����ړ��ʒu
    public float Speed;          // �p�l���ړ����̃X�s�[�h

    // �p�l���ړ�����
    public float Range_X;
    public float Range_Y;
    public float Range_Z;

    // �ړ��ʕۑ�
    private float Move_X = 0;
    private float Move_Y = 0;
    private float Move_Z = 0;
    //--------------------------------------------------------------------------------


    // �X�N���v�g���ł̕ϐ�-----------------------------------------------------------
    //private SaveData saveData;          // �X�e�[�W���̏��擾�p

    enum PANEL_STATE    // �p�l�����
    {
        LEFT,       // ���ړ�
        RIGHT,      // �E�ړ�
        NONE        // ����
    }
    private PANEL_STATE PanelState = PANEL_STATE.NONE;

    private GameObject[] Stages;        // �X�e�[�W�p�l���z�� 
    private GameObject InfoPanel;       // �X�e�[�W���\���p�l��
    private Text TimerData;             // �N���A�^�C��
    private Text StageNo;               // �p�l���̃X�e�[�WNo�p
    private Image[] star;               // ���\���p

    private float Base_Z = 850;         // �p�l�����s�����p    
    private float LeftPanel = 0;        // ���p�l������
    private float RightPanel = 7;       // �E�p�l������

    private bool CamZoom = false;       // �J�����ړ��t���O
    private float zoomSpeed = 0.025f;    // �J�����ړ����x   

    private int Select = 0;             // ���ݑI��       
    private int i;                      // ���[�v�p�ϐ�    

    private bool bLoad = false;         // �Z�[�u�f�[�^���[�h�p
    //--------------------------------------------------------------------------------


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

        // �������擾
        Image Star1 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star1").GetComponent<Image>();
        Image Star2 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star2").GetComponent<Image>();
        Image Star3 = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Star3").GetComponent<Image>();

        // �p�l����z���
        Stages = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Stage8 };

        // ��������z���
        star = new Image[] { Star1, Star2, Star3 };

        // �X�e�[�W���p�l���擾
        InfoPanel = GameObject.Find("StageInfoPanel");
        TimerData = GameObject.Find("Canvas").transform.Find("StageInfoPanel/Timer").GetComponent<Text>();
        StageNo = GameObject.Find("Canvas").transform.Find("StageInfoPanel/StageNo").GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        // �Z�[�u�f�[�^���[�h-------------------------------------------------------------
        if (bLoad == false)
        {
            SaveLoad.LoadData();
            ProgressStages = SaveLoad.saveData.Progress;
            bLoad = true;
        }
        //--------------------------------------------------------------------------------


        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoad.TestSaveLoad();
            ProgressStages = SaveLoad.saveData.Progress;
        }


        // �X�e�[�W����Z���N�g�ɖ߂��Ă����Ƃ��̉�ʒ���---------------------------------
        if (bBackSelect)
        {
            // ���N���A�������̈ȊO�̃p�l�����E���Ɉړ�������
            for (i = 0; i < ProgressStages - 1; i++)
            {
                Stages[i].transform.position = new Vector3(-Range_X,
                                                           Range_Y,
                                                           Base_Z - RightPanel - 1);

                LeftPanel++;
                RightPanel--;
                Select++;
            }

            // �N���A�����X�e�[�W��^�񒆂�
            Stages[i].transform.position = new Vector3(0, 0, 450);

            // �N���A���Ă������ɉ��o���o���E�E�E�H
            if (bClearStage)
            {

                bClearStage = false;
            }

            bBackSelect = false;
        }
        //--------------------------------------------------------------------------------


        // �X�e�[�W�I��-------------------------------------------------------------------
        if (PanelState == PANEL_STATE.NONE)
        {
            // ����ʈړ�
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
            {
                if (Select < ProgressStages)
                {
                    PanelState = PANEL_STATE.LEFT;

                    // �d������
                    FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.LEFT);

                    // �X�e�[�W��񌩂��Ȃ��悤��
                    InfoPanel.SetActive(false);
                }
            }

            // ����ʈړ�
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
            {
                if (Select > 0)
                {
                    PanelState = PANEL_STATE.RIGHT;

                    // �d������E
                    FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.RIGHT);

                    // �X�e�[�W��񌩂��Ȃ��悤��
                    InfoPanel.SetActive(false);
                }
            }
        }
        //--------------------------------------------------------------------------------


        // �p�l���ړ�---------------------------------------------------------------------
        switch (PanelState)
        {
            case PANEL_STATE.LEFT:
                // �p�l�����ړ�------------------------------------------------------------------
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
                // �p�l���E�ړ�-------------------------------------------------------------------
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
                // �ʏ펞-------------------------------------------------------------------------
                // �d�����񗎂�����
                if(!CamZoom)
                FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.NONE);

                // �w�i�ύX
                ChangeBG.ChangeBg(Select);

                // �X�e�[�W���\��
                InfoPanel.SetActive(true);
                TimerData.text = SaveLoad.saveData.Timer[Select];       // �^�C�}�[�����\��
                StageNo.text = "Stage�@" + Select.ToString();           // �X�e�[�W���\��

                for (i = 0; i < SaveLoad.saveData.Star[Select]; i++)    // �������\��
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


        // �J�����ړ�(�X�e�[�W�ڍs)-------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            // �J�����ړ��t���OON
            CamZoom = true;

            // �d������^�񒆂�
            FairyMoveSelect.MoveChange(FairyMoveSelect.FAIRY_STATE.CENTER);
        }

        // �X�e�[�W�ړ�
        if (CamZoom)
        {
            // �����̍����v�Z
            float diffX = TargetPos.x - MainCam.transform.position.x;
            float diffY = TargetPos.y - MainCam.transform.position.y;
            float diffZ = TargetPos.z - MainCam.transform.position.z;

            // �ړ��ʌv�Z
            Vector3 moveValue = new Vector3(diffX, diffY, diffZ) * zoomSpeed;

            // �ړ�
            MainCam.transform.Translate(moveValue);

            // �Y�[�����I��������
            if (diffX < 0.001f && diffY < 0.001f && diffZ < 0.001f)
            {
                CamZoom = false;

                // �X�e�[�W���\������
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
