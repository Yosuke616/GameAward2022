using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    private bool g_bFirstFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��񂾂������ĘA���ŉ��������Ƃɂ��Ȃ����߂̊֐�
    public bool GetOneTimeDown()
    {
        // �O�t���[���ɉ�����Ă��Ȃ�������
        if (!g_bFirstFlg)
        {
            // ���݉�����Ă�����
            if (Input.GetAxis("LTrigger") == 1)
            {
                Debug.Log("1����������[");
                g_bFirstFlg = false;
                return true;
            }
        }

        // ������Ă���t���O�̍X�V
        if (Input.GetAxis("LTrigger") == 1)
        {
            g_bFirstFlg = true;
        }
        else
        {
            g_bFirstFlg = false;

        }


        return false;
    }
}