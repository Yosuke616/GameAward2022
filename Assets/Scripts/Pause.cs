using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // �V�[���J�ڗp
using UnityEngine.UI;               // UI�p

public class Pause : MonoBehaviour
{
    // �|�[�Y�������ɕ\������UI
    [SerializeField]
    private GameObject PauseUI;

    // �|�[�Y��ʂ̍ŏ��̑I������Ă���{�^��
    public Button FirstSelectButton;

    // ���g���C����V�[��
    public SceneObject RetryScene;

    // �^�C�g���ɍs���V�[��
    public SceneObject TitleScene;

    public 

    void Start()
    {
        // �ŏ��̑I��
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h�� p ���R���g���[���[��Start�œ���
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
            PauseUI.SetActive(!PauseUI.activeSelf);

            //�@�|�[�YUI���\������Ă鎞�͒�~
            if (PauseUI.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    // ���g���C
    public void OnRetry()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene(RetryScene);
        Time.timeScale = 1f;
    }

    // �^�C�g����
    public void OnTitle()
    {
        // ����V�[����Ǎ�
        SceneManager.LoadScene(TitleScene);
        Time.timeScale = 1f;
    }

    // �ĊJ
    public void OnResume()
    {
        //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
        PauseUI.SetActive(!PauseUI.activeSelf);
        Time.timeScale = 1f;
    }
}