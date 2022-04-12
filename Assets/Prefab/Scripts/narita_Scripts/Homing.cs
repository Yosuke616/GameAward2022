using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{

    // �������܂˂�Ώۂ̃I�u�W�F�N�g
    public GameObject HomingTarget;

    private Vector3 OldPos;
    private Vector3 MoveValue;

    void Start()
    {
        OldPos = HomingTarget.transform.position;
    }

    void Update()
    {
        // �Ώۂ̃I�u�W�F�N�g�̈ړ���
        MoveValue = HomingTarget.transform.position - OldPos;

        // ���̃I�u�W�F�N�g�ɂ��̈ړ��ʂ����Z
        transform.position += MoveValue;

        // �ߋ����W���X�V
        OldPos = HomingTarget.transform.position;
    }
}
