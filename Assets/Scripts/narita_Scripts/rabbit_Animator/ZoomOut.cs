using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    private Vector3 targetPos;  // 目的位置
    private int zoomOutFrame;   // 移動に要する秒数

    private int frameCount;
    private bool active;

    void Start()
    {
        // 目的の座標を決める（今回はサブカメラ1）
        targetPos = GameObject.Find("SubCamera1").transform.position;

        frameCount = 0;
        zoomOutFrame = 60 * 2;

        active = false;
    }

    void Update()
    {
        if (active)
        {
            frameCount++;

            if (frameCount < zoomOutFrame)
            {

                // 距離の差を計算
                float diffX = targetPos.x - transform.position.x;
                float diffY = targetPos.y - transform.position.y;
                float diffZ = targetPos.z - transform.position.z;

                Vector3 moveValue = new Vector3(diffX, diffY, diffZ) * 1 / (float)zoomOutFrame;

                transform.Translate(moveValue);
            }
        }
    }

    public void ZoomStart()
    {
        active = true;
    }
}
