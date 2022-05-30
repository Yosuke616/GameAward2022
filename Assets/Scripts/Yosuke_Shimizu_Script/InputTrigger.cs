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


    public bool GetOneTimeDown()
    {
        // 1�t���[���ȓ��Ŋ��ɌĂ΂�Č��ʂ����܂��Ă����炻���Ԃ�
        if (numberCalled != 0) return saveGetOneTimeDownFlag;

        // ���̊֐����Ă΂ꂽ�񐔂��L��
        numberCalled++;

        bool ret = false;

        float tri = Input.GetAxis("LTrigger");
        // L�g���K�[
        if (tri > 0.95f)
        {
            // �O�t���[���܂ŉ�����Ă��Ȃ��������ǂ���
            if (pressed == false) ret = true;

            pressed = true;
            Debug.Log("L trigger:" + tri);
        }
        // R�g���K�[
        else if (tri < 0) {}
        // ���͂���Ă��Ȃ����
        else
        {
            pressed = false;
        }

        saveGetOneTimeDownFlag = ret;
        if (ret)
        {
            //SoundManager.Instance.PlaySeByName("SE_MenuOperation");
            Debug.LogWarning("����");
        }
       
        return ret;
    }
}
