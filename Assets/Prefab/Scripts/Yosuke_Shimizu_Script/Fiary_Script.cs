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
        FIARY_PAPER_IN,                     //����j���Ă������ۑ����Ă�����
        FIARY_BREAK_MOVE,                   //�������۔j�邽�߂Ɉړ����Ă�����

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

   //�N���b�N�����Ƃ����ۑ����邽�߂̃��X�g
    private List<Vector3> MousePos = new List<Vector3>();

    //�O��̍��W��ۑ����邽�߂̕ϐ�
    private Vector3 Old_Mouse_Pos;

    //���̊O���̍��W��ۑ����邽�߂̕ϐ�
    private Vector3 Paper_Out;

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

        //���X�g�̒��g������������
        MousePos.Clear();

        //�O��̍��W��ۑ����邽�߂̕ϐ�
        Old_Mouse_Pos = new Vector3(0.0f,0.0f,0.0f);

        //�O�����ق��񂷂�����߂̕ϐ��̏�����
        Paper_Out = new Vector3(0.0f,0.0f,0.0f);
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
        //�v���C���[�ɒǏ]���Ă���Ƃ��������̒��ɓ���
        
        if (!CS_Script.GetBreakFlg())
        //�N���b�N���Ă��Ȃ������ꍇ
        {
            //���g�����݂���̂Ȃ�΂��̒��ɓ���
            if (MousePos.Count > 0)
            {
                g_FiaryMove = FIARY_MOVE.FIARY_BREAK_MOVE;
            }
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

                //�����Ŕj��t���O�������Ă�����ς���
                if (CS_Script.GetBreakFlg())
                {
                    if (!gFirst_Flg)
                    {
                        g_FiaryMove = FIARY_MOVE.FIARY_PAPER_IN;
                    }

                }
                else {
                    if (Input.GetMouseButtonDown(0)) {
                        g_FiaryMove = FIARY_MOVE.FIARY_BREAK_PAPER;
                    }
                }

                Debug.Log("FIARY_PLAYER_TRACKING");

                break;
            case FIARY_MOVE.FIARY_BREAK_PAPER:

                //�}�E�X�̃|�W�V�����Ɉړ�������
                this.transform.position = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();

                Debug.Log(OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos());

                //�X�̒��ɓ����Ă���Ƃ��ɃN���b�N����ƒ��ɂ������ړ����Ă������[�h�ɕς��
                if (Input.GetMouseButtonDown(0)) {
                    g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
                    gFirst_Flg = false;
                }

                //�O���p�ɕۑ����Ă���
                Paper_Out = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();

                Debug.Log("FIARY_BREAK_PAPER");

                break;
            case FIARY_MOVE.FIARY_PAPER_IN:

                //�d���̈ʒu�͈납���ɌŒ肵�Ă���
                this.transform.position = Paper_Out;

                //����X�g��
                //�N���b�N�����ꏊ�������ɕۑ�����
                Vector3 vPos = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();

                //�N���b�N���ꂽ�ꏊ�����X�g�ɕۑ�����(�O��̍��W�ƈ�����Ƃ��Ƀ��X�g�ɒǉ�����)
                if (!(vPos == Old_Mouse_Pos)) {
                    ////���X�g�ɒǉ�����
                    MousePos.Add(vPos);
                    ////�ߋ��̍��W�Ɣ�ׂ邽�߂ɍ��̍��W��ۑ����Ă���
                    Old_Mouse_Pos = vPos;

                    Debug.Log("������������");
                }



                if (Input.GetKey(KeyCode.Space)) {
                    Debug.Log(vPos);
                    Debug.Log(Old_Mouse_Pos);
                    Debug.Log(MousePos.Count);
                }

                Debug.Log("FIARY_PAPER_IN");
                break;

            case FIARY_MOVE.FIARY_BREAK_MOVE:
                //���X�g�ɒ��g�������Ă����ԂŔj���t���O���I�t�ɂȂ��Ă�����j�����[�V�����Ɉڂ�
                //���X�g�����ǂ��ėd�����ړ�������

                //���݂̏�2022/4/12
                //�Ō�ɃJ�[�\���̂������ꏊ�܂ňړ�����

                //���̂܂܂��ƈ�u�ŏI���̂ł��̒��̃��X�g�̂Ȃ��݂��S�ď�������v���C���[�Ǐ]���[�h�ɂ���

                Debug.Log(this.transform.position);
                Debug.Log(MousePos.Count);

                //===============================================================================================
                //�d���̌��̏ꏊ�Ƃ̍������o���ėd�����ړ�������(���łł�����ŕK���蒼������)
                //�������o���Ă����ۑ����邽�߂̕ϐ�
                Vector3 vPos2 = OutSide_Cursor.GetComponent<OutSide_Paper_Script_Second>().GetCursorPos();
                Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
                //�^�[�Q�b�g�Ǝ��g�Ƃ̃|�W�V�����������Ĉړ������������x��������
                velocity += (MousePos[0] - this.transform.position)* 0.1f;
                //�����x�I�Ȃ��̂��ꂪ�����ƈ��̑��x�ɂȂ�
                //velocity *= 0.5f;
                //�Ō�Ɉړ����悤�Ƃ�����
                this.transform.position += velocity;
                //===============================================================================================

                //���X�g�̍ŏ�������(�v���C���[�ƃ��X�g�Ŏw�肵�����W����v���������)
                if (this.transform.position == MousePos[0]) {
                    MousePos.RemoveAt(0);
                }

                //���X�g�̒��g�������Ȃ���������������Ă���
                if (MousePos.Count == 0) {
                    g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;
                    gFirst_Flg = true;
                    Paper_Out = new Vector3(0.0f,0.0f,0.0f);
                }

                Debug.Log("FIARY_BREAK_MOVE");

                break;
            
                default:break;
        }
    }
}