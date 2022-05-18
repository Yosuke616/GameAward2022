using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    // Start is called before the first frame update
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
        Continue_B.Add(new Vector3(-5.0f,-3.9f,-0.1f));
        Continue_B.Add(new Vector3(-1.3f,-3.4f,-0.1f));
        Option_B.Add(new Vector3(1.25f,-3.9f,-0.1f));
        Option_B.Add(new Vector3(4.95f,-3.4f,-0.1f));
        End_B.Add(new Vector3(4.95f,-2.25f, -0.2f));
        End_B.Add(new Vector3(8.25f,-1.75f, -0.2f));

        first_Flg = false;


        //1���ڂ��ǂ����̃t���O���I�t�ɂ���
        bFirdt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Titleflg)
        {
            nCnt--;

            if (nCnt < 0)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
                {
                    //GameObject delete = GameObject.Find("breaking paper line");

                    //�X�C�b�`���Ŏq�I�u�W�F�N�g������
                    switch (nSelectButton) {
                        case 0:
                            int nChildCnt = StartButton.transform.childCount;
                            if (nChildCnt < 0) {
                                break;
                            }
                            for (int x = 0;x < nChildCnt; x++) {
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
                    if (!first_Flg) {
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

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
            {
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

    //���߂���
    public void OnFirst()
    {
        Debug.Log("���[�b�^�[");
        Cre_Mesh();
        // ����V�[����Ǎ�
        //SceneManager.LoadScene("StageSelect");
    }

    //��������
    public void OnCountinue()
    {
        Debug.Log("���[�b�^���킢���[");
        Cre_Mesh();
        // ����V�[����Ǎ�
        //SceneManager.LoadScene("StageSelect");
    }

    //�I�v�V����
    public void OnOption()
    {
        Debug.Log("���[�b�^�l�����ā[�I");
        Cre_Mesh();

        Titleflg = true;
        Option.SetActive(true);
        Plate.SetActive(true);
        GameObject obj = GameObject.Find("OptionTitle");
        obj.GetComponent<TitleOption_Script>().SetTitleOption(false);
    }

    //�I���
    public void OnEnd()
    {
        Cre_Mesh();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
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
               
                //����
                Start.Add(new Vector3(-8.0f,-2.25f,-0.1f));
                Start.Add(new Vector3(-8.0f,-1.75f,-0.1f));
                Start.Add(new Vector3(-5.0f,-1.75f,-0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f,0.0f);
                _Vector2[1] = new Vector2(0.0f,1.0f);
                _Vector2[2] = new Vector2(1.0f,1.0f);

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Start);

                //���X�g�̒��g���폜����
                Start.Clear();

                //�E�������
                Start.Add(new Vector3(-8.0f, -2.25f, -0.1f));
                Start.Add(new Vector3(-5.0f, -1.75f, -0.1f));
                Start.Add(new Vector3(-5.0f, -2.25f, -0.1f));

                //UV�̐ݒ�
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Start);

                break;
            //��������
            case 1:
                //�������������
                ContinueButton.SetActive(false);

                List<Vector3> Continue = new List<Vector3>();

                //����
                Continue.Add(new Vector3(-5.0f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-1.3f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-5.0f,-3.9f,-0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Continue);

                //���X�g�̒��g���폜����
                Continue.Clear();

                //�E��
                Continue.Add(new Vector3(-1.3f, -3.4f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.9f, -0.1f));
                Continue.Add(new Vector3(-5.0f, -3.9f, -0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Continue);

                break;
            case 2:
                //�I�v�V����������
                OptionButton.SetActive(false);

                List<Vector3> Option = new List<Vector3>();

                //����
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Option);

                //���X�g�̒��g���폜����
                Option.Clear();

                //�E��
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(Option);

                break;
            case 3:
                //�I��������
                EndButton.SetActive(false);

                List<Vector3> End = new List<Vector3>();

                //����
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(End);

                //���X�g�̒��g���폜����
                End.Clear();

                //�E��
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));

                //���b�V�������
                obj.GetComponent<DrawMesh>().CreateMesh(End);
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
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                }
                break;
            case 1:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                }
                break;
            case 2:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                }
                break;
            case 3:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                }
                break;
        }

    }

    //UV���𑗂�
    public Vector2[] GetUV()
    {
        return _Vector2;
    }

}
