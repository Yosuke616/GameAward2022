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
    public Button Resume;
    public Button Retry;
    public Button Title;
    public Button Option;
    private int MaxButton = 3;
    private int SelectButton = 0;

    private int Cnt = 0;

    public void Start()
    {
        Optionpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Cnt--;
        if (Cnt < 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
            {
                Cnt = 75;
                SelectButton--;
                if (SelectButton < 0)
                {
                    SelectButton = MaxButton;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
            {
                Cnt = 75;
                SelectButton++;
                if (SelectButton > MaxButton)
                {
                    SelectButton = 0;
                }
            }
        }

        switch(SelectButton)
        {
            case 0:
                Resume.Select();
                break;

            case 1:
                Retry.Select();
                break;

            case 2:
                Title.Select();
                break;

            case 3:
                Option.Select();
                break;
        }
    }

    // ���g���C
    public void OnRetry()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    // �^�C�g����
    public void OnTitle()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
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
    }

    public void OffOption()
    {
        Pausepanel.SetActive(true);
        Optionpanel.SetActive(false);
    }
}