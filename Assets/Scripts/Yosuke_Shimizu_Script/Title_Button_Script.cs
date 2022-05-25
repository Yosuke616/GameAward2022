using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Title_Button_Script : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g(�{�^��)�����蓖�Ă�
    public GameObject StartButton;
    public GameObject ContinueButton;
    public GameObject OptionButton;
    public GameObject EndButton;

    //�{�^���̐�
    private int nMaxButton = 3;
    private int nSelectButton = 0;

    //�R���g���[���[�̎g�p����
    private int nCnt;

    //��������^�C�~���O�����߂�
    private bool Titleflg;

    //�j��ڂ���邽�߂̃I�u�W�F�N�g
    private GameObject button;

    //���W��ۑ�����ׂ̃��X�g
    private List<Vector3> Start_B = new List<Vector3>();
    private List<Vector3> Continue_B = new List<Vector3>();
    private List<Vector3> Option_B = new List<Vector3>();
    private List<Vector3> End_B = new List<Vector3>();

    private bool first_Flg;

    //�I�v�V������������悤�ɂ���
    public GameObject Option;
    public GameObject Plate;

    //1���ڂ�2���ڂ��𔻕ʂ���t���O
    private bool bFirdt;

    //�j��Ƃ��ɕK�v�ȃ}�e���A�������炩���߃Z�b�g���Ă���
    public Material[] _Mats = new Material[4];

    //UV��ݒ肵�ĕۑ����邽�߂̕ϐ�
    private Vector2[] _Vector2 = new Vector2[3];

    //1��2���𕪂��鉉�o�����
    private GameObject Left_Paper;
    private GameObject Right_Paper;

    //���o�p�̃t���O
    private bool bNextStage;

    //�V�[���J�ڗp
    private bool FirstSceneMove;

    //��������悤�ɂ���t���O
    private bool StartFlg;

    void Start()
    {
        //�R���g���[���[�𓮂����ׂ̎���
        nCnt = 0;

        //�^�C�g���𓮂������߂̃t���O
        Titleflg = false;

        //�{�^���̂�[��
        button = GameObject.Find("CreatebreakManager");

        //���X�g�̒��g�����߂�
        Start_B.Add(new Vector3(-8.0f,-2.25f,-0.1f));
        Start_B.Add(new Vector3(-5.0f,-1.75f,-0.1f));
        Continue_B.Add(new Vector3(-4.5f,-3.9f,-0.1f));
        Continue_B.Add(new Vector3(-1.7f,-3.4f,-0.1f));
        Option_B.Add(new Vector3(1.75f,-3.9f,-0.1f));
        Option_B.Add(new Vector3(4.5f,-3.4f,-0.1f));
        End_B.Add(new Vector3(4.95f,-2.25f, -0.2f));
        End_B.Add(new Vector3(7.6f,-1.75f, -0.2f));

        first_Flg = false;


        //1���ڂ��ǂ����̃t���O���I�t�ɂ���
        bFirdt = false;

        //�l�N�X�e�͍ŏ��̓I�t
        bNextStage = false;

        FirstSceneMove = false;

        StartFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlg)
        {
            //�l�N�X�e�̏󋵂ɂ���đS�Ă͕ς��
            if (bNextStage)
            {
                SetNextScene();
                return;
            }

            if (!Titleflg)
            {
                nCnt--;

                if (nCnt < 0)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
                    {
                        //GameObject delete = GameObject.Find("breaking paper line");

                        //�X�C�b�`���Ŏq�I�u�W�F�N�g������
                        switch (nSelectButton)
                        {
                            case 0:
                                int nChildCnt = StartButton.transform.childCount;
                                if (nChildCnt < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt; x++)
                                {
                                    Destroy(StartButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 1:
                                int nChildCnt2 = ContinueButton.transform.childCount;
                                if (nChildCnt2 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt2; x++)
                                {
                                    Destroy(ContinueButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 2:
                                int nChildCnt3 = OptionButton.transform.childCount;
                                if (nChildCnt3 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt3; x++)
                                {
                                    Destroy(OptionButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 3:
                                int nChildCnt4 = EndButton.transform.childCount;
                                if (nChildCnt4 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt4; x++)
                                {
                                    Destroy(EndButton.transform.GetChild(x).gameObject);
                                }
                                break;
                        }

                        first_Flg = false;
                        nCnt = 45;
                        nSelectButton--;
                        if (nSelectButton < 0)
                        {
                            nSelectButton = nMaxButton;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
                    {
                        //�X�C�b�`���Ŏq�I�u�W�F�N�g������
                        switch (nSelectButton)
                        {
                            case 0:
                                int nChildCnt = StartButton.transform.childCount;
                                if (nChildCnt < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt; x++)
                                {
                                    Destroy(StartButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 1:
                                int nChildCnt2 = ContinueButton.transform.childCount;
                                if (nChildCnt2 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt2; x++)
                                {
                                    Destroy(ContinueButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 2:
                                int nChildCnt3 = OptionButton.transform.childCount;
                                if (nChildCnt3 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt3; x++)
                                {
                                    Destroy(OptionButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 3:
                                int nChildCnt4 = EndButton.transform.childCount;
                                if (nChildCnt4 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt4; x++)
                                {
                                    Destroy(EndButton.transform.GetChild(x).gameObject);
                                }
                                break;
                        }

                        first_Flg = false;
                        nCnt = 45;
                        nSelectButton++;
                        if (nSelectButton > nMaxButton)
                        {
                            nSelectButton = 0;
                        }
                    }
                }

                //��ɔ��ɕς��Ă���
                StartButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                ContinueButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                OptionButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                EndButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                switch (nSelectButton)
                {
                    case 0:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Start_B, StartButton);
                            first_Flg = true;
                        }
                        StartButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 1:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Continue_B, ContinueButton);
                            first_Flg = true;
                        }
                        ContinueButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 2:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Option_B, OptionButton);
                            first_Flg = true;
                        }
                        OptionButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 3:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(End_B, EndButton);
                            first_Flg = true;
                        }
                        EndButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    default: break;
                }

                // ����
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
                {
                    // �_�� �� �j����ɕύX
                    //List<GameObject> papers = new List<GameObject>();
                    //papers.AddRange(GameObject.Find("paper"));
                    //foreach(var papaer in papers)
                    //{
                        //for (int i = 0; i < paper.transform.childCount; i++)
                        //{
                        //    paper.transform.GetChild(i).GetComponent<LineRendererOperator>().hoge();
                        //}
                    //}

                    switch (nSelectButton)
                    {
                        case 0: OnFirst(); break;
                        case 1: OnCountinue(); break;
                        case 2: OnOption(); break;
                        case 3: OnEnd(); break;

                        default: break;
                    }
                }

            }
        }
    }

    //���߂���
    public void OnFirst()
    {
        Debug.Log("���[�b�^�[");

        // �f�[�^������
        SaveLoad.InitSaveData();

        Cre_Mesh();

        

        // ����V�[����Ǎ�
        //SceneManager.LoadScene("StageSelect");
    }

    //��������
    public void OnCountinue()
    {
        Debug.Log("���[�b�^���킢���[");

        // �f�[�^�Ǎ�
        SaveLoad.LoadData();

        Cre_Mesh();


        // ����V�[����Ǎ�
        //SceneManager.LoadScene("StageSelect");
    }

    //�I�v�V����
    public void OnOption()
    {
        Debug.Log("���[�b�^�l�����ā[�I");
        Cre_Mesh();

        //Titleflg = true;
        //Option.SetActive(true);
        //Plate.SetActive(true);
        //GameObject obj = GameObject.Find("OptionTitle");
        //obj.GetComponent<TitleOption_Script>().SetTitleOption(false);
    }

    //�I���
    public void OnEnd()
    {
        Cre_Mesh();

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//    Application.Quit();
//#endif
    }

    public void SetTitleFlg(bool TF)
    {
        Titleflg = TF;
    }

    private void Cre_Mesh() {
        //���o
        GameObject obj = GameObject.Find("CreatebreakManager");

        switch (nSelectButton) {
            //�X�^�[�g�{�^��
            case 0:
                //�X�^�[�g�{�^��������
                StartButton.SetActive(false);

                List<Vector3> Start = new List<Vector3>();
                List<Vector3> Start_Break = new List<Vector3>();
                List<Vector3> breakLine1 = new List<Vector3>();
               
                //����
                Start.Add(new Vector3(-8.0f,-2.25f,-0.1f));
                Start.Add(new Vector3(-8.0f,-1.75f,-0.1f));
                Start.Add(new Vector3(-5.0f,-1.75f,-0.1f));

                // �u���[�N���C��
                breakLine1.Add(new Vector3(-5.5f,-1.5f,-0.1f));
                breakLine1.Add(new Vector3(-7.2f,-2.4f,-0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f,0.0f);
                _Vector2[1] = new Vector2(0.0f,1.0f);
                _Vector2[2] = new Vector2(1.0f,1.0f);

                //���b�V�������
                GameObject paper1 = obj.GetComponent<DrawMesh>().CreateMesh(Start);

                // �u���[�N���C������
                var line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, paper1);
                line1.name = "broken paper line";
                line1.GetComponent<LineRendererOperator>().hoge();

                //���X�g�̒��g���폜����
                Start.Clear();

                //�u���C�N���C���̃��X�g�̃��Z�b�g
                breakLine1.Clear();

                //�E�������
                Start.Add(new Vector3(-8.0f, -2.25f, -0.1f));
                Start.Add(new Vector3(-5.0f, -1.75f, -0.1f));
                Start.Add(new Vector3(-5.0f, -2.25f, -0.1f));

                //�u���[�N���C��
                breakLine1.Add(new Vector3(-5.5f, -1.5f, -0.1f));
                breakLine1.Add(new Vector3(-7.2f, -2.4f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //���b�V�������
                GameObject paper2 = obj.GetComponent<DrawMesh>().CreateMesh(Start);
                paper2.tag = "paper";

                // �u���[�N���C������
                line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, paper2);
                line1.name = "broken paper line";
                line1.GetComponent<LineRendererOperator>().hoge();

                break;
            //��������
            case 1:
                //�������������
                ContinueButton.SetActive(false);

                List<Vector3> Continue = new List<Vector3>();
                List<Vector3> breakLine2 = new List<Vector3>();


                //����
                Continue.Add(new Vector3(-5.0f,-3.9f,-0.1f));
                Continue.Add(new Vector3(-5.0f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-1.3f,-3.4f,-0.1f));

                //�u���[�N���C���̐ݒ�
                breakLine2.Add(new Vector3(-2.4f, -3.15f, -0.1f));
                breakLine2.Add(new Vector3(-3.95f, -4.1f, -0.1f));


                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //���b�V�������
                GameObject paper3 = obj.GetComponent<DrawMesh>().CreateMesh(Continue);
                paper3.tag = "paper";

                // �u���[�N���C������
                var line3 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, paper3);
                line3.name = "broken paper line";
                line3.GetComponent<LineRendererOperator>().hoge();

                //���X�g�̒��g���폜����
                Continue.Clear();


                //�E��
                Continue.Add(new Vector3(-5.0f, -3.9f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.4f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.9f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //���b�V�������
                GameObject paper4 = obj.GetComponent<DrawMesh>().CreateMesh(Continue);
                paper4.tag = "paper";

                // �u���[�N���C������
                var line4 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, paper4);
                line4.name = "broken paper line";
                line4.GetComponent<LineRendererOperator>().hoge();

                break;
            case 2:
                //�I�v�V����������
                OptionButton.SetActive(false);

                List<Vector3> Option = new List<Vector3>();
                List<Vector3> breakLine3 = new List<Vector3>();


                //����
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));


                //�u���[�N���C���̐ݒ�
                breakLine3.Add(new Vector3(2.0f, -4.2f, -0.1f));
                breakLine3.Add(new Vector3(3.9f, -3.25f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //���b�V�������
                GameObject paper5 = obj.GetComponent<DrawMesh>().CreateMesh(Option);
                paper5.tag = "paper";

                // �u���[�N���C������
                var line5 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine3, paper5);
                line5.name = "broken paper line";
                line5.GetComponent<LineRendererOperator>().hoge();

                //���X�g�̒��g���폜����
                Option.Clear();

                //�E��
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.9f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //���b�V�������
                GameObject paper6 = obj.GetComponent<DrawMesh>().CreateMesh(Option);
                paper6.tag = "paper";

                // �u���[�N���C������
                var line6 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine3, paper6);
                line6.name = "broken paper line";
                line6.GetComponent<LineRendererOperator>().hoge();

                break;
            case 3:
                //�I��������
                EndButton.SetActive(false);

                List<Vector3> End = new List<Vector3>();

                List<Vector3> breakLine4 = new List<Vector3>();

                //����
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));

                //�u���[�N���C���̐ݒ�
                breakLine4.Add(new Vector3(5.75f, -2.6f, -0.1f));
                breakLine4.Add(new Vector3(7.4f, -1.6f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //���b�V�������
                GameObject paper7 = obj.GetComponent<DrawMesh>().CreateMesh(End);
                paper7.tag = "paper";

                // �u���[�N���C������
                var line7 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine4, paper7);
                line7.name = "broken paper line";
                line7.GetComponent<LineRendererOperator>().hoge();

                //���X�g�̒��g���폜����
                End.Clear();

                //�E��
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -2.25f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //���b�V�������
                GameObject paper8 = obj.GetComponent<DrawMesh>().CreateMesh(End);
                paper8.tag = "paper";

                // �u���[�N���C������
                var line8 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine4, paper8);
                line8.name = "broken paper line";
                line8.GetComponent<LineRendererOperator>().hoge();

                break;
        }
       
    }

    //�ǂ̃{�^�����I������Ă��邩�𑗂�֐�
    public int GetSelectBUtton() {
        return nSelectButton;
    }

    //�e�N�X�`����\��ׂ̊֐�
    public void SettingTexture(GameObject obj) {
        switch (nSelectButton)
        {
            case 0:               
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                    obj.transform.localScale = new Vector3(1.75f, 6.7f);
                    obj.transform.position = new Vector3(5.0f, 11.4f);               
                break;
            case 1:
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                    obj.transform.localScale = new Vector3(1.45f, 6.7f);
                    obj.transform.position = new Vector3(1.4f, 20.85f);
                break;
            case 2:
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                    obj.transform.localScale = new Vector3(1.75f, 6.7f);
                    obj.transform.position = new Vector3(-2.5f, 20.75f);
                break;
            case 3:
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                    obj.transform.localScale = new Vector3(1.65f, 6.7f);
                    obj.transform.position = new Vector3(-4.3f, 11.3f);
                break;
        }

        if (!bFirdt)
        {
            //�����̎����ړ������邽��
            Left_Paper = obj;

            //switch (nSelectButton)
            //{
            //    case 0:
            //        List<Vector3> Start_Break = new List<Vector3>();
            //        //�j��ڂ����(����)
            //        Start_Break.Add(new Vector3(-6.0f, -2.0f, -0.5f));
            //        Start_Break.Add(new Vector3(-5.5f, -1.5f, -0.5f));
            //        GameObject breaklineSt = PaperBreakLineManager.Instance.CreateBreakLine(Start_Break, obj);
            //        Debug.Log(0, breaklineSt);
            //        break;
            //    case 1:
            //        List<Vector3> Continue_Break = new List<Vector3>();
            //        //�j��ڂ����(����)
            //        Continue_Break.Add(new Vector3(-5.0f, -3.9f, -0.5f));
            //        Continue_Break.Add(new Vector3(-1.3f, -3.4f, -0.5f));
            //        GameObject breaklineCo = PaperBreakLineManager.Instance.CreateBreakLine(Continue_Break, obj);
            //        Debug.Log(1, breaklineCo);
            //        break;
            //    case 2:
            //        List<Vector3> Option_Break = new List<Vector3>();
            //        //�j��ڂ����(����)
            //        Option_Break.Add(new Vector3(1.25f, -3.9f, -0.2f));
            //        Option_Break.Add(new Vector3(4.95f, -3.4f, -0.2f));
            //        GameObject breaklineOp = PaperBreakLineManager.Instance.CreateBreakLine(Option_Break, obj);
            //        Debug.Log(2, breaklineOp);
            //        break;
            //    case 3:
            //        List<Vector3> End_Break = new List<Vector3>();
            //        //�j��ڂ����(����)
            //        End_Break.Add(new Vector3(4.95f, -2.25f, -0.1f));
            //        End_Break.Add(new Vector3(8.25f, -1.75f, -0.1f));
            //        GameObject breaklineEn = PaperBreakLineManager.Instance.CreateBreakLine(End_Break, obj);
            //        Debug.Log(3, breaklineEn);
            //        break;

            //}

            bFirdt = true;
        }
        else {
            //�E���̎����ړ������邽��
            Right_Paper = obj;
            bNextStage = true;
           
        }

    }

    //UV���𑗂�
    public Vector2[] GetUV()
    {
        return _Vector2;
    }

    //���o�����
    public void SetNextScene() {

        //����Ɉړ�����
        Left_Paper.transform.position += new Vector3(-0.01f,0.005f,0.0f) * 0.5f;

        //�E���Ɉړ�������
        Right_Paper.transform.position += new Vector3(0.01f, -0.005f, 0.0f) * 0.5f;

        if (!FirstSceneMove) {
            //�t�F�[�h�A�E�gsaseru 
            switch (nSelectButton) {
                case 0:
                    FadeManager.Instance.FadeStart("StageSelect");
                    break;
                case 1:
                    //FadeManager.Instance.FadeStart("StageSelect");
                    break;
                case 2:
                    //FadeManager.Instance.FadeStart("Title_Option");
                    break;
                case 3:
                    break;

            }

            FirstSceneMove = true;
            Titleflg = true;
        }
    }

    //�^�C�g���𓮂�����悤�ɂ���
    public void SetStartFlg(bool bStart) {
        StartFlg = bStart;
    }
}
