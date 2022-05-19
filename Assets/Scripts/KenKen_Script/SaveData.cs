using System;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    // �Z�[�u����������---------------------------------------------------------------
    public int Progress;
    public string[] Timer = new string[9];
    public int[] Star = new int[9];
    public int[,] ClearTime = new int[9, 3]{
        {9999,9999,9999 },    // �`���[�g���A��
        { 25, 30, 40 },     // �X�e�[�W1
        { 25, 30, 40 },     // �X�e�[�W2
        { 35, 40, 50 },     // �X�e�[�W3
        { 30, 40, 50 },     // �X�e�[�W4
        { 30, 40, 50 },     // �X�e�[�W5
        { 35, 45, 55 },     // �X�e�[�W6
        { 40, 45, 55 },     // �X�e�[�W7
        { 45, 55, 65 }      // �X�e�[�W8
    };
    //--------------------------------------------------------------------------------
}