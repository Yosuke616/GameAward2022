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
    }

    // テスト関数--------------------------------------------------------------------
    public static void TestSaveLoad()
    {
        saveData.Progress = 8;
        for (int i = 0; i < 9; i++)
        {
            saveData.Timer[i] = "12:34";
            saveData.Star[i] = 2;
        }

        // セーブ
        SaveSystem.Save(saveData, PATH);

        var Data = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));
        saveData = Data;

    }
    //--------------------------------------------------------------------------------


    // データ初期化関数---------------------------------------------------------------
    public static void InitSaveData()
    {
        saveData.Progress = 1;
        for (int i = 0; i < 9; i++)
        {
            saveData.Timer[i] = "99:99";
            saveData.Star[i] = 0;
        }

        // セーブ
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------


    // ステージ毎のセーブ関数---------------------------------------------------------
    public static void SaveStageData(string time)
    {
        // どのステージか
        int Select = new int();
        int i = new int();

        // 現在のステージ名取得
        string name = SceneManager.GetActiveScene().name;
        Select = 0;
        if (name == "1-1") Select = 1;
        if (name == "1-2") Select = 2;
        if (name == "1-3") Select = 3;
        if (name == "1-4") Select = 4;
        if (name == "1-5") Select = 5;
        if (name == "1-6") Select = 6;
        if (name == "1-7") Select = 7;
        if (name == "1-8") Select = 8;

        // 進捗度確認
        if (saveData.Progress < Select)
            saveData.Progress = Select;

        // タイマー更新確認
        int newTime = Convert.ToInt32(time);                         // タイマーテキストを数字に変換
        int bestTime = Convert.ToInt32(saveData.Timer[Select]);
        if (bestTime > newTime)
            saveData.Timer[Select] = time;

        // 獲得★確認
        int StarTime = Convert.ToInt32(saveData.Timer[Select]);
        for (i = 0; i < 3; i++)
        {
            if (StarTime <= Convert.ToInt32(saveData.ClearTime[Select, i]))
            {
                saveData.Star[Select] = 3 - i;
                break;
            }
        }

        // セーブ
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------


    // ステージ毎のセーブ関数---------------------------------------------------------
    public static int GetClearStar(string time)
    {
        // どのステージか
        int Select = new int();
        int i = new int();

        // 現在のステージ名取得
        string name = SceneManager.GetActiveScene().name;
        Select = 0;
        if (name == "1-1") Select = 1;
        if (name == "1-2") Select = 2;
        if (name == "1-3") Select = 3;
        if (name == "1-4") Select = 4;
        if (name == "1-5") Select = 5;
        if (name == "1-6") Select = 6;
        if (name == "1-7") Select = 7;
        if (name == "1-8") Select = 8;

        // 進捗度確認
        if (saveData.Progress < Select)
            saveData.Progress = Select;

        // タイマー更新確認
        // :以外にする
        string NewNum = time.Substring(0, 2) + time.Substring(3, 2);
        string BestNum = saveData.Timer[Select].Substring(0, 2) + saveData.Timer[Select].Substring(3, 2);

        // タイマーテキストを数字に変換
        int newTime = Convert.ToInt32(NewNum);
        int bestTime = Convert.ToInt32(BestNum);
        if (bestTime > newTime)
            saveData.Timer[Select] = time;

        // 獲得★確認
        string NowNum = saveData.Timer[Select].Substring(0, 2) + saveData.Timer[Select].Substring(3, 2);
        int StarTime = Convert.ToInt32(NowNum);
        for (i = 0; i < 3; i++)
        {
            if (StarTime <= Convert.ToInt32(saveData.ClearTime[Select, i]))
            {
                saveData.Star[Select] = 3 - i;
                break;
            }
        }

        // セーブ
        SaveSystem.Save(saveData, PATH);

        return saveData.Star[Select];
    }
    //--------------------------------------------------------------------------------




    // データセーブ関数---------------------------------------------------------------
    public static void SaveData(SaveData data)
    {
        if (saveData.Progress < data.Progress)
        {
            saveData.Progress = data.Progress;
        }

        for (int i = 0; i < 9; i++)
        {
            int BestTime = Convert.ToInt32(saveData.Timer[i]);
            int NewTime = Convert.ToInt32(data.Timer[i]);
            if (BestTime > NewTime)
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

