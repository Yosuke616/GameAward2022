/*
 2022/3/26 ShimizuYosuke�@�d���̓�������邽�߂̃X�N���v�g
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Script : MonoBehaviour
{
    //�萔��`=================================================
    //�d���̏�Ԃ�����
    public enum FIARY_MOVE {
        FIARY_PLAYER_TRACKING = 0,          //�v���C���[�ɒǏ]����
        FIARY_BREAK_PAPER,                  //�O�������ǂ�
        FIARY_PAPER_IN,                     //�������ɓ����Ă���

        FIARY_MAX
    }
    //=========================================================

    //�񋓑��������ϐ�
    FIARY_MOVE g_FiaryMove;

    //�J�[�\����Ԃ������Ă���X�N���v�g�������Ă���
    OutSide_Paper_Script_Second Paper_OutSide_Script;

    //�J�[�\���̃Q�[���I�u�W�F�N�g��������ׂ̕ϐ�
    GameObject OutSide_Cursor;

    //�ŏ��̈�񂾂��ǂݍ��߂�悤�ɂ���(true�œǂݍ��߂Ȃ�����)
    private bool g_bFirst_Load;

    //�e�I�u�W�F�N�g�̏����i�[����ϐ�
    GameObject ParentObj;

    //�e�̍��W���i�[����ׂ̕ϐ�
    Vector3 PlayerPos;

    //����j���Ă��邩�ǂ������m�F���邽�߂̕ϐ�
    CursorSystem CS_Script;

    //�J�[�\���̃V�X�e���𓾂邽�߂̊֐�
    GameObject Cursor_System;

    //��񂾂��O�g�ɍs����悤��
    private bool gFirst_Flg;

    // Start is called before the first frame update
    void Start()
    {
        //�񋓑�������������(�ŏ��̓v���C���[�̎����Ǐ]����)
        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;

        //���̃I�u�W�F�N�g�̐e�̏����擾����
        ParentObj = this.transform.parent.gameObject;

        //�e�I�u�W�F�N�g�̍��W��ۑ�����
        PlayerPos = ParentObj.transform.position;

        //�v���C���[�̏������Ɉړ�������
        this.transform.position = new Vector3(PlayerPos.x+1.0f, PlayerPos.y + 1.0f, PlayerPos.z) ;

        //�ŏ��͓ǂݍ��߂�悤�ɂ���
        g_bFirst_Load = false;

        //�ŏ������O�g�������悤�ɂ��邽�߂̃t���O
        gFirst_Flg = true;
    }

    // Update is called once per frame
    void Update()
    {
        //�ǂݍ��݂ł���悤�ɂ���(�^��������)
        if (!g_bFirst_Load) {
            //�J�[�\����������
            OutSide_Cursor = GameObject.Find("cursor");
            //�J�[�\���̒��̃X�N���v�g���Q�b�g����
            Paper_OutSide_Script = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>();


            //�j���Ă��邩�ǂ����̃t���O��������悤�ȃJ�[�\���𓾂�
            Cursor_System = GameObject.Find("Cursor");

            //�X�N���v�g�𓾂�
            CS_Script = Cursor_System.GetComponent<CursorSystem>();

            //���ڈȍ~�ǂݍ��܂Ȃ��悤�ɂ���
            g_bFirst_Load = true;
        }

        //���}�E�X���N���b�N���Ă��邩(�ꍇ�ɂ����)
        //�N���b�N���Ă����ꍇ
        if (CS_Script.GetBreakFlg())
        {
            //�j�郂�[�h�ōŏ��Ɉ�񂾂��͊O������郂�[�h�Ɉڍs����
            if (gFirst_Flg)
            {
                g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;
            }
            else {
                g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
            }
        }
        //�N���b�N���Ă��Ȃ������ꍇ
        else
        {
            g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
        }

        //��Ԃɂ���čX�V�̓��e��ς���
        switch (g_FiaryMove) {
            case FIARY_MOVE.FIARY_PLAYER_TRACKING:

                //�e�I�u�W�F�N�g�̍��W���X�V����
                ParentObj = this.transform.parent.gameObject;
                
                //�e�I�u�W�F�N�g�̍��W��ۑ�����
                PlayerPos = ParentObj.transform.position;

                //�v���C���[�̏������Ɉړ�������
                this.transform.position = new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z);

                break;
            case FIARY_MOVE.FIARY_BREAK_PAPER:
                //�}�E�X�̃|�W�V�����Ɉړ�������
                this.transform.position = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();
                //�X�̒��ɓ����Ă���Ƃ��ɃN���b�N����ƒ��ɂ������ړ����Ă������[�h�ɕς��
                if (Input.GetMouseButtonDown(0)) {
                    g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
                    gFirst_Flg = false;
                }
      
                break;
            case FIARY_MOVE.FIARY_PAPER_IN:
                //�N���b�N�����ꏊ�������ɕۑ�����
                Vector3 vPos = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();
                //�d���̌��̏ꏊ�Ƃ̍������o���ėd�����ړ�������(���łł�����ŕK���蒼������)
                Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);
                velocity += (vPos - this.transform.position)* 0.05f;
                velocity *= 0.5f;
                this.transform.position += velocity;                
                break;
            
            default:break;
        }


    }
}
