using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyInPlayer : MonoBehaviour
{
    // ���̒��ɂ�����̗d���̃A�j���[�V����

    private Animator animator;


    void Start()
    {

        animator = GetComponent<Animator>();


        if (!animator) Debug.LogError("�A�j���[�^�[���擾�ł��܂���ł���");
    }

    void Update()
    {
    }

    public void ClearMotion()
    {
        animator.CrossFade("Clear", 0);

    }
}
