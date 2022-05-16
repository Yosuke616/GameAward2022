using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    // �O�t���[���ɉ�����Ă������̃t���O
    static private bool pressed = false;
    // �g���K�[�@�\���Ă΂ꂽ��
    static private int numberCalled = 0;
    static private bool saveGetOneTimeDownFlag = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        numberCalled = 0;
        saveGetOneTimeDownFlag = false;
    }

    //��񂾂������ĘA���ŉ��������Ƃɂ��Ȃ����߂̊֐�
    public bool GetOneTimeDown2()
    {
        // �O�t���[���ɉ�����Ă��Ȃ�������
        //if (!g_bFirstFlg  && nCnt <= 0)
        //{
        //    // ���݉�����Ă�����
        //    if (Input.GetAxis("LTrigger") == 1)
        //    {
        //        Debug.Log("1����������[");
        //        g_bFirstFlg = false;
        //        nCnt = 15;
        //        return true;
        //    }
        //}

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

    public bool GetOneTimeDown()
    {
        if (numberCalled != 0) return saveGetOneTimeDownFlag;
        numberCalled++;

        bool ret = false;

        float tri = Input.GetAxis("LTrigger");
        if (tri > 0)
        {
            //Debug.Log("L trigger:" + tri);

            // �O�t���[���܂ŉ�����Ă��Ȃ��������ǂ���
            if (pressed == false) ret = true;

            pressed = true;
        }
        else if (tri < 0)
        {
            //Debug.Log("R trigger:" + tri);
        }
        else
        {
            //Debug.Log("  trigger:none");

            pressed = false;
        }

        saveGetOneTimeDownFlag = ret;
        if (ret) Debug.LogWarning("����");
       
        return ret;
    }
}
