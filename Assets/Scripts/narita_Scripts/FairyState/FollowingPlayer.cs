using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : FairyState
{

    GameObject fairy;

    public static FollowingPlayer Instantiate(GameObject game)
    {
        FollowingPlayer fp = new FollowingPlayer();
        // 妖精さんを設定しておく
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

    // プレイヤーに追従するように
    public override void UpdateFairy()
    {
        controller = GameObject.Find("MainCamera");
        outsideLine = GameObject.Find("cursor");

        // 破り始めフラグが立っていたらS TATE_DICISION_BREAKING_POINT
        if (outsideLine.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg())
        {
            //--- サブカメラ側の妖精を見えなくする
            List<GameObject> fairys = new List<GameObject>();
            fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
            foreach (var fairy in fairys)
            {
                // ---ここでスケールを小さくするフラグをONにする
                fairy.GetComponent<Fiary_Move>().SmallStart();
            }

            //--- ステート遷移（最初の座標確定
            var fs = fairy.GetComponent<Fiary_Script>();
            fs.SetState(Fiary_Script.eFairyState.STATE_DICISION_BREAKING_POINT);
            fs.BigStart();
        }
    }
}
