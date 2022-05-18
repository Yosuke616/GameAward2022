using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRLCur : MonoBehaviour
{
    private Vector3 m_pos;

    private Vector3 SendPos;

    // ���̃I�u�W�F�N�g��X,Y���W�̈ړ����E
    private Vector2 limit;

    //�{�^�����������Ƃ��̃t���O
    private bool g_bFirstFlg;

    void Start()
    {
        //�������ŃI�u�W�F�N�g�̏ꏊ�����߂�
        m_pos = this.transform.position = new Vector3(0.0f,0.5f,0.0f);
        SendPos = new Vector3(0.0f,0.0f,0.0f);
        g_bFirstFlg = false;

        // �ړ����E�����߂Ă���
        limit.x = CreateTriangle.paperSizeX + 0.1f;
        limit.y = CreateTriangle.paperSizeY + 0.1f;
    }

    void Update()
    {
        // MainCamera��enable�ł͂Ȃ��ꍇ�͉������Ȃ�
        if (Camera.main == null) { return; }

        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            // �ڑ�����Ă���R���g���[���̖��O�𒲂ׂ�
            var controllerNames = Input.GetJoystickNames();
            // �����R���g���[�����ڑ�����Ă��Ȃ���΃}�E�X��
            if (controllerNames.Length == 0)
            {
                //--- �}�E�X
                // ���̃I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ɕϊ������l����
                // �}�E�X���W��z�̒l��0�ɂ���
                Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                // �}�E�X���W�����[���h���W�ɕϊ�����
                m_pos = Camera.main.ScreenToWorldPoint(cursor);
            }
            else
            {
                //--- �Q�[���p�b�h

                //�E�����ɓ�����
                if (Input.GetAxis("Horizontal2") < 0)
                {
                    m_pos.x += 0.25f;
                }
                //�������ɓ�����
                if (Input.GetAxis("Horizontal2") > 0)
                {
                    m_pos.x += -0.25f;
                }
                //�������ɓ�����
                if (Input.GetAxis("Vertical2") < 0)
                {
                    m_pos.y += -0.25f;
                }
                //������ɓ�����
                if (Input.GetAxis("Vertical2") > 0)
                {
                    m_pos.y += 0.25f;
                }
            }

        }
        else {
            this.gameObject.SetActive(false);
        }

        // �ړ����E
        if (m_pos.x >  limit.x) m_pos.x =  limit.x;
        if (m_pos.x < -limit.x) m_pos.x = -limit.x;
        if (m_pos.y >  limit.y) m_pos.y =  limit.y;
        if (m_pos.y < -limit.y) m_pos.y = -limit.y;

        // ���W�𔽉f
        this.transform.position = m_pos;

        //if (Input.GetAxis("LTrigger") == 1) {
        //    Debug.Log("mario");
        //}

        SendPos = this.transform.position;
    }

    //�J�[�\���𓮂������߂ɂ����̍��W�𑗂�
    public Vector3 GetCTRLPos() {
        return SendPos;
    }
}
