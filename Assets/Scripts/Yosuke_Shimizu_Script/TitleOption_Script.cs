using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleOption_Script : MonoBehaviour
{
    // 各スライダー
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SESlider;
    public GameObject BackButton;
    public GameObject BackButtonback;

    public GameObject Fill1;
    public GameObject Fill2;
    public GameObject Fill3;


    // ↑の数
    private int MaxButton = 3;

    // 現在選択しているもの用
    private int SelectButton = 0;

    // 入力間隔
    private int Cnt = 0;

    //このふらぐでこれが使えるかどうかを決める
   // private bool TOFlg;
    //public GameObject Plate;

    //フラグで見えるか見えないかを幡部ウする
    public GameObject Option;

    // Start is called before the first frame update
    void Start()
    {
        //最大値
        MasterSlider.maxValue = 1.0f;
        BGMSlider.maxValue = 1.0f;
        SESlider.maxValue = 1.0f;
        //最小値
        MasterSlider.minValue = 0.0f;
        BGMSlider.minValue = 0.0f;
        SESlider.minValue = 0.0f;

        //スライダー自体の初期の音の設定
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

            //色は常に黒色に変えておく
            GameObject MasterText = GameObject.Find("MasterText");
            GameObject BGMText = GameObject.Find("BGMText");
            GameObject FEText = GameObject.Find("SEText");
            Fill1.GetComponent<Image>().color = new Color32(255,255,255,255);
            Fill2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            Fill3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            BackButton.SetActive(false);
            BackButtonback.SetActive(true);

            //選ばれたら赤く変える
            switch (SelectButton)
            {
                case 0:
                    MasterSlider.Select();
                    Fill1.GetComponent<Image>().color = new Color32(255,0, 0, 255);
                    Debug.Log("マスター");
                    break;

                case 1:
                    BGMSlider.Select();
                    Fill2.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    Debug.Log("BGMすらーだいー");
                    break;

                case 2:
                    SESlider.Select();
                    Fill3.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    Debug.Log("効果音");
                    break;

                case 3:
                BackButton.SetActive(true);
                BackButtonback.SetActive(false);
                Debug.Log("タイトルへ戻す");

                    break;
            }

            if (SelectButton == 3) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                {
       //             TOFlg = true;
       //             Option.SetActive(false);
       //             Plate.SetActive(false);
                    Debug.Log("ロゼッター");
                   // GameObject obj = GameObject.Find("Main Camera");
                //             obj.GetComponent<Title_Button_Script>().SetTitleFlg(false);

                FadeManager.Instance.FadeStart("Title");

            }
        }       
            //左右でスライダーの値を変える
            SetSlider();

            if (Input.GetKeyDown("joystick button 1"))
            {
       //         TOFlg = true;
                Debug.Log("ロゼッター");
                GameObject obj = GameObject.Find("Main Camera");
                obj.GetComponent<Title_Button_Script>().SetTitleFlg(false);
            }
       // }
    }

    //スライダーを移動させる関数
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
