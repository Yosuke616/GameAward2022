using System;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    // セーブしたいもの---------------------------------------------------------------
    public int Progress;
    public string[] Timer = new string[8];
    public int[] Star = new int[8];
    //--------------------------------------------------------------------------------
}