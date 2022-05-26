using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    //�G�ɂԂ����Ă��܂������ǂ����̃t���O
    private bool g_bGameOverFlg;

    //�_�ł����܂�
    private bool g_bBlinking;

    //�Q�[���I�[�o�[�̉摜���o�����߂̂��
    public Image _GameOverBG;

    //�������_�Ŏw���邩�͌X�̕ϐ��Ō��߂�
    private int MaxBlink;

    //�_�ł����邽�߂̕ϐ�
    private float alpha_Num;

    private bool Iine = true;

    int nCnt;

    //�{�^����ǉ����Ă���
    public Image Select;
    public Image Retry;
    public Image Title;

    //�{�^���̐�
    private int nMaxButton = 2;
    private int SelectButton = 0;

    //�R���g���[���[�̐���p
    private int nCnt2;

    //�I���ł��邩�̃t���O
    private bool Optionflg;

    private bool SE;

    // Start is called before the first frame update
    void Start()
    {
        //���߂͓���Ȃ��悤�ɂ���
        g_bGameOverFlg = false;

        //�ŏ��͔�A�N�e�B�u
        _GameOverBG.gameObject.SetActive(false);

        //�_�ł����邽�߂ɍŏ��̓I���ɂ���
        g_bBlinking = true;

        MaxBlink = 10;

        nCnt = 10;

        //���߂͉��ɂ��������Ȃ�
        Optionflg = false;

        nCnt2 = 0;

        SE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Optionflg)
        {
            if (g_bGameOverFlg)
            {

                nCnt--;

                //�_�ł����Ă����ʂ��Â�����
                if (g_bBlinking)
                {
                    GameObject camera = GameObject.Find("MainCamera");

                    if (Iine && nCnt < 0)
                    {
                        camera.GetComponent<Blink_Script>().Blink(Iine);

                        Iine = false;
                        MaxBlink--;
                        nCnt = 10;

                    }
                    else if (!Iine && nCnt < 0)
                    {
                        camera.GetComponent<Blink_Script>().Blink(Iine);

                        Iine = true;
                        MaxBlink--;
                        nCnt = 10;
                    }

                    if (MaxBlink < 0)
                    {
                        camera.GetComponent<Blink_Script>().LastBlink();
                        g_bBlinking = false;
                    }
                }

                else
                {
                    //if (GO_Tex != null) GO_Tex.text = "�@�@�@�@���s�I";

                    if (!SE) {
                        SoundManager.Instance.StopBgm();
                        SoundManager.Instance.PlaySeByName("jingle37");
                        SE = true;
                    }

                    _GameOverBG.gameObject.SetActive(true);
                    Optionflg = true;
                }

            }
        }
        else
        {
            nCnt2--;

            if (nCnt2 < 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
                {
                    nCnt2 = 10;
                    SelectButton--;
                    if (SelectButton < 0)
                    {
                        SelectButton = nMaxButton;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
                {
                    nCnt2 = 10;
                    SelectButton++;
                    if (SelectButton > nMaxButton)
                    {
                        SelectButton = 0;
                    }
                }
            }

          
            //��ɔ��ɕς��Ă���
            Select.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Retry.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Title.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            switch (SelectButton)
            {
                case 0:
                   
                    //Select.Select();
                    Select.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 1:

                    //Retry.Select();
                    Retry.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case 2:

                    //Title.Select();
                    Title.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
            }

            //�{�^���������邩�ǂ����𔻕ʂ���
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
            {
                Debug.Log("��������[");
                switch (SelectButton)
                {
                    case 0: OnSelect(); break;
                    case 1: OnRetry(); break;
                    case 2: OnTitle(); break;
                }
            }

        }
    }

    //�G�ɂԂ��������ǂ����̃t���O�𓾂�
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }


    public void OnSelect()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene("StageSelect");
    }

    public void OnRetry()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitle()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene("StageSelect");
    }


}
