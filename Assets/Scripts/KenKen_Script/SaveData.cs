using System;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    // セーブしたいもの---------------------------------------------------------------
    public int Progress;
    public Text[] Timer = new Text[8];
    public int[] Star = new int[8];
    //--------------------------------------------------------------------------------
}