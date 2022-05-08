using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    private Vector3 targetPos;  // �ړI�ʒu
    private int zoomOutFrame;   // �ړ��ɗv����b��

    private int frameCount;
    private bool active;

    void Start()
    {
        // �ړI�̍��W�����߂�i����̓T�u�J����1�j
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

                // �����̍����v�Z
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
