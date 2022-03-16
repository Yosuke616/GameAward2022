using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �σN���X�^�̃V���O���g���N���X
/// </summary>

public class SingletonMonoBehaviour<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T _instance = null;

    /// <summary>
    /// get-only�ȃv���p�e�B
    /// <summary>
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                // typeof(T)�^�̃I�u�W�F�N�g���擾
                _instance = (T)FindObjectOfType(typeof(T));

                // �擾�ł��Ȃ������Anull�������ꍇ
                if(_instance == null)
                {
                    Debug.LogError(typeof(T) + "���V�[���ɑ��݂��܂���");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        CheckInstance();

        // Unity�ł͊�{�I�ɃV�[����؂�ւ����GameObject���͑S���폜�����
        // ���L��DontDestroyOnLoad�͈����Ɏw�肵��GameObject�͔j������Ȃ��Ȃ�A
        // �V�[���؂�ւ����ɂ��̂܂܈����p�����E
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// �V�[�����ɈقȂ�C���X�^���X�����邩�ǂ���
    /// </summary>
    private void CheckInstance()
    {
        // �قȂ�C���X�^���X���������ꍇ(�ŏ��ɐ��������C���X�^���X�ȊO�����݂�����
        if(this != Instance)
        {
            // �폜����
            Destroy(this.gameObject);
            return;
        }
    }

}
