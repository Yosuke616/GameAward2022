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
    //--------------------------------------------------------------------------------


    // 変更用-------------------------------------------------------------------------
    private Texture[] Stage_BG;         // 背景を配列に
    private static int SelectBG = 0;    // 現在の背景
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // テクスチャを配列に突っ込む
        Stage_BG = new Texture[] { stage0_BG, stage1_BG, stage2_BG, stage3_BG, stage4_BG, stage5_BG, stage6_BG, stage7_BG, stage8_BG};
    }


    // Update is called once per frame
    void Update()
    {
        // テクスチャ変更
        GetComponent<Renderer>().material.mainTexture = Stage_BG[SelectBG];
    }


    // 背景テクスチャ変更用関数-------------------------------------------------------
    public static void ChangeBg(int Select)
    {
        SelectBG = Select;
    }
    //--------------------------------------------------------------------------------
}
