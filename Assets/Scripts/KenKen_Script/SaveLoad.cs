using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveLoad : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    // �ۑ�����t�@�C���̃p�X
    private static string PATH;

    //�Z�[�u�������N���X or �\���́@���C���X�^���X��
    private static SaveData saveData = new SaveData();
    //--------------------------------------------------------------------------------


    void Start()
    {
        // �Z�[�u����ꏊ�Ɩ��O
        PATH = Application.dataPath + "/SaveData/" + "hogehoge.json";
    }

    
    // �f�[�^�Z�[�u�֐�---------------------------------------------------------------
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

    
    // �f�[�^���[�h�֐�---------------------------------------------------------------
    // SaveData�^�Ńf�[�^��Ԃ�
    public static SaveData LoadData()
    {
        var Data = JsonUtility.FromJson<SaveData>(SaveSystem.Load(PATH));
        return saveData;
    }
    //--------------------------------------------------------------------------------
}

