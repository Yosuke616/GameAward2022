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

    //�����̂��
    public Text GO_Tex;

    //�Q�[���I�[�o�[�̉摜���o�����߂̂��
    public Image _GameOverBG;

    //�������_�Ŏw���邩�͌X�̕ϐ��Ō��߂�
    private int MaxBlink;

    //�_�ł����邽�߂̕ϐ�
    private float alpha_Num;

    private bool Iine;

    int nCnt;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (g_bGameOverFlg) {

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

            else {
                if (GO_Tex != null) GO_Tex.text = "�@�@�@�@���s�I";

                SoundManager.Instance.StopBgm();
                SoundManager.Instance.PlaySeByName("jingle37");

                _GameOverBG.gameObject.SetActive(true);
            }
           
        }
    }

    //�G�ɂԂ��������ǂ����̃t���O�𓾂�
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }

}
