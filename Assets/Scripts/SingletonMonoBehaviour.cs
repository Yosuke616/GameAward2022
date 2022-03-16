using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可変クラス型のシングルトンクラス
/// </summary>

public class SingletonMonoBehaviour<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T _instance = null;

    /// <summary>
    /// get-onlyなプロパティ
    /// <summary>
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                // typeof(T)型のオブジェクトを取得
                _instance = (T)FindObjectOfType(typeof(T));

                // 取得できなかった、nullだった場合
                if(_instance == null)
                {
                    Debug.LogError(typeof(T) + "がシーンに存在しません");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        CheckInstance();

        // Unityでは基本的にシーンを切り替えるとGameObject等は全部削除される
        // 下記のDontDestroyOnLoadは引数に指定したGameObjectは破棄されなくなり、
        // シーン切り替え時にそのまま引き継がれる・
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// シーン内に異なるインスタンスがあるかどうか
    /// </summary>
    private void CheckInstance()
    {
        // 異なるインスタンスがあった場合(最初に生成したインスタンス以外が存在したら
        if(this != Instance)
        {
            // 削除する
            Destroy(this.gameObject);
            return;
        }
    }

}
