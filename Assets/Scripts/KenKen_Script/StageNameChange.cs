using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNameChange : MonoBehaviour
{
    // ステージネーム画像-------------------------------------------------------------
    public Sprite Name_0;
    public Sprite Name_1;
    public Sprite Name_2;
    public Sprite Name_3;
    public Sprite Name_4;
    public Sprite Name_5;
    public Sprite Name_6;
    public Sprite Name_7;
    public Sprite Name_8;

    private Sprite[] Names;
    private static int Select = 0;

    public Image image;
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        Names = new Sprite[] { Name_0, Name_1, Name_2, Name_3, Name_4, Name_5, Name_6, Name_7, Name_8 };
    }


    // Update is called once per frame
    void Update()
    {
        image.sprite = Names[Select];
    }


    // 画像変更-----------------------------------------------------------------------
    public static void ChangeStageName(int select)
    {
        Select = select;
    }
    //--------------------------------------------------------------------------------

}
