using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    private bool g_bFirstFlg = false;

    //���ԂŘA���ŉ����邩�ǂ����𐧌䂷��
    private int nCnt;

    // Start is called before the first frame update
    void Start()
    {
        nCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        nCnt--;
        if (nCnt < -1) {
            nCnt = -1;
        }
    }

    //��񂾂������ĘA���ŉ��������Ƃɂ��Ȃ����߂̊֐�
    public bool GetOneTimeDown()
    {
        // �O�t���[���ɉ�����Ă��Ȃ�������
        if (!g_bFirstFlg  && nCnt <= 0)
        {
            // ���݉�����Ă�����
            if (Input.GetAxis("LTrigger") == 1)
            {
                Debug.Log("1����������[");
                g_bFirstFlg = false;
                nCnt = 15;
                return true;
            }
        }

        // ������Ă���t���O�̍X�V
        //if (Input.GetAxis("LTrigger") == 1)
        //{
        //    g_bFirstFlg = true;
        //}
        //else
        //{
        //    g_bFirstFlg = false;

        //}


        return false;
    }
}
