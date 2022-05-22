using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMoveSelect : MonoBehaviour
{
    // �d��---------------------------------------------------------------------------
    private GameObject Fairy;
    //--------------------------------------------------------------------------------


    // �d���̓����֌W-----------------------------------------------------------------
    public enum FAIRY_STATE    // �d���̓���
    {
        LEFT,       // ���ړ�
        RIGHT,      // �E�ړ�
        CENTER,     // ����
        CIRCLE,
        NONE        // ����
    }
    public static FAIRY_STATE FairyState = FAIRY_STATE.NONE;

    private float X_Range = 80;         // ���ړ��͈�
    private float Y_Range = 40;         // �c�ړ��͈�

    private float phase;                // �ʑ�

    private float diffX;                // �ړ���Ƃ̍�
    private float diffY;                // 
    private float diffZ;                // 

    private float TargetPos_X;          // �ړ���̈ʒu
    private float TargetPos_Y;          // 
    private float TargetPos_Z;          // 

    public static bool LeftRight = false;
    private bool Left = false;
    private bool Right = true;
    private bool  Change = false;       // ���t���[���Ɉ�񋓓���ς��邩�p
    private float Swing = 0;            // �U���p
    private float MoveSpeed = 1.0f;     // �ړ����x
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        Fairy = GameObject.Find("Yousei1");
    }

    // Update is called once per frame
    void Update()
    {
        // �d���̓���---------------------------------------------------------------------
        switch(FairyState)
        {
            case FAIRY_STATE.LEFT:
                // ��ʍ��ҋ@---------------------------------------------------------------------
                // �ʑ����v�Z���܂��B
                phase = Time.time * MoveSpeed * Mathf.PI;

                if (LeftRight == false)
                {
                    // �����̍����v�Z
                    diffX = -80 - Fairy.transform.position.x;
                    diffY = 0 - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // �ړ��ʌv�Z
                    diffX *= MoveSpeed / 120;
                    diffY *= MoveSpeed / 120;
                    diffZ *= MoveSpeed / 120;

                    // ���܂Ŗk
                    if (Fairy.transform.position.x < -75)
                    {
                        LeftRight = true;
                        Left = true;
                        Right = false;

                        // SE�Đ�
                        SoundManager.Instance.PlaySeByName("paper01");
                    }
                }
                else
                {
                    // �~�ړ��p�̌v�Z
                    TargetPos_X = Mathf.Cos(phase) * 10;
                    TargetPos_Y = Mathf.Sin(phase) * 10;

                    // �����̍����v�Z
                    diffX = -80 - TargetPos_X - Fairy.transform.position.x;
                    diffY = 0 - TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // �ړ��ʌv�Z
                    diffX *= MoveSpeed / 35;
                    diffY *= MoveSpeed / 35;
                    diffZ *= MoveSpeed / 35;
                }
                
                // �ʒu�ύX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.RIGHT:
                // ��ʉE�ҋ@---------------------------------------------------------------------
                // �ʑ����v�Z���܂��B
                phase = Time.time * MoveSpeed * Mathf.PI;

                if (LeftRight == false)
                {
                    // �����̍����v�Z
                    diffX = 80 - Fairy.transform.position.x;
                    diffY = 0 - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // �ړ��ʌv�Z
                    diffX *= MoveSpeed / 120;
                    diffY *= MoveSpeed / 120;
                    diffZ *= MoveSpeed / 120;

                    // ���܂Ŗk
                    if (Fairy.transform.position.x > 75 && Fairy.transform.position.x < 85)
                    {
                        LeftRight = true;
                        Left = false;
                        Right = true;

                        // SE�Đ�
                        SoundManager.Instance.PlaySeByName("paper01");
                    }
                }
                else
                {
                    // �~�ړ��p�̌v�Z
                    TargetPos_X = Mathf.Cos(phase) * 10;
                    TargetPos_Y = Mathf.Sin(phase) * 10;

                    // �����̍����v�Z
                    diffX = 80 - TargetPos_X - Fairy.transform.position.x;
                    diffY = 0 - TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // �ړ��ʌv�Z
                    diffX *= MoveSpeed / 35;
                    diffY *= MoveSpeed / 35;
                    diffZ *= MoveSpeed / 35;
                }

                // �ʒu�ύX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CENTER:
                // �����ɖ߂铮��-----------------------------------------------------------------
                // �����̍����v�Z
                diffX = 30 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 300 - Fairy.transform.position.z;

                // �ړ��ʌv�Z
                diffX *= MoveSpeed;
                diffY *= MoveSpeed;
                diffZ *= MoveSpeed;

                // �ړ� 
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CIRCLE:
                // �~�`�ɉ�铮��-----------------------------------------------------------------
                // �ʑ����v�Z���܂��B
                phase = Time.time * MoveSpeed * Mathf.PI;

                // �~�ړ��p�̌v�Z
                TargetPos_X = Mathf.Cos(phase) * X_Range;
                TargetPos_Y = Mathf.Sin(phase) * Y_Range;

                // �ʒu�ύX
                Fairy.transform.position = new Vector3(TargetPos_X, TargetPos_Y, 100);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.NONE:
                // �ӂ�ӂ킷�铮��-----------------------------------------------------------------
                // �������ĂȂ�
                LeftRight = false;

                // �ʑ����v�Z���܂��B
                phase = Time.time * MoveSpeed / 2 * Mathf.PI;

                // �U���ω�
               if(Change == false)
                {
                    Swing += Random.Range(0, 0.1f);
                    if (Swing > 20)
                    {
                        Change = true;
                    }
                }
               else
                {
                    Swing -= Random.Range(0, 0.1f); ;
                    if (Swing < 0)
                    {
                        Change = false;
                    }
                }

                TargetPos_Y = Mathf.Sin(phase) * 20 - 20;

                // �����̍����v�Z
                if (Left == true)
                {
                    diffX = -90 - Mathf.Cos(phase) * Swing - Fairy.transform.position.x;
                    diffY = TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;
                }
                if(Right == true)
                {
                    diffX = 90 - Mathf.Cos(phase) * Swing - Fairy.transform.position.x;
                    diffY = TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;
                }

                // �ړ��ʌv�Z
                diffX *= MoveSpeed / 150;
                diffY *= MoveSpeed / 150;
                diffZ *= MoveSpeed / 150;

                // �ʒu�ύX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------
    }


    // �d�������ω�-------------------------------------------------------------------
    public static void MoveChange(FAIRY_STATE change)
    {
        FairyState = change;
    }
    //--------------------------------------------------------------------------------


}
