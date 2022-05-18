using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMoveSelect : MonoBehaviour
{
    // —d¸---------------------------------------------------------------------------
    private GameObject Fairy;
    //--------------------------------------------------------------------------------


    // —d¸‚Ì“®‚«ŠÖŒW-----------------------------------------------------------------
    public enum FAIRY_STATE    // —d¸‚Ì“®‚«
    {
        LEFT,       // ¶ˆÚ“®
        RIGHT,      // ‰EˆÚ“®
        CENTER,     // ’†‰›
        CIRCLE,
        NONE        // –³‚µ
    }
    public static FAIRY_STATE FairyState = FAIRY_STATE.NONE;

    private float X_Range = 80;         // ‰¡ˆÚ“®”ÍˆÍ
    private float Y_Range = 40;         // cˆÚ“®”ÍˆÍ

    private float phase;                // ˆÊ‘Š

    private float diffX;                // ˆÚ“®æ‚Æ‚Ì·
    private float diffY;                // 
    private float diffZ;                // 

    private float TargetPos_X;          // ˆÚ“®æ‚ÌˆÊ’u
    private float TargetPos_Y;          // 
    private float TargetPos_Z;          // 

    private bool  Change = false;       // ‰½ƒtƒŒ[ƒ€‚Éˆê‰ñ‹““®‚ð•Ï‚¦‚é‚©—p
    private float Swing = 0;            // U•—p
    private float MoveSpeed = 1.0f;     // ˆÚ“®‘¬“x
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        Fairy = GameObject.Find("Yousei1");
    }

    // Update is called once per frame
    void Update()
    {
        // —d¸‚Ì“®‚«---------------------------------------------------------------------
        switch(FairyState)
        {
            case FAIRY_STATE.LEFT:
                // ‰æ–Ê¶‘Ò‹@---------------------------------------------------------------------
                // ‹——£‚Ì·‚ðŒvŽZ
                diffX = -80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // ˆÚ“®—ÊŒvŽZ
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // ˆÊ’u•ÏX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.RIGHT:
                // ‰æ–Ê‰E‘Ò‹@---------------------------------------------------------------------
                // ‹——£‚Ì·‚ðŒvŽZ
                diffX = 80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // ˆÚ“®—ÊŒvŽZ
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // ˆÊ’u•ÏX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CENTER:
                // ’†‰›‚É–ß‚é“®‚«-----------------------------------------------------------------
                // ‹——£‚Ì·‚ðŒvŽZ
                diffX = 30 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 300 - Fairy.transform.position.z;

                // ˆÚ“®—ÊŒvŽZ
                diffX *= MoveSpeed;
                diffY *= MoveSpeed;
                diffZ *= MoveSpeed;

                // ˆÚ“® 
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CIRCLE:
                // ‰~Œ`‚É‰ñ‚é“®‚«-----------------------------------------------------------------
                // ˆÊ‘Š‚ðŒvŽZ‚µ‚Ü‚·B
                phase = Time.time * MoveSpeed * Mathf.PI;

                // ‰~ˆÚ“®—p‚ÌŒvŽZ
                TargetPos_X = Mathf.Cos(phase) * X_Range;
                TargetPos_Y = Mathf.Sin(phase) * Y_Range;

                // ˆÊ’u•ÏX
                Fairy.transform.position = new Vector3(TargetPos_X, TargetPos_Y, 100);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.NONE:
                // ‚Ó‚í‚Ó‚í‚·‚é“®‚«-----------------------------------------------------------------
                // ˆÊ‘Š‚ðŒvŽZ‚µ‚Ü‚·B
                phase = Time.time * MoveSpeed / 2 * Mathf.PI;

                // U••Ï‰»
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

                // ‹——£‚Ì·‚ðŒvŽZ
                diffX = 90 - Mathf.Cos(phase) * Swing - Fairy.transform.position.x;
                diffY = TargetPos_Y - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // ˆÚ“®—ÊŒvŽZ
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // ˆÊ’u•ÏX
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------
    }


    // —d¸“®‚«•Ï‰»-------------------------------------------------------------------
    public static void MoveChange(FAIRY_STATE change)
    {
        FairyState = change;
    }
    //--------------------------------------------------------------------------------


}
