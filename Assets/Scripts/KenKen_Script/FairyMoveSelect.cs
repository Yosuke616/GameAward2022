using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMoveSelect : MonoBehaviour
{
    // 妖精---------------------------------------------------------------------------
    private GameObject Fairy;
    //--------------------------------------------------------------------------------


    // 妖精の動き関係-----------------------------------------------------------------
    public enum FAIRY_STATE    // 妖精の動き
    {
        LEFT,       // 左移動
        RIGHT,      // 右移動
        CENTER,     // 中央
        CIRCLE,
        NONE        // 無し
    }
    public static FAIRY_STATE FairyState = FAIRY_STATE.NONE;

    private float X_Range = 80;         // 横移動範囲
    private float Y_Range = 40;         // 縦移動範囲

    private float phase;                // 位相

    private float diffX;                // 移動先との差
    private float diffY;                // 
    private float diffZ;                // 

    private float TargetPos_X;          // 移動先の位置
    private float TargetPos_Y;          // 
    private float TargetPos_Z;          // 

    private float MoveSpeed = 1.0f;     // 移動速度
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        Fairy = GameObject.Find("Yousei1");
    }

    // Update is called once per frame
    void Update()
    {
        // 妖精の動き---------------------------------------------------------------------
        switch(FairyState)
        {
            case FAIRY_STATE.LEFT:
                // 画面左待機---------------------------------------------------------------------
                // 距離の差を計算
                diffX = -80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // 移動量計算
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // 位置変更
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.RIGHT:
                // 画面右待機---------------------------------------------------------------------
                // 距離の差を計算
                diffX = 80 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // 移動量計算
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // 位置変更
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CENTER:
                // 中央に戻る動き-----------------------------------------------------------------
                // 距離の差を計算
                diffX = 30 - Fairy.transform.position.x;
                diffY = 0 - Fairy.transform.position.y;
                diffZ = 300 - Fairy.transform.position.z;

                // 移動量計算
                diffX *= MoveSpeed;
                diffY *= MoveSpeed;
                diffZ *= MoveSpeed;

                // 移動 
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.CIRCLE:
                // 円形に回る動き-----------------------------------------------------------------
                // 位相を計算します。
                phase = Time.time * MoveSpeed * Mathf.PI;

                // 円移動用の計算
                TargetPos_X = Mathf.Cos(phase) * X_Range;
                TargetPos_Y = Mathf.Sin(phase) * Y_Range;

                // 位置変更
                Fairy.transform.position = new Vector3(TargetPos_X, TargetPos_Y, 100);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.NONE:
                // ふわふわする動き-----------------------------------------------------------------
                // 位相を計算します。
                phase = Time.time * MoveSpeed / 2 * Mathf.PI;
                TargetPos_Y = Mathf.Sin(phase) * Y_Range;

                // 距離の差を計算
                diffX = 100 - Fairy.transform.position.x;
                diffY = TargetPos_Y - Fairy.transform.position.y;
                diffZ = 100 - Fairy.transform.position.z;

                // 移動量計算
                diffX *= MoveSpeed / 35;
                diffY *= MoveSpeed / 35;
                diffZ *= MoveSpeed / 35;

                // 位置変更
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------
    }


    // 妖精動き変化-------------------------------------------------------------------
    public static void MoveChange(FAIRY_STATE change)
    {
        FairyState = change;
    }
    //--------------------------------------------------------------------------------


}
