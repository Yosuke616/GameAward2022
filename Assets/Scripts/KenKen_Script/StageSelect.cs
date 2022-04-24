using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // シーン遷移用

public class StageSelect : MonoBehaviour
{
    // 現在の進捗
    public static int ProgressStage = 0;

    // メインカメラ選択
    public Camera camera;

    // カメラ移動速度（スピード）
    public float SpeedCamera;

    // カメラ移動量範囲
    public float RangeCamera;

    // カメラ移動量
    private float MoveCamera = 0;

    // カメラ状態
    enum CAMERA_STATE
    {
        LEFT,
        RIGHT,
        NONE
    }
    private CAMERA_STATE _STATE = CAMERA_STATE.NONE;

    // 今現在の選択パネル
    private int Select = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 進捗状況によって最初の状況変化（クリア演出含む）
        //switch(ProgressStage)
        //{
        //}

        // ステージ選択
        if (_STATE == CAMERA_STATE.NONE)
        {
            // ←画面移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Select > 0)
                {
                    Select--;
                    _STATE = CAMERA_STATE.LEFT;
                }
                else
                    Select = 0;
            }

            // →画面移動
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Select < ProgressStage)
                {
                    Select++;
                    _STATE = CAMERA_STATE.RIGHT;
                }
                else
                    Select = ProgressStage;
            }
        }

        switch (_STATE)
        {
            case CAMERA_STATE.LEFT:
                camera.transform.position -= new Vector3(SpeedCamera, 0, 0);
                MoveCamera += SpeedCamera;
                if (MoveCamera >= RangeCamera)
                {
                    MoveCamera = 0;
                    _STATE = CAMERA_STATE.NONE;
                }
                break;

            case CAMERA_STATE.RIGHT:
                camera.transform.position += new Vector3(SpeedCamera, 0, 0);
                MoveCamera += SpeedCamera;
                if (MoveCamera >= RangeCamera)
                {
                    MoveCamera = 0;
                    _STATE = CAMERA_STATE.NONE;
                }
                break;

            case CAMERA_STATE.NONE:
                break;
        }

            // ステージ突入
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (Select)
            {
                case 0:
                SceneManager.LoadScene("test_Stage1");
                break;

                case 1:
                SceneManager.LoadScene("1-2");
                break;

                case 2:
                SceneManager.LoadScene("1-3");
                break;

                case 3:
                SceneManager.LoadScene("1-4");
                break;
            }
        }
    }


    // ステージクリア時に進捗保存関数
    public static void UpdateProgress(string name)
    {
        if (name == "test_Stage1") 
        {
            if (ProgressStage <= 1)
                ProgressStage = 1;
        }

        if (name == "1-2")
        {
            if (ProgressStage <= 2)
                ProgressStage = 2;
        }

        if (name == "1-3")
        {
            if (ProgressStage <= 3)
                ProgressStage = 3;
        }

        if (name == "1-4")
        {
            if (ProgressStage <= 4)
                ProgressStage = 4;
        }
    }
}
