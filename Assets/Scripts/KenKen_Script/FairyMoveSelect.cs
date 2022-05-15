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
                // �����̍����v�Z
                diffX = -80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // �ړ��ʌv�Z
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // �ʒu�ύX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.RIGHT:
                // ��ʉE�ҋ@---------------------------------------------------------------------
                // �����̍����v�Z
                diffX = 80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // �ړ��ʌv�Z
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

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
                // �ʑ����v�Z���܂��B
                phase = Time.time * MoveSpeed / 2 * Mathf.PI;
                TargetPos_Y = Mathf.Sin(phase) * Y_Range;

                // �����̍����v�Z
                diffX = 100 - Fairy.transform.position.x;
                diffY = TargetPos_Y - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // �ړ��ʌv�Z
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

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
