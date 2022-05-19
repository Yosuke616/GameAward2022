using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBG : MonoBehaviour
{
    // �w�i�e�N�X�`��-----------------------------------------------------------------
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


    // �ύX�p-------------------------------------------------------------------------
    private Texture[] Stage_BG;         // �w�i��z���
    private static int SelectBG = 0;    // ���݂̔w�i
    //--------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // �e�N�X�`����z��ɓ˂�����
        Stage_BG = new Texture[] { stage0_BG, stage1_BG, stage2_BG, stage3_BG, stage4_BG, stage5_BG, stage6_BG, stage7_BG, stage8_BG};
    }


    // Update is called once per frame
    void Update()
    {
        // �e�N�X�`���ύX
        GetComponent<Renderer>().material.mainTexture = Stage_BG[SelectBG];
    }


    // �w�i�e�N�X�`���ύX�p�֐�-------------------------------------------------------
    public static void ChangeBg(int Select)
    {
        SelectBG = Select;
    }
    //--------------------------------------------------------------------------------
}
