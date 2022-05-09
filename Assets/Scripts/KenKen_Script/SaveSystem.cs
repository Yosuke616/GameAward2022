using UnityEngine;
using System.IO;

public class SaveSystem
{
    //�Z�[�u
    // �C���X�^���X����SaveData�A�ۑ���Afalse = �㏑���Atrue = �����ď���
    public static void Save(object instance, string PATH, bool overWrite = false)
    {
        string jsonstr = JsonUtility.ToJson(instance);
        var writer = new StreamWriter(PATH, overWrite);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        Debug.Log("Saved!");
    }

    //���[�h
    public static string Load(string PATH)
    {
        var reader = new StreamReader(PATH);
        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log("Loaded!");
        return json;
    }
}
