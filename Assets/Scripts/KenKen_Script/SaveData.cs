using System;
[Serializable]
public class SaveData
{
    // セーブしたいもの
    public int Progress;
    public int[] Timer = new int[8];
    public int[] Star = new int[8];
}