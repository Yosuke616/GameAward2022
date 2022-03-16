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

    public void Start()
    {
        // �ŏ��̑I��
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

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
        PauseUI.SetActive(!PauseUI.activeSelf);
        Time.timeScale = 1f;
    }
}