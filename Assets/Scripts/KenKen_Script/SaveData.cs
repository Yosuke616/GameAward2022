using System;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    // セーブしたいもの---------------------------------------------------------------
    public int Progress;
    public int[] Star = new int[9];
    public string[] Timer = new string[9];
    public string[,] ClearTime = new string[9, 3]{
        { "99:99", "00:00", "00:00" },    // チュートリアル
        { "00:25", "00:30", "00:40" },     // ステージ1
        { "00:25", "00:30", "00:40" },     // ステージ2
        { "00:35", "00:40", "00:50" },     // ステージ3
        { "00:30", "00:40", "00:50" },     // ステージ4
        { "00:30", "00:40", "00:50" },     // ステージ5
        { "00:35", "00:45", "00:55" },     // ステージ6
        { "00:40", "00:45", "00:55" },     // ステージ7
        { "00:45", "00:55", "00:65" }      // ステージ8
    };
    //--------------------------------------------------------------------------------
}