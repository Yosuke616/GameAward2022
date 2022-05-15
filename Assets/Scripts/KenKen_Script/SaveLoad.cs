using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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


    // �X�e�[�W���̃Z�[�u�֐�---------------------------------------------------------
    public static void SaveStageData(string time, int star)
    {
        // �ǂ̃X�e�[�W��
        int Select = new int();

        // ���݂̃X�e�[�W���擾
        string name = SceneManager.GetActiveScene().name;
        if (name == "1-1") Select = 0;
        if (name == "1-2") Select = 1;
        if (name == "1-3") Select = 2;
        if (name == "1-4") Select = 3;
        if (name == "1-5") Select = 4;
        if (name == "1-6") Select = 5;
        if (name == "1-7") Select = 6;
        if (name == "1-8") Select = 7;

        // �i���x�m�F
        if (saveData.Progress < Select)
            saveData.Progress = Select;

        // �^�C�}�[�X�V�m�F
        int newTime = Convert.ToInt32(time);                         // �^�C�}�[�e�L�X�g�𐔎��ɕϊ�
        int bestTime = Convert.ToInt32(saveData.Timer[Select].text);
        if (bestTime > newTime)
            saveData.Timer[Select].text = time;

        // �l�����m�F
        if (saveData.Star[Select] < star)
            saveData.Star[Select] = star;

        // �Z�[�u
        SaveSystem.Save(saveData, PATH);
    }
    //--------------------------------------------------------------------------------



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

