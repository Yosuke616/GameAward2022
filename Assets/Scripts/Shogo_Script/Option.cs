using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    // �e�X���C�_�[
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SESlider;
    public Button BackButton;

    // ���̐�
    private int MaxButton = 3;

    // ���ݑI�����Ă�����̗p
    private int SelectButton = 0;

    // ���͊Ԋu
    private int Cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Cnt--;
        if (Cnt < 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
            {
                Cnt = 75;
                SelectButton--;
                if (SelectButton < 0)
                {
                    SelectButton = MaxButton;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
            {
                Cnt = 75;
                SelectButton++;
                if (SelectButton > MaxButton)
                {
                    SelectButton = 0;
                }
            }

            //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
            //{
                
            //}
            //if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
            //{
                
            //}
        }
        
        switch (SelectButton)
        {
            case 0:
                MasterSlider.Select();
                break;

            case 1:
                BGMSlider.Select();
                break;

            case 2:
                SESlider.Select();
                break;

            case 3:
                BackButton.Select();
                break;
        }
    }
}
