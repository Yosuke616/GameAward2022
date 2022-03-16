using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // セーブ先のファイル
    string filePath;

    // SaveData.csの保存したいもの
    SaveData save;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        save = new SaveData();
    }

    // 以下のSave関数やLoad関数を呼び出して使用
    public void Save()
    {
        string json = JsonUtility.ToJson(save);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            save = JsonUtility.FromJson<SaveData>(data);
        }
    }
}