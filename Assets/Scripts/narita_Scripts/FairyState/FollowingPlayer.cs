using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : FairyState
{
    private GameObject controller;
    private GameObject outsideLine;

    private void Start()
    {
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

            //--- �X�e�[�g�J�ځi�ŏ��̍��W�m��
            FairyState.SetState(eFairyState.STATE_DICISION_BREAKING_POINT);
        }

        Debug.Log("FOLLOW");
    }
}
