using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // �V�[���J�ڗp
using UnityEngine.UI;               // UI�p

public class Pause : MonoBehaviour
{
    // �|�[�Y�������ɕ\������UI
    public GameObject PauseUi;
    public GameObject Pausepanel;
    public GameObject Optionpanel;

    // �|�[�Y��ʂ̃{�^��
    public GameObject Resume;
    public GameObject Retry;
    public GameObject Title;
    public GameObject Option;

    public GameObject Resume_front;
    public GameObject Retry_front;
    public GameObject Title_front;
    public GameObject Option_front;

    private int MaxButton = 3;
    private int SelectButton = 0;

    private int Cnt = 0;

    private bool OptionFlg;

    public void Start()
    {
        Optionpanel.SetActive(false);
        OptionFlg = false;

        Resume.SetActive(false);
        Retry.SetActive(true);
        Title.SetActive(true);
        Option.SetActive(true);

        Resume_front.SetActive(true);
        Retry_front.SetActive(false);
        Title_front.SetActive(false);
        Option_front.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        Cnt--;
        if (Cnt < 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
            {
                Cnt = 10;
                SelectButton--;
                if (SelectButton < 0)
                {
                    SelectButton = MaxButton;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
            {
                Cnt = 10;
                SelectButton++;
                if (SelectButton > MaxButton)
                {
                    SelectButton = 0;
                }
            }
        }

        //��ɔ��ɂ����Ă���
        Resume.SetActive(true);
        Retry.SetActive(true);
        Title.SetActive(true);
        Option.SetActive(true);

        Resume_front.SetActive(false);
        Retry_front.SetActive(false);
        Title_front.SetActive(false);
        Option_front.SetActive(false);

        switch (SelectButton)
        {
            case 0:
                //Resume.Select();
                //Resume.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                //Debug.Log("Resume");
                Resume_front.SetActive(true);

                break;

            case 1:
                //Retry.Select();
                //Retry.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                //Debug.Log("Select");

                Retry_front.SetActive(true);

                break;
            case 2:
                //Title.Select();
                //Title.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                //Debug.Log("Title");

                Title_front.SetActive(true);

                break;

            case 3:
                //Option.Select();
                //Option.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                //Debug.Log("Option");

                Option_front.SetActive(true);

                break;
        }


        //�{�^���������邩�ǂ����𔻕ʂ���
        GameObject Camera = GameObject.Find("MainCamera");
        Debug.Log(OptionFlg);
        if (Camera.GetComponent<PauseContorol>().GetPauseFlf() && OptionFlg == false) {
            Debug.Log("���[�b�^�l�[");
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            {
                Debug.Log("��������[");
                switch (SelectButton) {
                    case 0: OnResume(); break;
                    case 1: OnRetry(); break;
                    case 2: OnTitle(); break;
                    case 3: OnOption(); break;
                }
            }

        }
    }

    // ���g���C
    public void OnRetry()
    {
        // ����V�[����Ǎ�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FadeManager.Instance.FadeStart(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;

        //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
        PauseUi.SetActive(false);
    }

    // �^�C�g����
    public void OnTitle()
    {
        // �Z���N�g�V�[����Ǎ�
        //SceneManager.LoadScene("StageSelect");
        FadeManager.Instance.FadeStart("StageSelect");
        Time.timeScale = 1f;

        //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
        PauseUi.SetActive(false);
    }

    // �ĊJ
    public void OnResume()
    {
        //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
        PauseUi.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnOption()
    {
        Pausepanel.SetActive(false);
        Optionpanel.SetActive(true);
        OptionFlg = true;
    }

    public void OffOption()
    {
        Pausepanel.SetActive(true);
        Optionpanel.SetActive(false);
        OptionFlg = false;
    }

    //������x�{�^�����������߂̃t���O���Z�b�g����ׂ̊֐�
    public void SetPauseFlg(bool Flg) {
        OptionFlg = Flg;
    }

}