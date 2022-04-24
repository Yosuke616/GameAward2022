using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField]
    private Image _backGround;
    [SerializeField]
    private GameObject _loadDisplay;
    [SerializeField]
    private Text _percentage;
    [SerializeField]
    private Slider _progressbar;
    [SerializeField]
    private float FADESPEED = 0.1f;
    [SerializeField]
    private float valueSpeed;

    private float currentSpeed;

    private string _nextScreenName;
    private bool _isFadeIn, _isFadeOut, _useFadeIn, _useFadeOut;

    // �񓯊�����Ŏg�p����
    private AsyncOperation _async;

    // Update is called once per frame
    void Update()
    {
        if (_isFadeOut)
        {
            FadeOut();
        }
        else if (_isFadeIn)
        {
            FadeIn();
        }
    }
    /// <summary>
    /// �t�F�[�h�J�n
    /// </summary>
    public void FadeStart(string nextSceneName)
    {
        _nextScreenName = nextSceneName;
        _useFadeOut = true;
        _useFadeIn = true;

        _backGround.gameObject.SetActive(true);

        // �t�F�[�h�A�E�g�̊J�n
        _isFadeOut = true;
        currentSpeed = valueSpeed;
    }
    /// <summary>
    /// �t�F�[�h�A�E�g�̏���
    /// </summary>
    private void FadeOut()
    {
        // �t�F�[�h�A�E�g�����邩�ǂ���
        if (_useFadeOut)
        {
            //_backGround.color += new Color(0.0f, 0.0f, 0.0f, FADESPEED * Time.deltaTime);
            valueSpeed += FADESPEED * Time.deltaTime;
            _backGround.material.SetFloat("_Value", valueSpeed);

            SoundManager.Instance.Volume -= FADESPEED * Time.deltaTime;

            // ��ʂ��Â��Ȃ�����
            if (valueSpeed >= 1.0f)
            {
                SoundManager.Instance.StopBgm();
                SoundManager.Instance.StopSe();
                _loadDisplay.SetActive(true);
                StartCoroutine("LoadScene");
                _isFadeOut = false;
            }
        }
        else
        {
            _backGround.color = Color.black;
            _loadDisplay.SetActive(true);
            StartCoroutine("LoadScene");
            _isFadeOut = false;
        }
    }
    /// <summary>
    /// �t�F�[�h�C���̏���
    /// </summary>
    private void FadeIn()
    {
        // �t�F�[�h�����邩�ǂ���
        if (_useFadeIn)
        {
            _loadDisplay.SetActive(false);
            //_backGround.color -= new Color(0.0f, 0.0f, 0.0f, FADESPEED * Time.deltaTime);
            valueSpeed -= FADESPEED * Time.deltaTime;
            _backGround.material.SetFloat("_Value", valueSpeed);

            SoundManager.Instance.Volume += FADESPEED * Time.deltaTime;

            // ��ʂ����邭�Ȃ�����
            if (valueSpeed <= currentSpeed)
            {
                SoundManager.Instance.Volume = 1;
                _backGround.gameObject.SetActive(false);
                _isFadeIn = false;
            }
        }
        else
        {
            SoundManager.Instance.Volume = 1;
            _backGround.gameObject.SetActive(false);
            _isFadeIn = false;
        }
    }
    /// <summary>
    /// ���[�h(NowLoading)�R�[���`��
    /// </summary>
    IEnumerator LoadScene()
    {
        // �V�[���̓ǂݍ��݂�����
        _async = SceneManager.LoadSceneAsync(_nextScreenName);

        // �V�[���̓ǂݍ��݊����������ōs���Ȃ��悤�ɂ���
        _async.allowSceneActivation = false;

        // progress��0.9�ȉ�(���[�h��)�̊ԌJ��Ԃ�
        while (_async.progress < 0.9f)
        {
            // ���[�h�̐i�s����p�[�Z���g�ŕ\��
            _percentage.text = (_async.progress * 100.0f).ToString("F0") + "%";

            // ���[�h�̐i�s����X���C�_�[�ɔ��f
            _progressbar.value = _async.progress;
        }
        _percentage.text = "100%";
        _progressbar.value = 1.0f;

        // 1�b�ԑҋ@
        yield return new WaitForSeconds(1);

        // �V�[���̓ǂݍ��݊���
        _async.allowSceneActivation = true;
        _isFadeIn = true;

        yield return null;
    }
}