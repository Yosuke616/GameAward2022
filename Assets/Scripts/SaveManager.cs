using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // �Z�[�u��̃t�@�C��
    string filePath;

    // SaveData.cs�̕ۑ�����������
    SaveData save;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        save = new SaveData();
    }

    // �ȉ���Save�֐���Load�֐����Ăяo���Ďg�p
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