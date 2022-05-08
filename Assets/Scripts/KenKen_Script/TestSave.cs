using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    //保存するファイルのパス
    private string PATH;

    void Start()
    {
        //SAVE_FILE_NAMEは　"hoge.json"　のようなファイル名
        PATH = Application.dataPath + "/SaveData/" + "hogehoge.json";
    }

    //セーブしたいクラス or 構造体　をインスタンス化
    SaveData saveData = new SaveData();

    void Update()
    {
        //Sキーをおしてセーブ    
        if (Input.GetKeyDown(KeyCode.S))
        {
            saveData.Progress = StageSelect.ProgressStages;

            SaveSystem.Save(saveData, PATH);

            //Lキーをおしてロード
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            var Data
              = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));

            StageSelect.ProgressStages = Data.Progress;
        }
    }
}
