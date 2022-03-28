/*
 2022/3/26 ShimizuYosuke
 ���S����O���Ɍ������ă��C���΂��A����ɒ��S�Ɍ������Ĕ�΂�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script : MonoBehaviour
{
    //�l��Ԃ����߂̃��C
    RaycastHit hits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X�̍��W���擾����(�r���[�{�[�g���W)
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mousePos.z = 0.1f;

        //���C���o���n�߂���W
        Vector3 StartPos = new Vector3(0.0f,0.0f,0.1f);

        //�v�Z�p�ɏ����������炷
        mousePos.x -= 0.5f;
        mousePos.y -= 0.5f;

        //���C�ɂԂ������I�u�W�F�N�g�̏����擾����
        RaycastHit hit;

        //�}�E�X���W�p�̌v�Z
        mousePos *= 10000;
        mousePos.z = 0.1f;


        //��������}�E�X�̍��W�Ɍ����ă��C���΂�
        if (Physics.Raycast(StartPos, mousePos, out hit)) {
            if (hit.collider.CompareTag("OutSideHitBox")) {
                //���C����������
                Debug.DrawRay(StartPos,mousePos,Color.red,1.0f);

                //�q�b�g�������W��ۑ����Ă���
                Vector3 HitPos = hit.point;
                HitPos.z = 0.1f;

                //Debug.Log(HitPos);

                //���C���΂��v�Z������
                mousePos *= -1;
                mousePos.z = 0.1f;


                //�q�b�g�����Ƃ��납�炳��Ƀ��C���΂�
                if (Physics.SphereCast(HitPos, 5.0f, mousePos, out hits))
                {
                    Debug.DrawRay(HitPos, mousePos, Color.blue, 1.0f);
                    if (hits.collider.CompareTag("paper")) {
                        Debug.Log(hits.point);
                    }
                }

            }
        }
    }

    //���ڂ̃��C�łԂ������O���̍��W�𑗂邽�߂̂��
    public Vector3 GetPaperHit() {
        return hits.point;
    }
}
