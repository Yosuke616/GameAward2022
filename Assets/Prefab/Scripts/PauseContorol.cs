using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseContorol : MonoBehaviour
{
    // �|�[�Y�p�l���I��
    public GameObject PauseObj;

    // �|�[�Y�p�l����������ꏊ
    public Transform parant;

    // Start is called before the first frame update
    void Start()
    {
        // �v���n�u����
        PauseObj = Instantiate(PauseObj, parant);
        PauseObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
            PauseObj.SetActive(!PauseObj.activeSelf);
        }

        //�@�|�[�YUI���\������Ă鎞�͒�~
        if (PauseObj.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}