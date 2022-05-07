using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    //�G�ɂԂ����Ă��܂������ǂ����̃t���O
    private bool g_bGameOverFlg;

    //�����̂��
    public Text GO_Tex;

    //�Q�[���I�[�o�[�̉摜���o�����߂̂��
    public Image _GameOverBG;

    // Start is called before the first frame update
    void Start()
    {
        //���߂͓���Ȃ��悤�ɂ���
        g_bGameOverFlg = false;

        //�ŏ��͔�A�N�e�B�u
        _GameOverBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (g_bGameOverFlg) {

            Debug.Log("���[�b�^�����Ă�");
            if (GO_Tex != null) GO_Tex.text = "�@�@�@�@���s�I";

            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlaySeByName("jingle37");

            _GameOverBG.gameObject.SetActive(true);
           
        }
    }

    //�G�ɂԂ��������ǂ����̃t���O�𓾂�
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }

}
