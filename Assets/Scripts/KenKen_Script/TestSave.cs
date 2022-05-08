using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    //�ۑ�����t�@�C���̃p�X
    private string PATH;

    void Start()
    {
        //SAVE_FILE_NAME�́@"hoge.json"�@�̂悤�ȃt�@�C����
        PATH = Application.dataPath + "/SaveData/" + "hogehoge.json";
    }

    //�Z�[�u�������N���X or �\���́@���C���X�^���X��
    SaveData saveData = new SaveData();

    void Update()
    {
        //S�L�[�������ăZ�[�u    
        if (Input.GetKeyDown(KeyCode.S))
        {
            saveData.Progress = StageSelect.ProgressStages;

            SaveSystem.Save(saveData, PATH);

            //L�L�[�������ă��[�h
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            var Data
              = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));

            StageSelect.ProgressStages = Data.Progress;
        }
    }
}
