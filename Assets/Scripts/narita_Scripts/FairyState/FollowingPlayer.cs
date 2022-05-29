using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : FairyState
{

    GameObject fairy;

    public static FollowingPlayer Instantiate(GameObject game)
    {
        FollowingPlayer fp = new FollowingPlayer();
        // �d�������ݒ肵�Ă���
        fp.fairy = game;

        return fp;
    }



    private GameObject controller;
    private GameObject outsideLine;

    private void Start()
    {
        //fairy = GameObject

        controller = GameObject.Find("MainCamera");
        outsideLine = GameObject.Find("cursor");
    }

    // �v���C���[�ɒǏ]����悤��
    public override void UpdateFairy()
    {
        controller = GameObject.Find("MainCamera");
        outsideLine = GameObject.Find("cursor");

        // �j��n�߃t���O�������Ă�����S TATE_DICISION_BREAKING_POINT
        if (outsideLine.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg())
        {
            //--- �T�u�J�������̗d���������Ȃ�����
            List<GameObject> fairys = new List<GameObject>();
            fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
            foreach (var fairy in fairys)
            {
                // ---�����ŃX�P�[��������������t���O��ON�ɂ���
                fairy.GetComponent<Fiary_Move>().SmallStart();
            }

            //--- �X�e�[�g�J�ځi�ŏ��̍��W�m��
            var fs = fairy.GetComponent<Fiary_Script>();
            fs.SetState(Fiary_Script.eFairyState.STATE_DICISION_BREAKING_POINT);
            fs.BigStart();
        }
    }
}
