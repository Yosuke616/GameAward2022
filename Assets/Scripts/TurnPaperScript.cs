/*
 2022/3/1 �u���z�S 
 �����߂��邽�߂̃X�N���v�g
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaperScript : MonoBehaviour
{
    //�y�[�W�̈ړ�����X�s�[�h
    [SerializeField] private float TurnSpeed = 3.0f;

    //�I�u�W�F�N�g�̖��O���擾����ׂ̃��m
    private GameObject m_Obj;

    // Start is called before the first frame update
    void Start()
    {
        //�������Ŏn�܂�̕��т����߂�
        for (int  i  = 0;i < 3 ;i++) {
            switch (i) {
                case 0:
                    m_Obj = GameObject.Find("Paper1Page");
                    //�����ŏꏊ�����肷��
                    GameObject.Find("Paper1Page").transform.position = new Vector3(15.0f, 0.0f, 100.0f);
                    //�y�[�W�ԍ���ݒ肷��

                    break;
                case 1:
                    m_Obj = GameObject.Find("Paper2Page");
                    //�����ŏꏊ�����肷��
                    GameObject.Find("Paper2Page").transform.position = new Vector3(15.0f, 0.0f, 99.0f);
                    //�y�[�W�ԍ���ݒ肷��

                    break;
                case 2:
                    m_Obj = GameObject.Find("Paper3Page");
                    //�����ŏꏊ�����肷��
                    GameObject.Find("Paper3Page").transform.position = new Vector3(15.0f, 0.0f, 98.0f);
                    //�y�[�W�ԍ���ݒ肷��

                    break;
                default:break;
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
