using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperChange : MonoBehaviour
{
    // ���|���S��
    [SerializeField]
    private Image _paperChange;

    // �t�F�[�h���A���Z���Ă����l
    [SerializeField]
    private float FADESPEED = 0.1f;

    // �t�F�[�h���̐��l
    private float valueSpeed;

    // �t�F�[�h�C���A�t�F�[�h�A�E�g�t���O
    private bool _isFadeIn, _isFadeOut;

    // �E�T�M�A�j���[�V�����p�J����
    public Camera openingCamera;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // �ŏ��͌����Ȃ�����
        _paperChange.gameObject.SetActive(false);

        // �t�F�[�h�A�E�g�t���O
        _isFadeOut = false;

        // �t�F�[�h�C���t���O
        _isFadeIn = false;

        // �t�F�[�h�̐��l
        valueSpeed = 0.0f;

        // �E�T�M�A�j���[�V�����p�J����
        openingCamera.enabled = true;
        mainCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadeOut)
        {
            // �t�F�[�h�A�E�g
            FadeOut();
        }
        else if (_isFadeIn)
        {
            // �t�F�[�h�C��
            FadeIn();
        }
    }

    public void FadeStart()
    {
        // ���|���S���`��
        _paperChange.gameObject.SetActive(true);

        // �t�F�[�h�A�E�g�J�n
        _isFadeOut = true;
    }

    private void FadeOut()
    {
        // �t�F�[�h�l
        valueSpeed += FADESPEED * Time.deltaTime;

        // Shader�ɒl��n��
        _paperChange.material.SetFloat("_Value", valueSpeed);

        // �t�F�[�h�A�E�g����
        if (valueSpeed >= 1.0f)
        {
            // �l�␳
            valueSpeed = 1.0f;

            // �t�F�[�h�A�E�g�I��
            _isFadeOut = false;

            // �t�F�[�h�C���J�n
            _isFadeIn = true;

            // �^�C�}�[�J�n
            var setTime = GameObject.Find("Time").GetComponentInChildren<TimerScript>();
            setTime.TimerStart();

            // �E�T�M�A�j���[�V�����I��
            openingCamera.enabled = false;
            mainCamera.enabled = true;

            // �I�[�v�j���O���[�h����A�N�V�������[�h�ɐ؂�ւ���
            CursorSystem.SetGameState(CursorSystem.GameState.MODE_ACTION);
        }
    }

    private void FadeIn()
    {
        // �t�F�[�h�l
        valueSpeed -= FADESPEED * Time.deltaTime;

        // Shader�ɒl��n��
        _paperChange.material.SetFloat("_Value", valueSpeed);

        // �t�F�[�h�C������
        if (valueSpeed <= 0.0f)
        {
            // �l�␳
            valueSpeed = 0.0f;

            // ���|���S���������Ȃ�����
            _paperChange.gameObject.SetActive(false);

            // �t�F�[�h�C���I��
            _isFadeIn = false;
        }
    }
}
