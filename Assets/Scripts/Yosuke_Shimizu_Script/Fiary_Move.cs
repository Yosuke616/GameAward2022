using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Move : MonoBehaviour
{

    //�e�I�u�W�F�N�g�̏����i�[����ϐ�
    GameObject ParentObj_Move;

    //�e�̍��W���i�[����ׂ̕ϐ�
    Vector3 PlayerPos_Move;

    bool qqq;

    //�E��������������
    //true�ŉE����
    private bool RightorLeft;

    private float RotateCnt;

    // Start is called before the first frame update
    void Start()
    {
        //���̃I�u�W�F�N�g�̐e�̏����擾����
        ParentObj_Move = this.transform.parent.gameObject;

        //�e�I�u�W�F�N�g�̍��W��ۑ�����
        PlayerPos_Move = ParentObj_Move.transform.position;

        //�v���C���[�̏������Ɉړ�������
        this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);

        qqq = false;

        //�����͉E����
        RightorLeft = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!qqq)
        {
            qqq = true;
            return;
        }

        GameObject ParentObj = transform.parent.gameObject;

        if (this.GetComponent<Fiary_Script>().GetMove())
        {
            //�e�I�u�W�F�N�g�̍��W���X�V����
            ParentObj_Move = this.transform.parent.gameObject;

            //�e�I�u�W�F�N�g�̍��W��ۑ�����
            PlayerPos_Move = ParentObj_Move.transform.position;

            //�v���C���[�̏������Ɉړ�������
            this.transform.position = new Vector3(PlayerPos_Move.x + 1.0f, PlayerPos_Move.y + 1.0f, PlayerPos_Move.z);
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            RightorLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RightorLeft = false;
        }

        if (RightorLeft)
        {

            if ( this.transform.localEulerAngles.y > 355 || this.transform.localEulerAngles.y <= 90.0f)
            {
                this.transform.Rotate(0, -5, 0);
            }
        }
        else
        {
            if (this.transform.localEulerAngles.y >= -5)
            {
                if (this.transform.localEulerAngles.y < 80 || this.transform.localEulerAngles.y >= 350)
                {
                    this.transform.Rotate(0, 5, 0);
                }
            }
        }
    }
}
