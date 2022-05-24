using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperChange : MonoBehaviour
{
    // 白ポリゴン
    [SerializeField]
    private Image _paperChange;

    // フェード時、加算していく値
    [SerializeField]
    private float FADESPEED = 0.1f;

    // フェード中の数値
    private float valueSpeed;

    // フェードイン、フェードアウトフラグ
    private bool _isFadeIn, _isFadeOut;

    // ウサギアニメーション用カメラ
    public Camera openingCamera;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 最初は見えなくする
        _paperChange.gameObject.SetActive(false);

        // フェードアウトフラグ
        _isFadeOut = false;

        // フェードインフラグ
        _isFadeIn = false;

        // フェードの数値
        valueSpeed = 0.0f;

        // ウサギアニメーション用カメラ
        openingCamera.enabled = true;
        mainCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadeOut)
        {
            // フェードアウト
            FadeOut();
        }
        else if (_isFadeIn)
        {
            // フェードイン
            FadeIn();
        }
    }

    public void FadeStart()
    {
        // 白ポリゴン描画
        _paperChange.gameObject.SetActive(true);

        // フェードアウト開始
        _isFadeOut = true;
    }

    private void FadeOut()
    {
        // フェード値
        valueSpeed += FADESPEED * Time.deltaTime;

        // Shaderに値を渡す
        _paperChange.material.SetFloat("_Value", valueSpeed);

        // フェードアウト完了
        if (valueSpeed >= 1.0f)
        {
            // 値補正
            valueSpeed = 1.0f;

            // フェードアウト終了
            _isFadeOut = false;

            // フェードイン開始
            _isFadeIn = true;

            // タイマー開始
            var setTime = GameObject.Find("Time").GetComponentInChildren<TimerScript>();
            setTime.TimerStart();

            // ウサギアニメーション終了
            openingCamera.enabled = false;
            mainCamera.enabled = true;

            // オープニングモードからアクションモードに切り替える
            CursorSystem.SetGameState(CursorSystem.GameState.MODE_ACTION);
        }
    }

    private void FadeIn()
    {
        // フェード値
        valueSpeed -= FADESPEED * Time.deltaTime;

        // Shaderに値を渡す
        _paperChange.material.SetFloat("_Value", valueSpeed);

        // フェードイン完了
        if (valueSpeed <= 0.0f)
        {
            // 値補正
            valueSpeed = 0.0f;

            // 白ポリゴンを見えなくする
            _paperChange.gameObject.SetActive(false);

            // フェードイン終了
            _isFadeIn = false;
        }
    }
}
