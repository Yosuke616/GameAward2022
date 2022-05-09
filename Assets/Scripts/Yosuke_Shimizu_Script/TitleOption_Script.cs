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
    public Button BackButton;

    // ↑の数
    private int MaxButton = 3;

    // 現在選択しているもの用
    private int SelectButton = 0;

    // 入力間隔
    private int Cnt = 0;

    //このふらぐでこれが使えるかどうかを決める
    private bool TOFlg;

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
        BGMSlider.value = Valume.BGMVal;
        //MasterSlider.value = Valume.MasterVal;
        SESlider.value = Valume.SEVal;

        TOFlg = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (!TOFlg)
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

            //色は常に黒色に変えておく
            GameObject MasterText = GameObject.Find("MasterText");
            GameObject BGMText = GameObject.Find("BGMText");
            GameObject FEText = GameObject.Find("SEText");
            MasterText.GetComponent<Text>().color = new Color(1f / 255f, 1f / 255f, 1f / 255f);
            BGMText.GetComponent<Text>().color = new Color(1f / 255f, 1f / 255f, 1f / 255f);
            FEText.GetComponent<Text>().color = new Color(1f / 255f, 1f / 255f, 1f / 255f);
            BackButton.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            //選ばれたら赤く変える
            switch (SelectButton)
            {
                case 0:
                    MasterSlider.Select();
                    MasterText.GetComponent<Text>().color = new Color(255f / 255f, 1f / 255f, 1f / 255f);
                    Debug.Log("マスター");
                    break;

                case 1:
                    BGMSlider.Select();
                    BGMText.GetComponent<Text>().color = new Color(255f / 255f, 1f / 255f, 1f / 255f);
                    Debug.Log("BGMすらーだいー");
                    break;

                case 2:
                    SESlider.Select();
                    FEText.GetComponent<Text>().color = new Color(255f / 255f, 1f / 255f, 1f / 255f);
                    Debug.Log("効果音");
                    break;

                case 3:
                    BackButton.Select();
                    BackButton.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    Debug.Log("タイトルへ戻す");

                    break;
            }

            if (SelectButton == 3) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
                {
                    TOFlg = true;
                    Debug.Log("ロゼッター");
                    GameObject obj = GameObject.Find("Main Camera");
                    obj.GetComponent<Title_Scrupt>().SetTitleFlg(false);
                }
            }

            //左右でスライダーの値を変える
            SetSlider();

            if (Input.GetKeyDown("joystick button 0"))
            {
                TOFlg = true;
                Debug.Log("ロゼッター");
                GameObject obj = GameObject.Find("Main Camera");
                obj.GetComponent<Title_Scrupt>().SetTitleFlg(false);
            }
        }
    }

    //スライダーを移動させる関数
    private void SetSlider()
    {
        MasterSlider.value = Valume.MasterVal;
        BGMSlider.value = Valume.BGMVal;
        SESlider.value = Valume.SEVal;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal2") > 0)
        {
            switch (SelectButton)
            {
                case 0:
                    Valume.MasterVal -= 0.05f;
                    if (Valume.MasterVal < 0.0f)
                    {
                        Valume.MasterVal = 0.0f;
                    }
                    MasterSlider.value = Valume.MasterVal;
                    break;
                case 1:
                    Valume.BGMVal -= 0.05f;
                    if (Valume.BGMVal < 0.0f)
                    {
                        Valume.BGMVal = 0.0f;
                    }
                    BGMSlider.value = Valume.BGMVal;
                    break;
                case 2:
                    Valume.SEVal -= 0.05f;
                    if (Valume.SEVal < 0.0f)
                    {
                        Valume.SEVal = 0.0f;
                    }
                    SESlider.value = Valume.SEVal;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal2") < 0)
        {
            switch (SelectButton)
            {
                case 0:
                    Valume.MasterVal += 0.05f;
                    if (Valume.MasterVal > 1.0f)
                    {
                        Valume.MasterVal = 1.0f;
                    }
                    MasterSlider.value = Valume.MasterVal;
                    break;
                case 1:
                    Valume.BGMVal += 0.05f;
                    if (Valume.BGMVal > 1.0f)
                    {
                        Valume.BGMVal = 1.0f;
                    }
                    BGMSlider.value = Valume.BGMVal;
                    break;
                case 2:
                    Valume.SEVal += 0.05f;
                    if (Valume.SEVal > 1.0f)
                    {
                        Valume.SEVal = 1.0f;
                    }
                    SESlider.value = Valume.SEVal;
                    break;
            }
        }
    }

    //タイトルのオプションフラグをセットするための関数
    public void SetTitleOption(bool TOflg) {
        TOFlg = TOflg;
    }

}
