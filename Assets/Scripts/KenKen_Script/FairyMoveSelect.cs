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
        NONE        // ����
    }
    public static FAIRY_STATE FairyState = FAIRY_STATE.NONE;

    private float X_Range = 80;         // ���ړ��͈�
    private float Y_Range = 40;         // �c�ړ��͈�

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
                break;

            case FAIRY_STATE.RIGHT:
                break;

            case FAIRY_STATE.CENTER:
                // �����ɖ߂铮��-----------------------------------------------------------------
                // �����̍����v�Z
                float diffX = 30 - Fairy.transform.position.x;
                float diffY = 0 - Fairy.transform.position.y;
                float diffZ = 300 - Fairy.transform.position.z;

                // �ړ��ʌv�Z
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // �ړ� 
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.NONE:
                // �~�`�ɉ�铮��-----------------------------------------------------------------
                // �ʑ����v�Z���܂��B
                float phase = Time.time * MoveSpeed * Mathf.PI;

                // �~�ړ��p�̌v�Z
                float xPos = Mathf.Cos(phase) * X_Range;
                float yPos = Mathf.Sin(phase) * Y_Range;

                // �ʒu�ύX
                Fairy.transform.position = new Vector3(xPos, yPos, 100);
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
