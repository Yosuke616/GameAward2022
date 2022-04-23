using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�|�[�Y�}�l�[�W���[�p�̒ǉ�
using UnityEngine.SceneManagement;

public class PauseContorol : MonoBehaviour
{
    // �|�[�Y�p�l���I��
    public GameObject PauseObj;

    // �|�[�Y�p�l����������ꏊ
    public Transform parant;

    //�~�߂�I�u�W�F�N�g
    public GameObject gameobject;

    // Start is called before the first frame update
    void Start()
    {
        // �v���n�u����
        PauseObj = Instantiate(PauseObj, parant);
        PauseObj.SetActive(false);
        OnUnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
            PauseObj.SetActive(!PauseObj.activeSelf);
        }

        //�@�|�[�YUI���\������Ă鎞�͒�~
        if (PauseObj.activeSelf)
        {
            OnPause();
        }
        else
        {
            OnUnPause();
        }

        Debug.Log(gameobject.transform.childCount);

    }

    //�|�[�Y���j���[�Ɉڍs���邽�߂̊֐�
    public void OnPause() {
        Time.timeScale = 0f;
        //�q�I�u�W�F�N�g�̑S�ẴX�N���v�g���~�߂�
        StopUpdate(gameobject.transform);
    }

    //�|�[�Y���j���[���������邽�߂̊֐�
    public void OnUnPause() {
        Time.timeScale = 1f;
        //�q�I�u�W�F�N�g�̑S�ẴX�N���v�g���ĊJ
        StartUpdate(gameobject.transform);
    }

    //�q�I�u�W�F�N�g�̃A�b�v�f�[�g���~�߂邽�߂̊֐�
    void StopUpdate(Transform GO) {
       
        for (int i = 0; i < GO.childCount; i++)
        {
            Transform child = GO.GetChild(i);
            StopUpdate(child);
            foreach (MonoBehaviour mb in child.gameObject.GetComponents<MonoBehaviour>()) {
                mb.enabled = false;
            }
            
        }
    }

    //�q�I�u�W�F�N�g�̃A�b�v�f�[�g���ĊJ����֐�
    void StartUpdate(Transform GO) {
        for (int i = 0; i < GO.childCount; i++)
        {
            Transform child = GO.GetChild(i);
            StartUpdate(child);
            foreach (MonoBehaviour mb in child.gameObject.GetComponents<MonoBehaviour>())
            {
                mb.enabled = true;
            }
        }
    }
}
