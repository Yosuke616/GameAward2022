using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink_Script : MonoBehaviour
{
    //�v���C���[�p�̃��X�g
    GameObject[] Players;

    //�d���p�̃��X�g
    GameObject[] Fiaries;


    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�^�O�̂��Ă���I�u�W�F�N�g��S�Ēǉ�����
        Players = GameObject.FindGameObjectsWithTag("PlayerSub");

        //�d���^�O�̂��Ă���I�u�W�F�N�g��S�Ēǉ�����
        Fiaries = GameObject.FindGameObjectsWithTag("Fiary");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            Debug.Log(Players.Length);
            Debug.Log(Fiaries.Length);
        }
    }


    //�d���ƃv���C���[��_�ł�����֐���
    public void Blink(bool blink)
    {
        if (blink)
        {
            //�d���ƃv���C���[������
            foreach (GameObject P_Num in Players) {
                P_Num.gameObject.SetActive(false);
            }
            foreach (GameObject F_Num in Fiaries) {
                F_Num.gameObject.SetActive(false);
            }

        }
        else
        {
            //�d���ƃv���C���[���o��������
            foreach (GameObject P_Num in Players) {
                P_Num.gameObject.SetActive(true);
            }
            foreach (GameObject F_Num in Fiaries) {
                F_Num.gameObject.SetActive(true);
            }

        }
    }

    public void LastBlink() {
        //�d���ƃv���C���[������
        foreach (GameObject P_Num in Players)
        {
            P_Num.gameObject.SetActive(false);
        }
        foreach (GameObject F_Num in Fiaries)
        {
            F_Num.gameObject.SetActive(false);
        }
    }
}
