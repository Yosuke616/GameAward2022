/*
作成日：2022/3/15 Shimizu Shogo
内容  ：Fedeの管理クラス
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // フェードのスピード
    private readonly float FADESPEED = 1.0f;

    // 遷移したいシーンの名前
    private string _nextScreenName;
    // フェードフラグ
    private bool _isFadeIn, _isFadeOut, _useFadeIn, _useFadeOut;

    // 非同期操作で使用する
    private AsyncOperation _async;

    // Update is called once per frame
    void Update()
    {
        if(_isFadeOut)
        {
            FadeOut();
        }
        else if(_isFadeIn)
        {
            FadeIn();
        }
    }
    /// <summary>
    /// フェード開始
    /// </summary>
    public void FadeStart(string nextSceneName)
    {
        _nextScreenName = nextSceneName;
        _useFadeOut = true;
        _useFadeIn = true;

        _backGround.gameObject.SetActive(true);

        SoundManager.Instance.StopBgm();
        
        // フェードアウトの開始
        _isFadeOut = true;
    }
    /// <summary>
    /// フェードアウトの処理
    /// </summary>
    private void FadeOut()
    {
        // フェードアウトさせるかどうか
        if(_useFadeOut)
        {
            _backGround.color += new Color(0.0f, 0.0f, 0.0f, FADESPEED * Time.deltaTime);
            // 画面が暗くなったら
            if(_backGround.color.a >= 1.0f)
            {
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
        }
    }
    /// <summary>
    /// フェードインの処理
    /// </summary>
    private void FadeIn()
    {
        // フェードさせるかどうか
        if(_useFadeIn)
        {
            _loadDisplay.SetActive(false);
            _backGround.color -= new Color(0.0f, 0.0f, 0.0f, FADESPEED * Time.deltaTime);
            // 画面が明るくなったら
            if(_backGround.color.a <= 0.0f)
            {
                _backGround.gameObject.SetActive(false);
                _isFadeIn = false;
            }
        }
        else
        {
            _backGround.gameObject.SetActive(false);
            _isFadeIn = false;
        }
    }
    /// <summary>
    /// ロード(NowLoading)コールチン
    /// </summary>
    IEnumerator LoadScene()
    {
        // シーンの読み込みをする
        _async = SceneManager.LoadSceneAsync(_nextScreenName);
        
        // シーンの読み込み完了を自動で行われないようにする
        _async.allowSceneActivation = false;
        
        // progressが0.9以下(ロード中)の間繰り返す
        while(_async.progress < 0.9f)
        {
            // ロードの進行具合をパーセントで表す
            _percentage.text = (_async.progress * 100.0f).ToString("F0") + "%";
            
            // ロードの進行具合をスライダーに反映
            _progressbar.value = _async.progress;
        }
        _percentage.text = "100%";
        _progressbar.value = 1.0f;
        
        // 1秒間待機
        yield return new WaitForSeconds(1);

        // シーンの読み込み完了
        _async.allowSceneActivation = true;
        _isFadeIn = true;

        yield return null;
    }
}