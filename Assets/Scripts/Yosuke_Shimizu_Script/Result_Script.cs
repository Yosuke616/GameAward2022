using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Result_Script : MonoBehaviour
{
    //�N���A�������ǂ����̃t���O
    private bool g_bGoal;

    //���ꏊ
    private Vector3 targetPos = new Vector3(3.5f, -1.5f, -5.0f);

    //����Ă����X�s�[�h
    private float m_fSpeed = 0.5f;

    //������肳����
    private float m_fYukkuri = 0.25f;

    //�v�Z�p�ϐ�
    private Vector3 m_fvelocity;

    //�������o�����߂̂��
    public Text tex;
    public Text timerTex;

    //�g�p����摜
    public Image _resultBG;

    // Start is called before the first frame update
    void Start()
    {
        //���߂͓��疳���悤�ɂ���
        g_bGoal = false;

        //�A�N�e�B�u�ɂ��Ȃ�
        _resultBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            g_bGoal = true;
        }



        if (g_bGoal) {
            //�J���������
            GameObject Camera = GameObject.Find("MainCamera");

            
            m_fvelocity += ((targetPos - Camera.transform.position) * m_fSpeed);

            m_fvelocity *= m_fYukkuri;

            Camera.transform.position += m_fvelocity;

            if (Camera.transform.position == targetPos) {
                // �X�e�[�W�i���ۑ�
                StageSelect.UpdateProgress(SceneManager.GetActiveScene().name);

                if (tex && timerTex) tex.text = "�N���A�^�C��:" + timerTex.text;

                SoundManager.Instance.StopBgm();
                SoundManager.Instance.PlaySeByName("clear");

                _resultBG.gameObject.SetActive(true);
            }
            //Camera.transform.position = new Vector3(3.5f,-1.5f,-5.0f);

        }
    }

    //�N���A�������ǂ����̃t���O�𓾂�
    public void SetGoalFlg(bool GoalFlg) {
        g_bGoal = GoalFlg;
    }
}
