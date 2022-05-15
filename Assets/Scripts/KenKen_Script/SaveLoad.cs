using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SaveLoad : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    // 保存するファイルのパス
    private static string PATH;

    //セーブしたいクラス or 構造体　をインスタンス化
    public static SaveData saveData = new SaveData();
    //--------------------------------------------------------------------------------


    void Start()
    {
        // セーブする場所と名前
        PATH = Application.dataPath + "/SaveData/" + "hogehoge.json";

        // テスト用
        InitSaveData();
    }


    // データ初期化関数---------------------------------------------------------------
    public static void InitSaveData()
    {
        saveData.Progress = 0;
        for (int i = 0; i < 8; i++)
        {
            saveData.Timer[i] = "9999";
            saveData.Star[i] = 0;
        }

        // セーブ
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------


    // ステージ毎のセーブ関数---------------------------------------------------------
    public static void SaveStageData(string time, int star)
    {
        // どのステージか
        int Select = new int();

        // 現在のステージ名取得
        string name = SceneManager.GetActiveScene().name;
        if (name == "1-1") Select = 0;
        if (name == "1-2") Select = 1;
        if (name == "1-3") Select = 2;
        if (name == "1-4") Select = 3;
        if (name == "1-5") Select = 4;
        if (name == "1-6") Select = 5;
        if (name == "1-7") Select = 6;
        if (name == "1-8") Select = 7;

        // 進捗度確認
        if (saveData.Progress < Select)
            saveData.Progress = Select;

        // タイマー更新確認
        int newTime = Convert.ToInt32(time);                         // タイマーテキストを数字に変換
        int bestTime = Convert.ToInt32(saveData.Timer[Select]);
        if (bestTime > newTime)
            saveData.Timer[Select] = time;

        // 獲得★確認
        if (saveData.Star[Select] < star)
            saveData.Star[Select] = star;

        // セーブ
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------



    // データセーブ関数---------------------------------------------------------------
    public static void SaveData(SaveData data)
    {
        if (saveData.Progress < data.Progress)
        {
            saveData.Progress = data.Progress;
        }

        for (int i = 0; i < 8; i++)
        {
            int BestTime = Convert.ToInt32(saveData.Timer[i]);
            int NewTime = Convert.ToInt32(data.Timer[i]);
            if(BestTime > NewTime)
            {
                saveData.Timer[i] = data.Timer[i];
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
    public static void LoadData()
    {
        var Data = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));
        saveData = Data;
    }
    //--------------------------------------------------------------------------------
}

