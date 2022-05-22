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

    public static bool LeftRight = false;
    private bool Left = false;
    private bool Right = true;
    private bool  Change = false;       // 何フレームに一回挙動を変えるか用
    private float Swing = 0;            // 振幅用
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
                // 位相を計算します。
                phase = Time.time * MoveSpeed * Mathf.PI;

                if (LeftRight == false)
                {
                    // 距離の差を計算
                    diffX = -80 - Fairy.transform.position.x;
                    diffY = 0 - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // 移動量計算
                    diffX *= MoveSpeed / 120;
                    diffY *= MoveSpeed / 120;
                    diffZ *= MoveSpeed / 120;

                    // 左まで北
                    if (Fairy.transform.position.x < -75)
                    {
                        LeftRight = true;
                        Left = true;
                        Right = false;

                        // SE再生
                        SoundManager.Instance.PlaySeByName("paper01");
                    }
                }
                else
                {
                    // 円移動用の計算
                    TargetPos_X = Mathf.Cos(phase) * 10;
                    TargetPos_Y = Mathf.Sin(phase) * 10;

                    // 距離の差を計算
                    diffX = -80 - TargetPos_X - Fairy.transform.position.x;
                    diffY = 0 - TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // 移動量計算
                    diffX *= MoveSpeed / 35;
                    diffY *= MoveSpeed / 35;
                    diffZ *= MoveSpeed / 35;
                }
                
                // 位置変更
                Fairy.transform.position += new Vector3(diffX, diffY, diffZ);
                break;
                //--------------------------------------------------------------------------------


            case FAIRY_STATE.RIGHT:
                // 画面右待機---------------------------------------------------------------------
                // 位相を計算します。
                phase = Time.time * MoveSpeed * Mathf.PI;

                if (LeftRight == false)
                {
                    // 距離の差を計算
                    diffX = 80 - Fairy.transform.position.x;
                    diffY = 0 - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // 移動量計算
                    diffX *= MoveSpeed / 120;
                    diffY *= MoveSpeed / 120;
                    diffZ *= MoveSpeed / 120;

                    // 左まで北
                    if (Fairy.transform.position.x > 75 && Fairy.transform.position.x < 85)
                    {
                        LeftRight = true;
                        Left = false;
                        Right = true;

                        // SE再生
                        SoundManager.Instance.PlaySeByName("paper01");
                    }
                }
                else
                {
                    // 円移動用の計算
                    TargetPos_X = Mathf.Cos(phase) * 10;
                    TargetPos_Y = Mathf.Sin(phase) * 10;

                    // 距離の差を計算
                    diffX = 80 - TargetPos_X - Fairy.transform.position.x;
                    diffY = 0 - TargetPos_Y - Fairy.transform.position.y;
                    diffZ = 100 - Fairy.transform.position.z;

                    // 移動量計算
                    diffX *= MoveSpeed / 35;
                    diffY *= MoveSpeed / 35;
                    diffZ *= MoveSpeed / 35;
                }

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
                // 何もしてない
                LeftRight = false;

                // 位相を計算します。
                phase = Time.time * MoveSpeed / 2 * Mathf.PI;

                // 振幅変化
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

                // 距離の差を計算
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

                // 移動量計算
                diffX *= MoveSpeed / 150;
                diffY *= MoveSpeed / 150;
                diffZ *= MoveSpeed / 150;

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
