/*
�쐬���F2022/2/23 Shimizu Yosuke 
���e  �F�V�[���J�ڂ��ł���悤�ɂ�����
        �t�@���N�V�����L�[�ňړ��ł���
        ���ꂼ��̃V�[�����ƂɈړ��ł���悤�ɂ���
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;     //�V�[���J�ڂ��邽�߂ɕK�v�ȃC���N���[�h�݂����Ȃ��

public class Scenetransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F1)) {
            SceneManager.LoadScene("PeparCutTest");
        }
        if (Input.GetKey(KeyCode.F2))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F3))
        {
            SceneManager.LoadScene("PlayerMove");
        }
        if (Input.GetKey(KeyCode.F4))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F5))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F6))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F7))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F8))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F9))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F10))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F11))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKey(KeyCode.F12))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
