using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicisionBreakingPoint : FairyState
{
    GameObject fairy;
    public static DicisionBreakingPoint Instantiate(GameObject game)
    {
        DicisionBreakingPoint fp = new DicisionBreakingPoint();
        // �d�������ݒ肵�Ă���
        fp.fairy = game;

        return fp;
    }

    Vector3 firstBreakPoint;
    GameObject outsideLine = GameObject.Find("cursor");

    void Start()
    {

    }

    public override void UpdateFairy()
    {
        outsideLine = GameObject.Find("cursor");
        var outsider = outsideLine.GetComponent<OutSide_Paper_Script_Second>();

        // ���W�Œ�
        if (outsider)
        {
            Vector2 pos = outsider.GetCursorPos();
            fairy.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        }
    }
}
