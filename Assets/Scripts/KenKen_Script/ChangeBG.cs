using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBG : MonoBehaviour
{
    // 背景テクスチャ-----------------------------------------------------------------
    public Texture stage0_BG;
    public Texture stage1_BG;
    public Texture stage2_BG;
    public Texture stage3_BG;
    public Texture stage4_BG;
    public Texture stage5_BG;
    public Texture stage6_BG;
    public Texture stage7_BG;
    public Texture stage8_BG;

    public SpriteRenderer BackFade;     // フェード用
    //--------------------------------------------------------------------------------


    // 変更用-------------------------------------------------------------------------
    private Texture[] Stage_BG;         // 背景を配列に
    private static int SelectBG = 0;    // 現在の背景

    public enum FADE_TYPE
    {
        FADE_NONE,
        FADE_OUT,
        FADE_IN,
    }
    public static FADE_TYPE fade_type = FADE_TYPE.FADE_NONE;
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // テクスチャを配列に突っ込む
        Stage_BG = new Texture[] { stage0_BG, stage1_BG, stage2_BG, stage3_BG, stage4_BG, stage5_BG, stage6_BG, stage7_BG, stage8_BG};

        // スプライト取得;
    }


    // Update is called once per frame
    void Update()
    {
        // 背景状態
        switch(fade_type)
        {
            case FADE_TYPE.FADE_OUT:
                BackFade.color += new Color(0, 0, 0, 0.01f);
                if (BackFade.color.a >= 1)
                {
                    BackFade.color = new Color(0, 0, 0, 1);
                    fade_type = FADE_TYPE.FADE_IN;

                    // テクスチャ変更
                    GetComponent<Renderer>().material.mainTexture = Stage_BG[SelectBG];
                }
                break;


            case FADE_TYPE.FADE_IN:
                BackFade.color -= new Color(0, 0, 0, 0.01f);
                if (BackFade.color.a <= 0)
                {
                    BackFade.color = new Color(0, 0, 0, 0);
                    fade_type = FADE_TYPE.FADE_NONE;
                }
                break;


            case FADE_TYPE.FADE_NONE:
                break;
        }
    }


    // 背景テクスチャ変更用関数-------------------------------------------------------
    public static void ChangeBg(int Select)
    {
        SelectBG = Select;
        fade_type = FADE_TYPE.FADE_OUT;
    }
    //--------------------------------------------------------------------------------
}
