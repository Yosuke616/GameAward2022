using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfChildrenChanged : MonoBehaviour
{

    [SerializeField]private int _childCount = 0;
    public int _lowerLimitNum = 15;

    private void Start()
    {
        _childCount = transform.childCount;
    }

    private void Update()
    {
        // �q�I�u�W�F�N�g���ɕύX���������ꍇ�ɓ���
        if (_childCount != transform.childCount)
        {
            // �q�I�u�W�F�N�g�̐���\������
            Debug.Log("CheckIfChildrenChanged   " + _childCount + "  �� " + transform.childCount + "limit:" + _lowerLimitNum);

            // �q�I�u�W�F�N�g�̐������̐��ȉ��ɂȂ����炱�̃I�u�W�F�N�g������
            if (transform.childCount < _lowerLimitNum) Destroy(this.gameObject);

            // �q�I�u�W�F�N�g�̐����X�V
            _childCount = transform.childCount;
        }
    }

}