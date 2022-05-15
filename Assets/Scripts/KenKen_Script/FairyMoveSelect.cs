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
        NONE        // –³‚µ
    }
    public static FAIRY_STATE FairyState = FAIRY_STATE.NONE;

    private float X_Range = 80;         // ‰¡ˆÚ“®”ÍˆÍ
    private float Y_Range = 40;         // cˆÚ“®”ÍˆÍ

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
                break;

            case FAIRY_STATE.RIGHT:
                break;

            case FAIRY_STATE.CENTER:
                // ’†‰›‚É–ß‚é“®‚«-----------------------------------------------------------------
                // ‹——£‚Ì·‚ğŒvZ
                float diffX = 30 - Fairy.transform.position.x;
                float diffY = 0 - Fairy.transform.position.y;
                float diffZ = 300 - Fairy.transform.position.z;

                // ˆÚ“®—ÊŒvZ
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // ˆÚ“® 
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.NONE:
                // ‰~Œ`‚É‰ñ‚é“®‚«-----------------------------------------------------------------
                // ˆÊ‘Š‚ğŒvZ‚µ‚Ü‚·B
                float phase = Time.time * MoveSpeed * Mathf.PI;

                // ‰~ˆÚ“®—p‚ÌŒvZ
                float xPos = Mathf.Cos(phase) * X_Range;
                float yPos = Mathf.Sin(phase) * Y_Range;

                // ˆÊ’u•ÏX
                Fairy.transform.position = new Vector3(xPos, yPos, 100);
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
