using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveLoad : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    // 保存するファイルのパス
    private static string PATH;

    //セーブしたいクラス or 構造体　をインスタンス化
    private static SaveData saveData = new SaveData();
    //--------------------------------------------------------------------------------


    void Start()
    {
        // セーブする場所と名前
        PATH = Application.dataPath + "/SaveData/" + "hogehoge.json";
    }

    
    // データセーブ関数---------------------------------------------------------------
    public static void SaveData(SaveData data)
    {
        if (saveData.Progress < data.Progress)
        {
            saveData.Progress = data.Progress;
        }

        for (int i = 0; i < 8; i++)
        {
            int BestTime = Convert.ToInt32(saveData.Timer[i].text);
            int NewTime = Convert.ToInt32(data.Timer[i].text);
            if(BestTime > NewTime)
            {
                saveData.Timer[i].text = data.Timer[i].text;
            }

            if (saveData.Star[i] < data.Star[i])
            {
                saveData.Star[i] = data.Star[i];
            }
        }
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------

    
    // データロード関数---------------------------------------------------------------
    // SaveData型でデータを返す
    public static SaveData LoadData()
    {
        var Data = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));
        return saveData;
    }
    //--------------------------------------------------------------------------------
}

