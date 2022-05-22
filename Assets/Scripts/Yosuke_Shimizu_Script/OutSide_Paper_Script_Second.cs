/*
 2022/3/26 ShimizuYosuke 
 ���X�g����O���𓾂ă}�E�X���璆�S�̐����̌�_�����߂�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script_Second : MonoBehaviour
{
    //���X�g��ۑ����邽�߂̕ϐ�
    [SerializeField] private List<Vector3> OutLinePaper = new List<Vector3>();

    //��񂾂��N���b�N�����Ƃ��ɂ����Ŏ~�܂邽�߂̃t���O
    [SerializeField] private bool First_Flg;
    //�ŏ��̈�񂾂��O������ǂݍ��߂�悤�ɂ���(ture�œǂݍ��߂Ȃ�����)
    private bool g_bFirst_Load;

    //��_�̍��W��ۑ�����ϐ�
    [SerializeField] private Vector2 Cross_Pos;

    //���̒��S���W�͏�Ɉ�̂Ȃ̂ł����ŃZ�b�g����
    private Vector2 Paper_Center;

    //�N���b�N�����ꏊ��ۑ�����ϐ�
    private Vector3 Old_Click_Pos;

    //�ŏI�I�Ɍ��߂���W
    private Vector3 Pos;

    //���̊O����ۑ����邽�߂̕ϐ�
    private Vector3 OutPaper_Pos;

    //�����ɍs���Ȃ��悤�ɂ��邽�߂̕ϐ�
    private Vector3 Old_Pos;


    void Start()
    {
        //���̒��S���W���Z�b�g����
        Paper_Center = new Vector2(0.0f, 0.0f);

        //���������邽�т�false�ɂ��ēǂݍ��߂�悤�ɂ���
        g_bFirst_Load = false;

        //���̊O���p�̕ϐ�������������
        OutPaper_Pos = new Vector3(0.0f, 0.0f, 0.0f);

        //�ŏ��̃t���O��false�ɂȂ��Ă���(true�ň��ڂ͏I��)
        First_Flg = false;

        OutLinePaper.Add(new Vector3(-CreateTriangle.paperSizeX - 0.05f, CreateTriangle.paperSizeY + 0.05f, 0.0f));  // ����
        OutLinePaper.Add(new Vector3(CreateTriangle.paperSizeX + 0.05f, CreateTriangle.paperSizeY + 0.05f, 0.0f));  // �E��
        OutLinePaper.Add(new Vector3(CreateTriangle.paperSizeX + 0.05f, -CreateTriangle.paperSizeY - 0.05f, 0.0f));  // �E��
        OutLinePaper.Add(new Vector3(-CreateTriangle.paperSizeX - 0.05f, -CreateTriangle.paperSizeY - 0.05f, 0.0f));  // ����

    }

    // Update is called once per frame
    void Update()
    {
        //����Ŗ��Ȃ��͂�
        if (Camera.main == null) { return; }

        // �R���W�����J�����ɉf���Ă���v���C���[���擾
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && // �v���C���[���������Ԃ̏ꍇ
            player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            //�}�E�X���W�������ŃQ�b�g���܂�
            Vector2 Mouse_Pos = Input.mousePosition;
            //��ʂ̔����̑傫�����}�C�i�X���邱�Ƃɂ�萮������}�邺
            Mouse_Pos.x -= Screen.width / 2;
            Mouse_Pos.y -= Screen.height / 2;

            //Old_Click_Pos = Input.mousePosition;
            //Old_Click_Pos.z = 10.0f;
            //var Pos2 = Camera.main.ScreenToWorldPoint(Old_Click_Pos);

            //���N���b�N���ꂽ�炻���ŃJ�[�\���͎~�܂�
            if (!First_Flg)
            {
                // �ڑ�����Ă���R���g���[���̖��O�𒲂ׂ�
                var controllerNames = Input.GetJoystickNames();
                // �����R���g���[�����ڑ�����Ă��Ȃ���΃}�E�X��
                if (controllerNames.Length == 0)
                {
                    // �}�E�X
                    Cross_Pos = calcCrossPos_Mouse(Mouse_Pos, Paper_Center, OutLinePaper);
                }
                else
                {
                    // �Q�[���p�b�h
                    Cross_Pos = calcCrossPos_GamePad(Mouse_Pos, Paper_Center, OutLinePaper);
                }

                //�J�[�\�����Z�b�g�������W�Ɉړ�������
                this.transform.position = Cross_Pos;

                OutPaper_Pos = Cross_Pos;

            }
            else
            {
                this.transform.position = Cross_Pos;
            }

            #region --- �����ɍs���Ȃ��悤�ɂ��鏈��
            if (!(this.transform.position == new Vector3(0.0f, 0.0f, 0.0f)))
            {
                Old_Pos = this.transform.position;
            }
            if (this.transform.position == new Vector3(0.0f, 0.0f, 0.0f))
            {
                this.transform.position = Old_Pos;
            }
            #endregion
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    //�����Ɛ����̌v�Z�����邽�߂̊֐�
    //�߂�l:�v�Z���ďo�����񎟌��x�N�g��
    //����  :�}�E�X�̍��W�Ǝ��̒��S�̍��W�Ǝ���4�ӂ̏�񂪓��������X�g
    private Vector2 calcCrossPos_Mouse(Vector2 Mouse, Vector2 Center, List<Vector3> Square)
    {
        //�Ō�ɕԂ��Ƃ��̂��߂�2�����x�N�g��
        Vector2 CrossVector = new Vector2(0.0f, 0.0f);

        //�}�E�X�ƒ��S���d�Ȃ��Ă����瑬�U�Ŗ߂�l(0,0)��Ԃ�
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0))
        {
            return CrossVector;
        }

        //4�̕Ӕ_�n�ǂꂩ�������܂ŌJ��Ԃ�
        for (int i = 0; i < Square.Count; i++)
        {
            //���ԂɊO�����g���Ă���
            //�����ŊO���̒������o�邪����͎g��Ȃ�
            //���̍l�������Ԃ͕K�v
            Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //�U��ԍ������
            int VegetaNum = ((i + 1) % (Square.Count));

            //�����p�̐�����p�ӂ���
            float Num = (Mouse.x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Mouse.y - Center.y) * (Square[VegetaNum].x - Square[i].x);

            //�c�Ɖ��̌v�Z������
            float X = ((Square[i].x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Square[i].y - Center.y) * (Square[VegetaNum].x - Square[i].x)) / Num;
            float Y = ((Square[i].x - Center.x) * (Mouse.y - Center.y) - (Square[i].y - Center.y) * (Mouse.x - Center.x)) / Num;

            //�͈͊O�������玟�̐����ɍs��
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f)
            {
                continue;
            }

            //�ŏI����
            CrossVector.x = Center.x + X * (Mouse.x - Center.x);
            CrossVector.y = Center.y + X * (Mouse.y - Center.y);


            //Debug.Log(CrossVector.x);
            //Debug.Log(CrossVector.y);

        }


        //�ŏI�I�ɏo�����l��Ԃ�
        return CrossVector;
    }

    private Vector2 calcCrossPos_GamePad(Vector2 Mouse, Vector2 Center, List<Vector3> Square)
    {
        //�R���g���[���[�̂��
        GameObject CTRL = GameObject.Find("CTRLCur");

        Vector3 ctrl = CTRL.GetComponent<CTRLCur>().GetCTRLPos();

        Vector2 mmm;

        mmm.x = ctrl.x;
        mmm.y = ctrl.y;

        Mouse = mmm;

        Mouse *= 100;

        //�Ō�ɕԂ��Ƃ��̂��߂�2�����x�N�g��
        Vector2 CrossVector = new Vector2(0.0f, 0.0f);

        //�}�E�X�ƒ��S���d�Ȃ��Ă����瑬�U�Ŗ߂�l(0,0)��Ԃ�
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0))
        {
            return CrossVector;
        }

        //4�̕Ӕ_�n�ǂꂩ�������܂ŌJ��Ԃ�
        for (int i = 0; i < Square.Count; i++)
        {
            //���ԂɊO�����g���Ă���
            //�����ŊO���̒������o�邪����͎g��Ȃ�
            //���̍l�������Ԃ͕K�v
            Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //�U��ԍ������
            int VegetaNum = ((i + 1) % (Square.Count));

            //�����p�̐�����p�ӂ���
            float Num = (Mouse.x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Mouse.y - Center.y) * (Square[VegetaNum].x - Square[i].x);

            //�c�Ɖ��̌v�Z������
            float X = ((Square[i].x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Square[i].y - Center.y) * (Square[VegetaNum].x - Square[i].x)) / Num;
            float Y = ((Square[i].x - Center.x) * (Mouse.y - Center.y) - (Square[i].y - Center.y) * (Mouse.x - Center.x)) / Num;

            //�͈͊O�������玟�̐����ɍs��
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f)
            {
                continue;
            }

            //�ŏI����
            CrossVector.x = Center.x + X * (Mouse.x - Center.x);
            CrossVector.y = Center.y + X * (Mouse.y - Center.y);


            //Debug.Log(CrossVector.x);
            //Debug.Log(CrossVector.y);

        }


        //�ŏI�I�ɏo�����l��Ԃ�
        return CrossVector;
    }


    //�d���̏ꏊ��ύX���邽�߂̃Q�b�^�[
    public Vector2 GetCursorPos()
    {
        return Cross_Pos;
    }

    //���ɓ������J�[�\���̏ꏊ�𑗂邽�߂̊֐�
    public Vector3 GetCursorPoss_IN()
    {
        return this.transform.position;
    }

    //�A�E�g���C���̃��X�g���X�V����ׂ̊֐�
    //public void SetMoveLine(List<Vector3> Line, Vector3 Center)
    //{
    //    //���X�g�̍X�V������
    //    OutLinePaper = Line;
    //
    //    //���S���W���X�V����
    //    Paper_Center = Center;
    //}

    //�j�鎞�ɃJ�[�\���������Ɉړ�������
    private void CursorBreak()
    {
        GameObject TPA = GameObject.Find("CTRLCur");

        this.transform.position = TPA.transform.position;
    }

    //���ڂ��ǂ����̃t���O�𑗂邽�߂̊֐�
    public bool GetFirstFlg()
    {
        return First_Flg;
    }

    public void DivideStart()
    {
        First_Flg = true;
    }
    public void DivideEnd()
    {
        First_Flg = false;
    }

}
