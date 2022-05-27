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
    public GameObject BackButton;
    public GameObject BackButtonback;

    //Fill��ς��邽�߂̃��m
    public GameObject Fill1;
    public GameObject Fill2;
    public GameObject Fill3;

    // ���̐�
    private int MaxButton = 3;

    // ���ݑI�����Ă�����̗p
    private int SelectButton = 0;

    // ���͊Ԋu
    private int Cnt = 0;

    public GameObject Pausepanel;
    public GameObject Optionpanel;
    public GameObject PauseUi;

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

    }

    // Update is called once per frame
    void Update()
    {


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
        Fill2.GetComponent<Image>().color = new Color32(255,255,255,255);
        Fill3.GetComponent<Image>().color = new Color32(255,255,255,255);
        BackButton.SetActive(false);
        BackButtonback.SetActive(true);

        //�I�΂ꂽ��Ԃ��ς���
        switch (SelectButton)
        {
            case 0:
                MasterSlider.Select();
                Fill1.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
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
                // BackButton.Select();
                BackButton.SetActive(true);
                BackButtonback.SetActive(false);
                Debug.Log("�ĊJ");

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0")) {
                    GameObject Sarch = GameObject.Find("PausePanel(Clone)");
                    Sarch.GetComponent<Pause>().SetPauseFlg(false);
                    Pausepanel.SetActive(true);
                    Optionpanel.SetActive(false);
                    PauseUi.SetActive(false);
                    Time.timeScale = 1f;
                }

                break;
        }

        //���E�ŃX���C�_�[�̒l��ς���
        SetSlider();

        if (Input.GetKeyDown("joystick button 1")) {
            GameObject Sarch = GameObject.Find("PausePanel(Clone)");
            Sarch.GetComponent<Pause>().OffOption();
        }

    }

    //�X���C�_�[���ړ�������֐�
    private void SetSlider() {
        MasterSlider.value = Valume.MasterVal;
        BGMSlider.value = Valume.BGMVal;
        SESlider.value = Valume.SEVal;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
        {
            switch (SelectButton) {
                case 0:
                    Valume.MasterVal -= 0.05f;
                    if (Valume.MasterVal < 0.0f) {
                        Valume.MasterVal = 0.0f;
                    }
                    MasterSlider.value = Valume.MasterVal;
                    break;
                case 1:
                    Valume.BGMVal -= 0.05f;
                    if (Valume.BGMVal < 0.0f){
                        Valume.BGMVal = 0.0f;
                    }
                    BGMSlider.value = Valume.BGMVal;
                    break;
                case 2:
                    Valume.SEVal -= 0.05f;
                    if (Valume.SEVal < 0.0f){
                        Valume.SEVal = 0.0f;
                    }
                    SESlider.value = Valume.SEVal;
                    break;
            }   
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
        {
            switch (SelectButton)
            {
                case 0:
                    Valume.MasterVal += 0.05f;
                    if (Valume.MasterVal > 1.0f){
                        Valume.MasterVal = 1.0f;
                    }
                    MasterSlider.value = Valume.MasterVal;
                    break;
                case 1:
                    Valume.BGMVal += 0.05f;
                    if (Valume.BGMVal > 1.0f){
                        Valume.BGMVal = 1.0f;
                    }
                    BGMSlider.value = Valume.BGMVal;
                    break;
                case 2:
                    Valume.SEVal += 0.05f;
                    if (Valume.SEVal > 1.0f){
                        Valume.SEVal = 1.0f;
                    }
                    SESlider.value = Valume.SEVal;
                    break;
            }
        }
    }

}
