/*
 2022/3/26 ShimizuYosuke 
 ���X�g����O���𓾂ă}�E�X���璆�S�̐����̌�_�����߂�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script_Second : MonoBehaviour
{
    //���X�g�̏���ۑ����邽�߂̃Q�[���I�u�W�F�N�g��ۑ�����ϐ�
    GameObject PaperObject;

    //�A�E�g���C���̃X�N���v�g�������Ă���
    OutLinePath outlinepath_script;

    //���X�g��ۑ����邽�߂̕ϐ�
    private List<Vector3> OutLinePaper = new List<Vector3>();

    //�ŏ��̈�񂾂��O������ǂݍ��߂�悤�ɂ���(ture�œǂݍ��߂Ȃ�����)
    private bool g_bFirst_Load;

    //��_�̍��W��ۑ�����ϐ�
    public Vector2 Cross_Pos;

    //���̒��S���W�͏�Ɉ�̂Ȃ̂ł����ŃZ�b�g����
    //���̒��S�̍��W���Z�b�g����
    private Vector2 Paper_Center;

    //�N���b�N�����ꏊ��ۑ�����ϐ�
    private Vector3 Old_Click_Pos;

    //�ŏI�I�Ɍ��߂���W
    private Vector3 Pos;

    //���̊O����ۑ����邽�߂̕ϐ�
    private Vector3 OutPaper_Pos;

    //��񂾂��N���b�N�����Ƃ��ɂ����Ŏ~�܂邽�߂̃t���O
    private bool First_Flg;

    // Start is called before the first frame update
    void Start()
    {
        //���̒��S���W���Z�b�g����
        Paper_Center = new Vector2(0.0f, 0.0f);

        //���������邽�т�false�ɂ��ēǂݍ��߂�悤�ɂ���
        g_bFirst_Load = false;

        //���̊O���p�̕ϐ�������������
        OutPaper_Pos = new Vector3(0.0f,0.0f,0.0f);

        //�ŏ��̃t���O��false�ɂȂ��Ă���(true�ň��ڂ͏I��)
        First_Flg = false;

    }

    // Update is called once per frame
    void Update()
    {
        //��񂾂��ǂݍ��߂�悤�ɂ�����
        if (!g_bFirst_Load) {
            //�A�E�g���C���̏������I�u�W�F�N�g��T���Ă��ă��X�g�̏���Ⴄ
            //�o���Ȃ������炱�����ł�낤
            //���̃I�u�W�F�N�g�������Ă���
            PaperObject = GameObject.Find("paper");
            //�X�N���v�g���Q�b�g����
            outlinepath_script = PaperObject.GetComponent<OutLinePath>();

            //���X�g��������
            OutLinePaper = outlinepath_script.OutLineVertices;

            //�ŏ��̈�񂾂��ǂݍ��߂�悤�ɂ���
            g_bFirst_Load = true;
        }
        //����Ŗ��Ȃ��͂�
        if (Camera.main == null) { return; }

        //�}�E�X���W�������ŃQ�b�g���܂�
        Vector2 Mouse_Pos = Input.mousePosition;

        //��ʂ̔����̑傫�����}�C�i�X���邱�Ƃɂ�萮������}�邺
        Mouse_Pos.x -= Screen.width/2;
        Mouse_Pos.y -= Screen.height/2; 

        //�j�邽�߂̃X�N���v�g��p�ӂ���
        GameObject Cur = GameObject.Find("Cursor");
        CursorSystem Cur_SY = Cur.GetComponent<CursorSystem>();
        //�j���ԂɂȂ����Ƃ��ɃJ�[�\���������Ɉړ�������֐����Ă�
        if (Cur_SY.GetBreakFlg())
        {
            CursorBreak();
            First_Flg = false;
        }
        else {
            Old_Click_Pos = Input.mousePosition;
            Old_Click_Pos.z = 10.0f;


            var Pos2 = Camera.main.ScreenToWorldPoint(Old_Click_Pos);

                //���N���b�N���ꂽ�炻���ŃJ�[�\���͎~�܂�
                if ((!First_Flg))
                {
                    //�}�E�X�ƒ��S�Ƃ��ꂼ��ǂ̕ӂ����l����(�֐��������) 
                    Cross_Pos = CalculationVector(Mouse_Pos, Paper_Center, OutLinePaper);

                    //�J�[�\�����Z�b�g�������W�Ɉړ�������
                    this.transform.position = Cross_Pos;

                    OutPaper_Pos = Cross_Pos;

                    if (Input.GetMouseButtonDown(0)) {
                        First_Flg = true;
                    }

                }
                else {
                    this.transform.position = Cross_Pos;
                }

        }
    }

    //�����Ɛ����̌v�Z�����邽�߂̊֐�
    //�߂�l:�v�Z���ďo�����񎟌��x�N�g��
    //����  :�}�E�X�̍��W�Ǝ��̒��S�̍��W�Ǝ���4�ӂ̏�񂪓��������X�g
    private Vector2 CalculationVector(Vector2 Mouse,Vector2 Center,List<Vector3> Square) {
        //�Ō�ɕԂ��Ƃ��̂��߂�2�����x�N�g��
        Vector2 CrossVector = new Vector2(0.0f,0.0f);

        //�}�E�X�ƒ��S���d�Ȃ��Ă����瑬�U�Ŗ߂�l(0,0)��Ԃ�
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0)){
            return CrossVector;
        }

        //4�̕Ӕ_�n�ǂꂩ�������܂ŌJ��Ԃ�
        for (int i = 0;i < Square.Count;i++) {
            //���ԂɊO�����g���Ă���
            //�����ŊO���̒������o�邪����͎g��Ȃ�
            //���̍l�������Ԃ͕K�v
            Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //�U��ԍ������
            int VegetaNum = ((i+1)%(Square.Count));

            //�����p�̐�����p�ӂ���
            float Num = (Mouse.x-Center.x)*(Square[VegetaNum].y-Square[i].y)-(Mouse.y-Center.y)*(Square[VegetaNum].x-Square[i].x);

            //�c�Ɖ��̌v�Z������
            float X = ((Square[i].x-Center.x)*(Square[VegetaNum].y-Square[i].y)-(Square[i].y-Center.y)*(Square[VegetaNum].x-Square[i].x))/Num;
            float Y = ((Square[i].x-Center.x)*(Mouse.y-Center.y)-(Square[i].y-Center.y)*(Mouse.x-Center.x))/Num;

            //�͈͊O�������玟�̐����ɍs��
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f) {
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
    public Vector2 GetCursorPos() {
        return Cross_Pos;
    }

    //�A�E�g���C���̃��X�g���X�V����ׂ̊֐�
    public void SetMoveLine(List<Vector3> Line,Vector3 Center) {
        //���X�g�̍X�V������
        OutLinePaper = Line;

        //���S���W���X�V����
        Paper_Center = Center;
    }

    //�j�鎞�ɃJ�[�\���������Ɉړ�������
    private void CursorBreak() {
        Pos = new Vector3(0.0f,0.0f,0.0f);
        //�{�^�������������W�Ɏ����Ă���
        if (Input.GetMouseButtonDown(0)) {
            Old_Click_Pos = Input.mousePosition;
            Old_Click_Pos.z = 10.0f;
        }

        Pos = Camera.main.ScreenToWorldPoint(Old_Click_Pos);
        //Debug.LogWarning(Pos);
        //Pos = Old_Click_Pos.WorldToScreenPoint(Old_Click_Pos);
        this.transform.position = Pos;
    }

}
