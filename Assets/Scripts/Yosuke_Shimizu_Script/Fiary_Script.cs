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
        FIARY_PLAYER_TRACKING = 0,
        FIARY_BREAK_PAPER,

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

            //���ڈȍ~�ǂݍ��܂Ȃ��悤�ɂ���
            g_bFirst_Load = true;
        }

        //���}�E�X���N���b�N���Ă��邩(�ꍇ�ɂ����)
        //�N���b�N���Ă����ꍇ
        if (Input.GetMouseButton(0))
        {
            g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;   
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
                break;
            default:break;
        }


    }
}
