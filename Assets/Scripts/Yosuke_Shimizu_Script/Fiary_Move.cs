using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Move : MonoBehaviour
{

    //�e�I�u�W�F�N�g�̏����i�[����ϐ�
    GameObject ParentObj_Move;
    //�e�̍��W���i�[����ׂ̕ϐ�
    Vector3 PlayerPos_Move;


    // ����������t���O
    private bool small, big;
    public void SmallStart()
    {
        small = true;
        frameCount = 30;
    }
    public void BigStart()
    {
        big = true;
        frameCount = 30;
    }
    Vector3 normalScale;
    int frameCount;


    void Start()
    {
        //���̃I�u�W�F�N�g�̐e�̏����擾����
        ParentObj_Move = this.transform.parent.gameObject;
        //�e�I�u�W�F�N�g�̍��W��ۑ�����
        PlayerPos_Move = ParentObj_Move.transform.position;
        //�v���C���[�̏������Ɉړ�������
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        big = small = false;
        normalScale = new Vector3(0.8f, 0.8f, 0.8f);
        frameCount = 30;
    }

    void Update()
    {
        GameObject ParentObj = transform.parent.gameObject;

        //�e�I�u�W�F�N�g�̍��W���X�V����
        ParentObj_Move = this.transform.parent.gameObject;

        //�e�I�u�W�F�N�g�̍��W��ۑ�����
        PlayerPos_Move = ParentObj_Move.transform.position;

        //�v���C���[�̏������Ɉړ�������
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        if (small)
        {
            // 0.5�b�ŃX�P�[����0�ɂ���
            frameCount--;

            Vector3 scale = normalScale * (frameCount / 30.0f);
            this.transform.localScale = scale;

            if (frameCount <= 0)
            {
                small = false;
                frameCount = 30;
            }
        }
        else if (big)
        {
            // 0.5�b�ŃX�P�[����0�ɂ���
            frameCount--;

            Vector3 scale = normalScale * (Mathf.Abs((float)(30 - frameCount)) / 30.0f);

            this.transform.localScale = scale;

            if (frameCount <= 0)
            {
                big = false;
                frameCount = 30;
            }
        }

    }
}
