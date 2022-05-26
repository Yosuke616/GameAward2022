/*
作成日：2022/5/07 Shimizu Shogo
内容  ：切り取られた紙を変形させる
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakingPaper : MonoBehaviour
{
    private Material _mat;              // マテリアル

    public float Add;                   // 変化させる値
    public float valueChange;           // Shaderに渡す値
    public float valueAngle;            // 角度(左か右か)
    public float valueRadius;           // 固定値
    public float valueStrength;         // 固定値

    private bool bStart = false;        // 動作フラグ
    private bool bVibration = false;    // 振動フラグ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ゲームパッド状況取得
        var gamepad = Gamepad.current;

        // 破れる処理
        if (bStart)
        {
            // だんだんめくれていく
            valueChange += Add;

            // 振動処理
            if(gamepad != null && !bVibration)
            {
                StartCoroutine("Vibration");
                bVibration = true;
            }

            if (valueChange >= 300.0f)
            {
                valueChange = 300.0f;
                bStart = false;
                bVibration = false;
            }
            else if (valueChange <= -300.0f)
            {
                valueChange = -300.0f;
                bStart = false;
                bVibration = false;
            }
            _mat.SetFloat("_BendPivot", valueChange);
            _mat.SetFloat("_BendAngle", valueAngle);
            _mat.SetFloat("_BendRadius", valueRadius);
            _mat.SetFloat("_BendStrength", valueStrength);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // 右側にめくる
    public void SetRight()
    {
        bStart = true;
        Add = 0.7f;
        valueChange = 47.0f;
        valueAngle = 220.0f;
        valueRadius = 0.01f;
        valueStrength = 1.0f;
    }

    // 左側にめくる
    public void SetLeft()
    {
        bStart = true;
        Add = 0.7f;
        valueChange = 37.0f;
        valueAngle = 140.0f;
        valueRadius = 0.01f;
        valueStrength = 1.0f;
    }

    // マテリアルの設定
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }

    IEnumerator Vibration()
    {
        // ゲームパッドの状態取得
        var game = Gamepad.current;
        
        // 振動の強さ
        game.SetMotorSpeeds(0.5f, 0.5f);
        
        // 振動秒数
        yield return new WaitForSecondsRealtime(0.5f);
        
        // 振動無効
        game.SetMotorSpeeds(0.0f, 0.0f);
    }
}
