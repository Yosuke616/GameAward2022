using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // �V�[���J�ڗp
using UnityEngine.UI;               // UI�p

public class Title : MonoBehaviour
{
    // �^�C�g����ʂ̍ŏ��̑I������Ă���{�^��
    public Button FirstSelectButton;

    // ���g���C�X���V�[��
    public SceneObject StartScene;

    void Start()
    {
        // �ŏ��̑I��
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �Q�[���J�n
    public void OnStart()
    {
        // �J�n�V�[����Ǎ�
        SceneManager.LoadScene(StartScene);
    }

    // �I��
    public void OnEnd()
    {
// �J�����p
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
// ����ȊO
#else
        Application.Quit();
#endif
    }
}