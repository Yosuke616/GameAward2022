using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicisionBreakingPoint : FairyState
{
    Vector3 firstBreakPoint;
    GameObject outsideLine = GameObject.Find("cursor");

    void Start()
    {
        
    }

    public override void UpdateFairy()
    {
        Debug.Log("DICISION");
        //outsideLine = GameObject.Find("cursor");
        //var outsider = outsideLine.GetComponent<OutSide_Paper_Script_Second>();
        //
        //// ç¿ïWå≈íË
        //if(outsider)
        //{
        //    Vector2 pos = outsider.GetCursorPos();
        //    this.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        //}
    }
}
