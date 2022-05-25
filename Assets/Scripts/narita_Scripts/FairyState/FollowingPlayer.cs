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

    // プレイヤーに追従するように
    public override void UpdateFairy()
    {
        controller = GameObject.Find("MainCamera");
        outsideLine = GameObject.Find("cursor");

        // 破り始めフラグが立っていたらS TATE_DICISION_BREAKING_POINT
        if (outsideLine.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg())
        {
            //--- サブカメラ側の妖精を見えなくする

            //--- ステート遷移（最初の座標確定
            FairyState.SetState(eFairyState.STATE_DICISION_BREAKING_POINT);
        }

        Debug.Log("FOLLOW");
    }
}
