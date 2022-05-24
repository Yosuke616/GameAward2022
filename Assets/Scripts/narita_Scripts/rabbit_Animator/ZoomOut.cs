using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    //public Camera openingCamera;
    //public Camera mainCamera;

    private Vector3 targetPos;  // 目的位置

    private bool active;
    private float zoomSpeed = 0.05f;

    void Start()
    {
        // 目的の座標を決める（今回はサブカメラ1）
        targetPos = GameObject.Find("SubCamera1").transform.position;

        active = false;
    }

    void Update()
    {
        if (active)
        {
            // 距離の差を計算
            float diffX = targetPos.x - transform.position.x;
            float diffY = targetPos.y - transform.position.y;
            float diffZ = targetPos.z - transform.position.z;
            // 移動量計算
            Vector3 moveValue = new Vector3(diffX, diffY, diffZ) * zoomSpeed;
            // 移動
            transform.Translate(moveValue);

            // ズームが終了したら
            if(diffX < 0.001f && diffY < 0.001f && diffZ < 0.001f)
            {
                active = false;
                Debug.LogWarning("一致したよ");

                // 白フェードを入れる
                var paperChange = GameObject.Find("PaperChange").GetComponent<PaperChange>();
                paperChange.FadeStart();

                // オープニングカメラからゲームカメラに切り替える
                //openingCamera.enabled = false;
                //mainCamera.enabled = true;

                // オープニングモードからアクションモードに切り替える
                CursorSystem.SetGameState(CursorSystem.GameState.MODE_ACTION);
            }
        }
    }

    public void ZoomStart()
    {
        active = true;
    }

	public bool GetZoomStart()
	{
		return active;
	}
}
