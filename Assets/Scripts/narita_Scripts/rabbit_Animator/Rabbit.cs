using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Animator rabbitAnimator;
    public GameObject openingCamera;

    private bool onceFunc;

    // Start is called before the first frame update
    void Start()
    {
        onceFunc = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̃A�j���[�V������Ԃ��擾
        var state = rabbitAnimator.GetCurrentAnimatorStateInfo(0); // ������Animator��Layers�̔ԍ�

        // �A�j���[�V������Ԃ�"Move" �� "Idle"�ɑJ�ڂ����ꍇ
        if(onceFunc && state.IsName("RabbitIdle"))
        {
            onceFunc = false;

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
            // �I�[�v�j���O�J�����̃Y�[���A�E�g�@�\��ON�ɂ���
            Debug.LogWarning("�ҋ@����");
        }
    }
}
