using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    //public Camera openingCamera;
    //public Camera mainCamera;

    private Vector3 targetPos;  // �ړI�ʒu

    private bool active;
    private float zoomSpeed = 0.05f;

    void Start()
    {
        // �ړI�̍��W�����߂�i����̓T�u�J����1�j
        targetPos = GameObject.Find("SubCamera1").transform.position;

        active = false;
    }

    void Update()
    {
        if (active)
        {
            // �����̍����v�Z
            float diffX = targetPos.x - transform.position.x;
            float diffY = targetPos.y - transform.position.y;
            float diffZ = targetPos.z - transform.position.z;
            // �ړ��ʌv�Z
            Vector3 moveValue = new Vector3(diffX, diffY, diffZ) * zoomSpeed;
            // �ړ�
            transform.Translate(moveValue);

            // �Y�[�����I��������
            if(diffX < 0.001f && diffY < 0.001f && diffZ < 0.001f)
            {
                active = false;
                Debug.LogWarning("��v������");

                // ���t�F�[�h������
                var paperChange = GameObject.Find("PaperChange").GetComponent<PaperChange>();
                paperChange.FadeStart();

                // �I�[�v�j���O�J��������Q�[���J�����ɐ؂�ւ���
                //openingCamera.enabled = false;
                //mainCamera.enabled = true;

                // �I�[�v�j���O���[�h����A�N�V�������[�h�ɐ؂�ւ���
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
