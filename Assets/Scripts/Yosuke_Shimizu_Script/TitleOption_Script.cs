using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleOption_Script : MonoBehaviour
{
    // �e�X���C�_�[
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SESlider;
    public GameObject BackButton;
    public GameObject BackButtonback;

    public GameObject Fill1;
    public GameObject Fill2;
    public GameObject Fill3;


    // ���̐�
    private int MaxButton = 3;

    // ���ݑI�����Ă�����̗p
    private int SelectButton = 0;

    // ���͊Ԋu
    private int Cnt = 0;

    //���̂ӂ炮�ł��ꂪ�g���邩�ǂ��������߂�
   // private bool TOFlg;
    //public GameObject Plate;

    //�t���O�Ō����邩�����Ȃ����𔦕��E����
    public GameObject Option;

    // Start is called before the first frame update
    void Start()
    {
        //�ő�l
        MasterSlider.maxValue = 1.0f;
        BGMSlider.maxValue = 1.0f;
        SESlider.maxValue = 1.0f;
        //�ŏ��l
        MasterSlider.minValue = 0.0f;
        BGMSlider.minValue = 0.0f;
        SESlider.minValue = 0.0f;

        //�X���C�_�[���̂̏����̉��̐ݒ�
        MasterSlider.value = Valume.MasterVal;
        BGMSlider.value = Valume.BGMVal;
        SESlider.value = Valume.SEVal;

        //TOFlg = true;

        //Option.SetActive(false);
        //Plate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

       // if (!TOFlg)
        //{
            Cnt--;
            if (Cnt < 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
                {
                    Cnt = 10;
                    SelectButton--;
                    if (SelectButton < 0)
                    {
                        SelectButton = MaxButton;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
                {
                    Cnt = 10;
                    SelectButton++;
                    if (SelectButton > MaxButton)
                    {
                        SelectButton = 0;
                    }
                }
            }

            //�F�͏�ɍ��F�ɕς��Ă���
            GameObject MasterText = GameObject.Find("MasterText");
            GameObject BGMText = GameObject.Find("BGMText");
            GameObject FEText = GameObject.Find("SEText");
            Fill1.GetComponent<Image>().color = new Color32(255,255,255,255);
            Fill2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            Fill3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            BackButton.SetActive(false);
            BackButtonback.SetActive(true);

            //�I�΂ꂽ��Ԃ��ς���
            switch (SelectButton)
            {
                case 0:
                    MasterSlider.Select();
                    Fill1.GetComponent<Image>().color = new Color32(255,0, 0, 255);
                    Debug.Log("�}�X�^�[");
                    break;

                case 1:
                    BGMSlider.Select();
                    Fill2.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    Debug.Log("BGM����[�����[");
                    break;

                case 2:
                    SESlider.Select();
                    Fill3.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    Debug.Log("���ʉ�");
                    break;

                case 3:
                BackButton.SetActive(true);
                BackButtonback.SetActive(false);
                Debug.Log("�^�C�g���֖߂�");

                    break;
            }

            if (SelectButton == 3) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                {
       //             TOFlg = true;
       //             Option.SetActive(false);
       //             Plate.SetActive(false);
                    Debug.Log("���[�b�^�[");
                   // GameObject obj = GameObject.Find("Main Camera");
                //             obj.GetComponent<Title_Button_Script>().SetTitleFlg(false);

                FadeManager.Instance.FadeStart("Title");

            }
        }       
            //���E�ŃX���C�_�[�̒l��ς���
            SetSlider();

            if (Input.GetKeyDown("joystick button 1"))
            {
       //         TOFlg = true;
                Debug.Log("���[�b�^�[");
                GameObject obj = GameObject.Find("Main Camera");
                obj.GetComponent<Title_Button_Script>().SetTitleFlg(false);
            }
       // }
    }

    //�X���C�_�[���ړ�������֐�
    private void SetSlider()
    {
        float a;

        //a = Valume.MasterVal;
        //MasterSlider.value = a;
        //a = Valume.BGMVal;
        //BGMSlider.value = a;
        //a = Valume.SEVal;
        //SESlider.value = a;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
        {
            switch (SelectButton)
            {
                case 0:
                    MasterSlider.value -= 0.05f;
                    if (MasterSlider.value < 0.0f)
                    {
                        MasterSlider.value = 0.0f;
                    }
                    Valume.MasterVal = MasterSlider.value;
                    break;
                case 1:
                    BGMSlider.value -= 0.05f;
                    if (BGMSlider.value < 0.0f)
                    {
                        BGMSlider.value = 0.0f;
                    }
                    Valume.BGMVal = BGMSlider.value;
                    break;
                case 2:
                    SESlider.value -= 0.05f;
                    if (SESlider.value < 0.0f)
                    {
                        SESlider.value = 0.0f;
                    }
                     Valume.SEVal = SESlider.value;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
        {
            switch (SelectButton)
            {
                case 0:
                    MasterSlider.value += 0.05f;
                    if (MasterSlider.value > 1.0f)
                    {
                        MasterSlider.value = 1.0f;
                    }
                    Valume.MasterVal = MasterSlider.value;
                    break;
                case 1:
                    BGMSlider.value += 0.05f;
                    if (BGMSlider.value > 1.0f)
                    {
                        BGMSlider.value = 1.0f;
                    }
                    Valume.BGMVal = BGMSlider.value;
                    break;
                case 2:
                    SESlider.value += 0.05f;
                    if (SESlider.value > 1.0f)
                    {
                        SESlider.value = 1.0f;
                    }
                    Valume.SEVal = SESlider.value;
                    break;
            }
        }
    }
}
