using System;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    // �Z�[�u����������---------------------------------------------------------------
    public int Progress;
    public string[] Timer = new string[8];
    public int[] Star = new int[8];
    public int[,] ClearTime = new int[8, 3]{
        { 30, 100, 130 },     // �X�e�[�W1
        { 30, 100, 130 },     // �X�e�[�W2
        { 30, 100, 130 },     // �X�e�[�W3
        { 30, 100, 130 },     // �X�e�[�W4
        { 30, 100, 130 },     // �X�e�[�W5
        { 30, 100, 130 },     // �X�e�[�W6
        { 30, 100, 130 },     // �X�e�[�W7
        { 30, 100, 130 }      // �X�e�[�W8
    };
    //--------------------------------------------------------------------------------
}