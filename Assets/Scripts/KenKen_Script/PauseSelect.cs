using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSelect : MonoBehaviour
{
    // �|�[�Y�p�l���I��
    public GameObject PauseObj1;

    // �|�[�Y�p�l����������ꏊ
    public Transform parant1;


    // Start is called before the first frame update
    void Start()
    {
        // �v���n�u����
        PauseObj1 = Instantiate(PauseObj1, parant1);
        PauseObj1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
            PauseObj1.SetActive(!PauseObj1.activeSelf);

            if (PauseObj1.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
